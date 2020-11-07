using System;
using System.Linq;
using System.Reflection;

namespace PokerGameCore.Util
{
    public class AttributeUtil
    {

        /// <summary>
        /// Get the attribute value for the property name
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="propertyName"></param>
        public static object GetAttributeValue<T>(Type propertyType, string propertyName, string attributeName) where T : Attribute
        {
            var info = propertyType.GetMember(propertyName).FirstOrDefault();
            if (info == null)
            {
                return string.Empty;
            }
            var attribute = info.GetCustomAttribute<T>();
            return attribute.GetType().GetProperty(attributeName).GetValue(attribute, null);
        }
    }
}
