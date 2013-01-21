using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibConfig
{
    public class ConfigUtilities
    {
        /// <summary>
        /// Converts a setting string back into an array.  (Only works with primative types).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] StringToArray<T>(string value)
        {
            string[] items = value.TrimStart('{').TrimEnd('}').Split(',');
            T[] array = new T[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                array[i] = (T)Convert.ChangeType(items[i], typeof(T));
            }

            return array;
        }

        /// <summary>
        /// Converts an array back into a setting string.  (Only works with primative types).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayToString<T>(T[] array)
        {
            string value = "{ ";

            for (int i = 0; i < array.Length; i++)
            {
                if (i > 0)
                {
                    value += ", ";
                }

                value += Convert.ToString(array[i]);
            }

            value += " }";

            return value;
        }
    }
}
