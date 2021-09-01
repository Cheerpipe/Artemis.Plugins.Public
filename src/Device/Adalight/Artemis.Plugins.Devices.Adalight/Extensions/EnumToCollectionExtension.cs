using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace Artemis.Plugins.Devices.Adalight.Extensions
{
    public class EnumToCollectionExtension : MarkupExtension
    {
        public Type EnumType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null) throw new ArgumentNullException(nameof(EnumType));

            return Enum.GetValues(EnumType).Cast<Enum>().Select(EnumToInt);
        }

        private int EnumToInt(Enum value)
        {
            return Convert.ToInt32(value.GetType().GetField(value.ToString())
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Cast<DescriptionAttribute>()
                       .FirstOrDefault()?.Description ?? value.ToString());
        }

    }
}
