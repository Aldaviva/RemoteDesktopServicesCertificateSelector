#nullable enable

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RemoteDesktopServicesCertificateSelector.Views.Converters;

internal class ColumnSortDirectionToVisibilityConverter: IValueConverter {

    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture) {
        if (targetType != typeof(Visibility)) {
            return DependencyProperty.UnsetValue;
        } else if (value == null) {
            return Visibility.Collapsed;
        } else {
            return Visibility.Visible;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}