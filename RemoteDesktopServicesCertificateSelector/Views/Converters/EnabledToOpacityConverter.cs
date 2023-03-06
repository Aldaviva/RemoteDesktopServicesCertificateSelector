#nullable enable

using System;
using System.Globalization;
using System.Windows.Data;

namespace RemoteDesktopServicesCertificateSelector.Views.Converters; 

public class EnabledToOpacityConverter: IValueConverter {

    private const double ENABLED_OPACITY  = 1.0;
    private const double DISABLED_OPACITY = 0.5;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            bool v => v ? ENABLED_OPACITY : DISABLED_OPACITY,
            _      => ENABLED_OPACITY
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            double v => v >= ENABLED_OPACITY,
            _        => true
        };
    }

}