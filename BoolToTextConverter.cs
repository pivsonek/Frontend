using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace project
{
    /// <summary>
    /// Převádí hodnotu bool na odpovídající text "Zmenšit" nebo "Zvětšit".
    /// </summary>
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is bool isExpanded && isExpanded) ? "Zmenšit" : "Zvětšit";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing; // Zabrání chybě
        }
    }
}