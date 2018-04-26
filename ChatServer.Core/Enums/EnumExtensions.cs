using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description attribute of the enum
        /// </summary>
        /// <returns></returns>
        public static string GetDescription<T>(this T scope)
        {
            FieldInfo fieldInfo = scope.GetType().GetField(scope.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
    }
}
