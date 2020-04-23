using System;
using System.Windows;
using CertificateUpdater.Managers;
using CertificateUpdater.Views;
using Prism.Ioc;

namespace CertificateUpdater {

    public partial class App {

        protected override Window CreateShell() {
            // Disable Aero for Windows 8 and later, so the newer, nicer Metro-looking WPF theme is used
            if (Environment.OSVersion.Version >= Version.Parse("6.2")) {
                Current.Resources.MergedDictionaries.Clear();
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton(typeof(CertificateManager), typeof(CertificateManagerImpl));
        }

    }

}