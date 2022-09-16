using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PromptMaker.Common.Extensions
{
    static class ScriptTypeEnumExtensions
    {
        public static string GetName<T>(this T Value) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (!(typeof(T).IsEnum))
            {
                throw new InvalidEnumArgumentException();
            }

            var fieldInfo = Value.GetType().GetField(Value.ToString()!);
            if (fieldInfo == null) return String.Empty;

            var attr = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute))!;
            if (attr == null) return "";
            return attr.Description;
        }
    }
}
