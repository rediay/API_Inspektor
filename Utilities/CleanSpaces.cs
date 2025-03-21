using System.Linq;

namespace Inspektor_API_REST.Utilities
{
    public static class CleanSpaces
    {
        public static void TrimAllStrings<T>(this T obj)
        {
            var stringProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var prop in stringProperties)
            {
                var value = (string)prop.GetValue(obj, null);
                if (value != null)
                {
                    prop.SetValue(obj, value.Trim(), null);
                }
            }
        }
    }
}
