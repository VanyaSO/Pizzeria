using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Pizzeria.Helpers;

public static class ApplicationExtensions
{
    public static string GetDescription(this Enum myEnum)
    {
        return myEnum.GetType()
            .GetMember(myEnum.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DisplayAttribute>().Name;
    }
}