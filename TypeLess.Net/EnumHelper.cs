using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Net
{
    public class EnumAttr
    {
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public string GroupName { get; set; }
        public int Order { get; set; }
        public string Prompt { get; set; }
        public string ShortName { get; set; }

        public EnumAttr(string name, string description, string groupName, int order, string prompt, string shortName)
        {
            DisplayName = name ?? String.Empty;
            Description = description ?? String.Empty;
            GroupName = groupName ?? String.Empty;
            Order = order;
            Prompt = prompt ?? String.Empty;
            ShortName = shortName ?? String.Empty;
        }

        public static EnumAttr Empty
        {
            get
            {
                return new EnumAttr(String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty);
            }
        }
    }

    public static class EnumHelper
    {

        public static string GetDescription(this Enum value)
        {
            if (value == null) {
                return String.Empty;
            }

            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }

        public static EnumAttr GetDisplayAttributes(this Enum value)
        {
            if (value == null)
            {
                return EnumAttr.Empty;
            }


            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length > 0)
            {
                var attr = attributes[0];
                return new EnumAttr(attr.Name, attr.Description, attr.GroupName, attr.Order, attr.Prompt, attr.ShortName);
            }
            else
            {
                return EnumAttr.Empty;
            }

        }

    }
}
