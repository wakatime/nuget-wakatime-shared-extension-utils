using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WakaTime.Shared.ExtensionUtils
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum e)
        {
            DescriptionAttribute attr = (DescriptionAttribute)e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return attr?.Description ?? e.ToString();
        }
    }
}
