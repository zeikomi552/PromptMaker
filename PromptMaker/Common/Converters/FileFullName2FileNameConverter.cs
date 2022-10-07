using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PromptMaker.Common.Converters
{
    [System.Windows.Data.ValueConversion(typeof(string), typeof(string))]
    public class FileFullName2FileNameConverter : System.Windows.Data.IValueConverter
    {

        #region IValueConverter メンバ
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var target = (string)value;

            if (!string.IsNullOrEmpty(target))
            {
                return System.IO.Path.GetFileName(target);
            }
            else
            {
                return string.Empty;
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
