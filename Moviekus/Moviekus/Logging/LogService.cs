using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Moviekus.Logging
{
    public class LogService : ILogService
    {
        public void Initialize(Assembly assembly, string assemblyName)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var nlogConfigFile = GetEmbeddedResourceStream(assembly, "NLog.config");
                if (nlogConfigFile != null)
                {
                    var xmlReader = System.Xml.XmlReader.Create(nlogConfigFile);
                    NLog.LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
                }
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                var nlogConfigFile = GetEmbeddedResourceStream(assembly, "NLog.config");
                if (nlogConfigFile != null)
                {
                    var xmlReader = System.Xml.XmlReader.Create(nlogConfigFile);
                    NLog.LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);

                    var storageFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    NLog.GlobalDiagnosticsContext.Set("LogPath", storageFolder + "\\");
                }
            }
            else
            {
                throw new Exception("Could not initialize Logger: Unknonw Platform");
            }

        }

        private Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourcePaths = assembly.GetManifestResourceNames()
              .Where(x => x.EndsWith(resourceFileName, StringComparison.OrdinalIgnoreCase))
              .ToList();

            if (resourcePaths.Count == 1)
            {
                return assembly.GetManifestResourceStream(resourcePaths.Single());
            }

            return null;
        }
    }
}
