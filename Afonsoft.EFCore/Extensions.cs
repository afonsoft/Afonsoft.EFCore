using System;

namespace Afonsoft.EFCore
{
    public static class Extensions
    {
        /// <summary>
        /// ToEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// ToEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            T result = defaultValue;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }
    }
}
