using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PromptMaker.Common.Converters
{
    [System.Windows.Data.ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : System.Windows.Data.IValueConverter
    {

        #region IValueConverter メンバ
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var target = (bool)value;

            if (target)
            {
                // ここに処理を記述する
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        // TwoWayの場合に使用する
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
