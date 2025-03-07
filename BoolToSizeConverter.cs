using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace project
{
    /// <summary>
    /// Převádí bool hodnotu na šířku nebo výšku grafu.
    /// </summary>
    public class BoolToSizeConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isExpanded = value is bool b && b;

            if (parameter is string param)
            {
                return param switch
                {
                    "height" => isExpanded ? 600 : 400,
                    "width" => isExpanded ? 900 : -1,
                    _ => -1
                };
            }

            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing; // Zabrání chybě
        }
    }
}