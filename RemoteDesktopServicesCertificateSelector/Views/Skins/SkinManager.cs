#nullable enable

using System.Windows;
using Dark.Net;

namespace RemoteDesktopServicesCertificateSelector.Views.Skins;

internal static class SkinManager {

    public static void register(ResourceDictionary lightModeResources, ResourceDictionary darkModeResources) {
        ResourceDictionary darkOrLightModeResources = new();
        darkOrLightModeResources.MergedDictionaries.Add(DarkNet.Instance.EffectiveCurrentProcessThemeIsDark ? darkModeResources : lightModeResources);
        Application.Current.Resources.MergedDictionaries.Add(darkOrLightModeResources);

        DarkNet.Instance.EffectiveCurrentProcessThemeIsDarkChanged += (_, isDarkMode) => { darkOrLightModeResources.MergedDictionaries[0] = isDarkMode ? darkModeResources : lightModeResources; };
    }

}