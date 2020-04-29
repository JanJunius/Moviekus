using Acr.UserDialogs;
using Moviekus.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Moviekus.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Bootstrapper.Init();

            InitializeNLog();
            LoadApplication(new Moviekus.App());
        }

        private void InitializeNLog()
        {
            var assembly = this.GetType().Assembly;
            var assemblyName = assembly.GetName().Name;
            new LogService().Initialize(assembly, assemblyName);
        }
    }
}
