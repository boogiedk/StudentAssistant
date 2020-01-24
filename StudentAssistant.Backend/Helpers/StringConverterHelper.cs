using System;

namespace StudentAssistant.Backend.Helpers
{
    public static class StringConverterHelper
    {
        /// <summary>
        /// Парсит строку с названием дня недели в DayOfWeek енум.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DayOfWeek ToDayOfWeek(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new NullReferenceException();
            }

            switch (str.ToLower())
            {
                case "понедельник":
                    return DayOfWeek.Monday;
                case "вторник":
                    return DayOfWeek.Tuesday;
                case "среда":
                    return DayOfWeek.Wednesday;
                case "четверг":
                    return DayOfWeek.Thursday;
                case "пятница":
                    return DayOfWeek.Friday;
                case "суббота":
                    return DayOfWeek.Saturday;
                case "воскресенье":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Делает заглавной первую букву в слове (строке).
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        
        
    }
}