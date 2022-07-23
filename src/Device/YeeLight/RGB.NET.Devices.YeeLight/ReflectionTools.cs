using System.Reflection;

namespace RGB.NET.Devices.YeeLight
{
    public class ReflectionTools
    {
        public static T GetPrivateProperty<T>(object obj, string propertyName)
        {
            return (T)obj.GetType()
                          .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                          .GetValue(obj);
        }

        public static void SetPrivateProperty<T>(object obj, string propertyName, T value)
        {
            obj.GetType()
               .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(obj, value);
        }

        public static T GetPrivateField<T>(object obj, string propertyName)
        {
            return (T)obj.GetType()
                          .GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                          .GetValue(obj);
        }

        public static void SetPrivateField<T>(object obj, string propertyName, T value)
        {
            obj.GetType()
               .GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(obj, value);
        }
    }
}
