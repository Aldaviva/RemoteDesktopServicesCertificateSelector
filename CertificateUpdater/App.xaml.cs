using System;
using System.Windows;
using CertificateUpdater.Managers;
using CertificateUpdater.Views;
using Prism.Ioc;

namespace CertificateUpdater {

    public partial class App {

        /*protected override void OnStartup(StartupEventArgs e) {
            // https://spin.atomicobject.com/2013/12/11/wpf-data-binding-debug/
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
            base.OnStartup(e);
        }*/

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

    /*public class DebugTraceListener: TraceListener {

        public override void Write(string message) { }

        public override void WriteLine(string message) {
            // Debugger.Break();
        }

    }*/

}