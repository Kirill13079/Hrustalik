using System;
using System.Text;

namespace HealthApp.Extensions
{
    public static class DateExtension
    {
        /// <summary>
        /// Сравнивает предоставленную дату с текущей датой и генерирует строку
        /// ("5 дней назад", "1 секунду назад")
        /// </summary>
        /// <param name="value">The date to convert</param>
        /// <param name="approximate">Когда выключено, рассчитывает временной интервал с точностью до секунды.
        /// Когда включено, приближается к наибольшей круглой единице времени.</param>
        /// <returns></returns>
        public static string ToRelativeDateString(this DateTime value, bool approximate)
        {
            StringBuilder sb = new StringBuilder();

            string suffix = (value > DateTime.Now) ? " " : " назад";

            TimeSpan timeSpan = new TimeSpan(Math.Abs(DateTime.Now.Subtract(value).Ticks));

            if (timeSpan.Days > 0)
            {
                sb.AppendFormat("{0} {1}", timeSpan.Days, timeSpan.Days.Generate("день", "дня", "дней"));

                if (approximate)
                {
                    return sb.ToString() + suffix;
                }
            }
            if (timeSpan.Hours > 0)
            {
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                  timeSpan.Hours, timeSpan.Hours.Generate("час", "часа", "часов"));

                if (approximate)
                {
                    return sb.ToString() + suffix;
                }
            }
            if (timeSpan.Minutes > 0)
            {
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                  timeSpan.Minutes, timeSpan.Minutes.Generate("минута", "минуты", "минут"));

                if (approximate)
                {
                    return sb.ToString() + suffix;
                }
            }
            if (timeSpan.Seconds > 0)
            {
                sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                    timeSpan.Seconds, timeSpan.Seconds.Generate("секунда", "секунды", "секунд"));

                if (approximate)
                {
                    return sb.ToString() + suffix;
                }
            }
            if (sb.Length == 0) return "только что";

            sb.Append(suffix);
            return sb.ToString();
        }
    }
}
