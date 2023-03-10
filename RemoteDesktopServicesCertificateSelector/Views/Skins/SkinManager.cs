#nullable enable

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dark.Net;

namespace RemoteDesktopServicesCertificateSelector.Views.Skins;

internal static class SkinManager {

    public static void register(Uri lightModeResources, Uri darkModeResources) {
        Collection<ResourceDictionary> appResources         = Application.Current.Resources.MergedDictionaries;
        ResourceDictionary?            skinResources        = appResources.FirstOrDefault(r => r.Source.Equals(lightModeResources) || r.Source.Equals(darkModeResources));
        Uri                            currentModeResources = DarkNet.Instance.EffectiveCurrentProcessThemeIsDark ? darkModeResources : lightModeResources;

        if (skinResources != null) {
            skinResources.Source = currentModeResources;
        } else {
            skinResources = new ResourceDictionary { Source = currentModeResources };
            appResources.Add(skinResources);
        }

        DarkNet.Instance.EffectiveCurrentProcessThemeIsDarkChanged += (_, isDarkMode) => { skinResources.Source = isDarkMode ? darkModeResources : lightModeResources; };
    }

}