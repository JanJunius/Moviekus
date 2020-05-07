using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Moviekus.Logging;
using Android.Content;
using Microsoft.Identity.Client;
using Moviekus.OneDrive;
using System.IO;
using System.Threading.Tasks;

namespace Moviekus.Droid
{
    [Activity(Label = "Moviekus", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static readonly int PickImageRequestCode = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Instance = this;

            Bootstrapper.Init();
            UserDialogs.Init(this);

            InitializeNLog();
            LoadApplication(new App());

            GraphClientManager.Ref.AuthUIParent = this;
        }
        
        // Für die Berechtigungsanforderung des GraphClients (OneDrive) erforderlich
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageRequestCode)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else PickImageTaskCompletionSource.SetResult(null);
            }
            else
            {
                // One-Drive-Anmeldung abgeschlossen
                AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, intent);
            }
        }

        private void InitializeNLog()
        {
            var assembly = this.GetType().Assembly;
            var assemblyName = assembly.GetName().Name;
            new LogService().Initialize(assembly, assemblyName);
        }
    }
}