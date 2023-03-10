#nullable enable

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RemoteDesktopServicesCertificateSelector.Views.Converters;

public class ColumnSortDirectionToRotationConverter: IValueConverter {

    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture) {
        double angle;
        if (targetType == typeof(double) && value is ListSortDirection direction) {
            angle = direction == ListSortDirection.Ascending ? 270 : 90;
        } else {
            angle = 0;
        }

        return angle;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return value;
    }

}