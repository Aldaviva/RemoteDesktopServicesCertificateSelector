using System;
using System.Linq;
using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using RemoteDesktopServicesCertificateSelector.Managers;
using RemoteDesktopServicesCertificateSelector.Views;

namespace RemoteDesktopServicesCertificateSelector {

    public partial class App {

        protected override Window CreateShell() {
            // Disable Aero for Windows 8 and later, so the newer, nicer Metro-looking WPF theme is used
            if (Environment.OSVersion.Version >= Version.Parse("6.2")) {
                const string WIN7_AERO = "/themes/Aero.NormalColor.xaml"; //exact URI changes in Release build thanks to ILRepack
                Current.Resources.MergedDictionaries.Remove(Current.Resources.MergedDictionaries.First(dictionary => dictionary.Source.ToString().EndsWith(WIN7_AERO)));
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            containerRegistry.RegisterSingleton<CertificateManager, CertificateManagerImpl>();
        }

        protected override void OnExit(ExitEventArgs e) {
            ((DryIocContainerExtension) Container).Instance.Dispose();
            base.OnExit(e);
        }

        /*protected override void OnStartup(StartupEventArgs e) {
            // https://spin.atomicobject.com/2013/12/11/wpf-data-binding-debug/
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            // PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Information | SourceLevels.Warning | SourceLevels.Error;
            base.OnStartup(e);
        }*/

    }

    /*public class DebugTraceListener: TraceListener {

        public override void Write(string message) { }

        public override void WriteLine(string message) {
            // Debugger.Break();
        }

    }*/

}