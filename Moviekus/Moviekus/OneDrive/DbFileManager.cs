using Microsoft.Graph;
using Moviekus.EntityFramework;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Moviekus.OneDrive
{
    public static class DbFileManager
    {
		private static readonly string MoviekusFolderName = "/Moviekus/";

		public static async Task<bool> UploadDbToOneDrive()
		{
			try
			{
				var remotePath = MoviekusFolderName + MoviekusDefines.DbFileName;
				var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MoviekusDefines.DbFileName);

				LogManager.GetCurrentClassLogger().Info("Signing in to OneDrive...");
				await GraphClientManager.Ref.SignIn();

				LogManager.GetCurrentClassLogger().Info("Starting Db-Upload to OneDrive...");
				LogManager.GetCurrentClassLogger().Info($"Local path is: {localPath}");
				LogManager.GetCurrentClassLogger().Info($"Remote path is: {remotePath}");
				
				using (var stream = System.IO.File.OpenRead(localPath))
				{
					if (stream != null)
					{
						if (stream.Length > 4 * 1024 * 1024)    // Ab 4MB müssen Chunks übertragen werden
						{
							var session = await GraphClientManager.Ref.GraphClient.Drive.Root.ItemWithPath(remotePath).CreateUploadSession().Request().PostAsync();
							var maxSizeChunk = 320 * 4 * 1024;
							var provider = new ChunkedUploadProvider(session, GraphClientManager.Ref.GraphClient, stream, maxSizeChunk);
							var chunckRequests = provider.GetUploadChunkRequests();
							var exceptions = new List<Exception>();
							DriveItem itemResult = null;
							foreach (var request in chunckRequests)
							{
								var result = await provider.GetChunkRequestResponseAsync(request, exceptions);
								if (result.UploadSucceeded)
									itemResult = result.ItemResponse;
							}

							// Check that upload succeeded
							if (itemResult == null)
								await UploadDbToOneDrive();
						}
						else await GraphClientManager.Ref.GraphClient.Drive.Root.ItemWithPath(remotePath).Content.Request().PutAsync<DriveItem>(stream);
					}
				}
			}
			catch (Exception ex)
			{
				LogManager.GetCurrentClassLogger().Error(ex);
				return false;
			}
			
			LogManager.GetCurrentClassLogger().Info("Finished Db-Upload to OneDrive.");
			return true;
		}

		public static async Task<bool> DownloadDbFromOneDrive()
		{
			var remotePath = MoviekusFolderName + MoviekusDefines.DbFileName;
			var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MoviekusDefines.DbFileName);

			try
			{
				LogManager.GetCurrentClassLogger().Info("Signing in to OneDrive...");
				await GraphClientManager.Ref.SignIn();

				LogManager.GetCurrentClassLogger().Info("Starting Db-Download from OneDrive...");
				LogManager.GetCurrentClassLogger().Info($"Local path is: {localPath}");
				LogManager.GetCurrentClassLogger().Info($"Remote path is: {remotePath}");

				var stream = await GraphClientManager.Ref.GraphClient.Drive.Root.ItemWithPath(remotePath).Content.Request().GetAsync();
				System.IO.File.WriteAllBytes(localPath, GetStreamBytes(stream));

				// Migrationen durchführen, falls eine alte DB geladen wurde
				using (var context = new MoviekusDbContext())
				{
					context.Migrate();					
				}
			}
			catch (Exception ex)
			{
				LogManager.GetCurrentClassLogger().Error(ex);
				return false;
			}

			LogManager.GetCurrentClassLogger().Info("Finished Db-Download from OneDrive.");

			return true;
		}

		private static byte[] GetStreamBytes(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

	}
}
