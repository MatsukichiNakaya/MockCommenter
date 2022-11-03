using System;
using System.Globalization;
using System.Windows.Data;

namespace MockCommenter
{
	public class IconSizeConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			var size = (Double)value;
			return size + 20;
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
