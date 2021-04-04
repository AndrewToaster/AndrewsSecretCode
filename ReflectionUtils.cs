using System;
namespace AndrewsSecretCode
{
    public static class ReflectionUtils
    {
        public static void SetProperty<T>(object obj, string name, T value)
        {
            obj.GetType().GetProperty(name).SetValue(obj, value);
        }

        public static T GetProperty<T>(object obj, string name)
        {
            return (T)obj.GetType().GetProperty(name).GetValue(obj);
        }

        public static void SetField<T>(object obj, string name, T value)
        {
            obj.GetType().GetField(name).SetValue(obj, value);
        }

        public static T GetField<T>(object obj, string name)
        {
            return (T)obj.GetType().GetField(name).GetValue(obj);
        }
    }
}
