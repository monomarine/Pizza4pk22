using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Pizza.Converters
{
    internal class ValueToVisibilityConverter : IValueConverter
    {
        public bool Negate { get; set; }
        public Visibility FalseVisibility { get; set; }
        public ValueToVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }
        #region IValieConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue;
            bool result = bool.TryParse(value.ToString(), out boolValue);
            if (!result) return value;

            if (boolValue && Negate) return FalseVisibility;
            if (boolValue && !Negate) return Visibility.Visible;
            if (!boolValue && Negate) return Visibility.Visible;
            if (!boolValue && !Negate) return FalseVisibility;

            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
