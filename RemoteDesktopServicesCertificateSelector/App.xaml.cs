#nullable enable

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Dark.Net;
using Prism.DryIoc;
using Prism.Ioc;
using RemoteDesktopServicesCertificateSelector.Managers;
using RemoteDesktopServicesCertificateSelector.Views;

namespace RemoteDesktopServicesCertificateSelector;

public partial class App {

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        DarkNet.Instance.SetCurrentProcessTheme(Theme.Auto);

        // Disable Aero for Windows 8 and later, so the newer, nicer Metro-looking WPF theme (Aero2) is used
        if (Environment.OSVersion.Version < new Version(6, 2)) {
            Current.Resources.MergedDictionaries.Add(getResourceDictionary("PresentationFramework.Aero", "themes/Aero.NormalColor.xaml"));
        }

        // TODO uncomment this
        // SkinManager.register(
        //     getResourceDictionary("RemoteDesktopServicesCertificateSelector", "Views/Skins/Skin.Light.xaml"),
        //     getResourceDictionary("RemoteDesktopServicesCertificateSelector", "Views/Skins/Skin.Dark.xaml"));
    }

    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
        containerRegistry.RegisterSingleton<CertificateManager, CertificateManagerImpl>();
    }

    protected override void OnExit(ExitEventArgs e) {
        ((DryIocContainerExtension) Container).Instance.Dispose();
        base.OnExit(e);
    }

    private static ResourceDictionary getResourceDictionary(string unpackedAssemblyName, string resourceDictionaryXamlPath) {
        resourceDictionaryXamlPath = Uri.EscapeUriString(resourceDictionaryXamlPath.TrimStart('/'));
        unpackedAssemblyName       = Uri.EscapeUriString(unpackedAssemblyName);
        string executingAssembly = Uri.EscapeUriString(Assembly.GetExecutingAssembly().GetName().Name);

        ResourceDictionary resourceDictionary = new();
        try {
            resourceDictionary.Source = new Uri($"pack://application:,,,/{executingAssembly};component/{unpackedAssemblyName}/{resourceDictionaryXamlPath}", UriKind.Absolute);
        } catch (IOException) {
            resourceDictionary.Source = new Uri($"pack://application:,,,/{unpackedAssemblyName};component/{resourceDictionaryXamlPath}", UriKind.Absolute);
        }

        return resourceDictionary;
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