using System.Reflection;

namespace ArSeqProduct
{

    public static class SPA  //StaticPropertyAccessor
    {
        public static object? GetValue(string propertyName)
        {
            var type = typeof(Params);
            var prop = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public);
            return prop?.GetValue(null);
        }

        public static bool SetValue(string propertyName, object value)
        {
            var type = typeof(Params);
            var prop = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public);
            if (prop != null && prop.CanWrite)
            {

 //               prop.SetValue(null, Convert.ChangeType(value, prop.PropertyType));
                return true;
            }
            return false;
        }

    }
}
