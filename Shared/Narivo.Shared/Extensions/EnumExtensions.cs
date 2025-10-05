using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Shared.Extensions;
public static class EnumExtensions
{
    public static string ToDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
        {
            return attr.Description;
        }
        return value.ToString(); // Eğer description yoksa enum adını döner
    }
}
