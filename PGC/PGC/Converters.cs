using PGC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PGC
{
    [ValueConversion(typeof(PlayerGroup), typeof(Brush))]
    internal class IsGroupSelectedToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var currentGroup = value as PlayerGroup;
            //var hex = "#f4efe1";
            //Brush? brush = (Brush)new BrushConverter().ConvertFromString(hex);
            return currentGroup.IsSelected ? Brushes.Purple : Brushes.Red;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(PlayerGroup), typeof(Brush))]
    internal class IsGroupSelectedToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedGroup = value as PlayerGroup;
            var currentGroup = parameter as PlayerGroup;
            return selectedGroup == currentGroup ? Brushes.LightBlue : Brushes.White;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
