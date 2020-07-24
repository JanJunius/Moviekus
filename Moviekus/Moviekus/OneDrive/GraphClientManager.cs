using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using System.Net.Http.Headers;
using System.Diagnostics;
using NLog;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ServiceContracts;

namespace Moviekus.OneDrive
{
    public class GraphClientManager
    {
        // UIParent used by Android version of the App
        public object AuthUIParent = null;

        // KeyChain security group used by iOS version of the App
        public string iOSKeychainSecurityGroup = null;

        // Microsoft Authentication client for native/mobile Apps
        public IPublicClientApplication PCA;

        // Microsoft Graph client
        public GraphServiceClient GraphClient;

        public bool IsSignedIn { get; set; }

        public bool IsSignedOut { get; set; }

        private static Settings Settings;


        public static GraphClientManager Ref
        {
            get
            {
                if (TheInstance == null)
                {
                    TheInstance = new GraphClientManager();
                    TheInstance.InitInstance();
                }
                return TheInstance;
            }
        }
        private readonly string[] Scopes = "files.readwrite".Split(' ');

        private static GraphClientManager TheInstance = null;

        private GraphClientManager() { }

        public async Task SignIn()
        {
            // First, attempt silent sign in
            // If the user's information is already in the App's cache,
            // they won't have to sign in again.
            try
            {
                if (PCA == null)
                    InitInstance();

                var accounts = await PCA.GetAccountsAsync();

                var silentAuthResult = await PCA
                    .AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                LogManager.GetCurrentClassLogger().Info($"Successful silent authentication for: {silentAuthResult.Account.Username}");
            }
            catch (MsalUiRequiredException msalEx)
            {
                // This exception is thrown when an interactive sign-in is required.
                LogManager.GetCurrentClassLogger().Warn(msalEx);

                // Prompt the user to sign-in
                var interactiveRequest = PCA.AcquireTokenInteractive(Scopes);

                if (AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(AuthUIParent);
                }

                var interactiveAuthResult = await interactiveRequest.ExecuteAsync();
                LogManager.GetCurrentClassLogger().Info($"Successful interactive authentication for: {interactiveAuthResult.Account.Username}");
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }

            await InitializeGraphClientAsync();
        }

        public async Task SignOut()
        {
            try
            {
                // Get all cached accounts for the App (Should only be one)
                var accounts = await PCA.GetAccountsAsync();
                while (accounts.Any())
                {
                    // Remove the account info from the cache
                    await PCA.RemoveAsync(accounts.First());
                    accounts = await PCA.GetAccountsAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
            IsSignedIn = false;
        }

        private async Task InitializeGraphClientAsync()
        {
            var currentAccounts = await PCA.GetAccountsAsync();
            try
            {
                if (currentAccounts.Count() > 0)
                {
                    // Initialize Graph client
                    GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            var result = await PCA.AcquireTokenSilent(Scopes, currentAccounts.FirstOrDefault())
                                .ExecuteAsync();

                            requestMessage.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", result.AccessToken);
                        }));

                    IsSignedIn = true;
                }
                else
                {
                    IsSignedIn = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                LogManager.GetCurrentClassLogger().Warn($"Accounts in the MSAL cache: {currentAccounts.Count()}.");
            }
        }

        private void InitInstance()
        {
            try
            {
                Settings = Resolver.Resolve<ISettingsService>().GetSettings();

                var builder = PublicClientApplicationBuilder
                    .Create(Settings.OneDriveApplicationId)
                    .WithRedirectUri("msauth://com.companyname.Moviekus");

                if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
                {
                    builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
                }

                PCA = builder.Build();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
            }
        }
    }

}
