using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MockCommenter
{
    public class BrushConverter : IMultiValueConverter
    {
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
            => Color.FromArgb(System.Convert.ToByte(values[0]), System.Convert.ToByte(values[1]),
                System.Convert.ToByte(values[2]), System.Convert.ToByte(values[3]));

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            var C = (Color)value;
            return new Object[] { (Double)C.ScA, (Double)C.ScR, (Double)C.ScG, (Double)C.ScB };
        }
    }
}
