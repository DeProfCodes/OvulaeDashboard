using System.Globalization;

namespace OvulaeDashboard.Helpers
{
    public class ConversionsHelpers
    {
        public static string GetFormatedThousands(double count, int precision = 0, string sign = null, string separator = " ")
        {
            // Format without decimals, using spaces as thousand separators
            var culture = new CultureInfo("en-ZA");
            culture.NumberFormat.NumberGroupSeparator = separator;
            culture.NumberFormat.NumberDecimalDigits = precision;

            var formatted = count.ToString("N", culture);

            if (!string.IsNullOrEmpty(sign) && count > 0)
            {
                return $"{sign}{formatted}";
            }

            return formatted;
        }

        public static string GetAmountInCurrency(double amount, string currency, int precision = 0, string sign = null, string separator = " ")
        {
            return $"{currency}{GetFormatedThousands(amount, precision, sign, separator)}";
        }

        public static string GetFormatedPercentage(double rate, int precision = 2, string sign = null)
        {
            var percent = $"{Math.Round(rate, precision)}%";
            var rateStr = (sign != null && rate > 0) ? $"{sign}{percent}" : percent;

            return rateStr;
        }
    }
}
