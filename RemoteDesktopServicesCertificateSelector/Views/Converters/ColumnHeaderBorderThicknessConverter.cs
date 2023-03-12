using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RemoteDesktopServicesCertificateSelector.Views.Converters;

public class ColumnHeaderBorderThicknessConverter: IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (targetType == typeof(Thickness) && value is int displayIndex) {
            return new Thickness(displayIndex == 0 ? 0 : 1, 0, 0, 0);
        } else {
            return value;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}