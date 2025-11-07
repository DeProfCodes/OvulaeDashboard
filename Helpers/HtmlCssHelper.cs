using OvulaeDashboard.Helpers.Constants;
using OvulaeDashboard.Helpers.Enums;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;
using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OvulaeDashboard.Helpers
{
    public class HtmlCssHelper
    {
        public static string GetBadgeCssForStatus(StatusType status)
        {
            if (status == StatusType.Paid || status == StatusType.Active) return "success";
            if (status == StatusType.Pending || status == StatusType.Unpaid) return "warning";
            if (status == StatusType.Terminated || status == StatusType.Deleted) return "danger";
            if (status == StatusType.Cancelled || status == StatusType.Closed) return "secondary";

            return "";
        }

        public static string GetBadgeCssForAccountStatus(AccountStatusType status)
        {
            if (status == AccountStatusType.Active) return "success";
            if (status == AccountStatusType.InActive || status == AccountStatusType.PendingAffiliate) return "warning";
            if (status == AccountStatusType.Suspended || status == AccountStatusType.RejectedAffiliate) return "danger";
            if (status == AccountStatusType.Paused) return "secondary";

            return "secondary";
        }

        public static string GetBadgeCssForRole(UserRoleType role)
        {
            if (role == UserRoleType.Affiliate) return "user-affiliate-clr";
            if (role == UserRoleType.Client) return "user-premium-clr";
            if (role == UserRoleType.Doctor) return "user-doctor-clr";
            if (role == UserRoleType.Admin) return "user-admin-clr";

            return "";
        }

        public static string GetBadgeCssForTransactionType(TransactionType transactionType)
        {
            if (transactionType == TransactionType.Deposit) return "primary";
            if (transactionType == TransactionType.DepositTopUp) return "warning";
            if (transactionType == TransactionType.Withdrawal) return "danger";

            return "";
        }

        public static string GetTimeZonedTime(DateTime dateTime)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            return localTime.ToString("dd-MM-yyyy HH:mm");
        }

        public static string GetFormatedAmount(double amount, string sign = null, bool isRands = true)
        {
            var currency = isRands ? "R" : "$";
            var money = $"{currency}{amount.ToString("N2", new CultureInfo("en-ZA"))}";
            var amountStr = (sign != null && amount > 0) ? $"{sign}{money}" : money;
            
            return amountStr;
        }

        public static string GetFormatedThousands(double count, string sign = null)
        {
            // Format without decimals, using spaces as thousand separators
            var culture = new CultureInfo("en-ZA");
            culture.NumberFormat.NumberGroupSeparator = " ";
            culture.NumberFormat.NumberDecimalDigits = 0;

            var formatted = count.ToString("N0", culture);

            if (!string.IsNullOrEmpty(sign) && count > 0)
            {
                return $"{sign}{formatted}";
            }

            return formatted;
        }

        public static string GetFormatedPercentageDirect(double rate, int precision = 2, string sign = null)
        {
            var percent = $"{Math.Round(rate, precision)}%";
            var rateStr = (sign != null && rate > 0) ? $"{sign}{percent}" : percent;

            return rateStr;
        }

        public static string GetFormatedPercentageInDirect(double value, double total, int precision = 2, string sign = null)
        {
            var rate = (value / total) * 100.0;
            return GetFormatedPercentageDirect(rate, precision, sign);
        }

        public static (string countryName, string flagCode) GetCountryAndFlag(string countryCode)
        {
            var name = CountryCodeFunctions.GetCountryNameFromCode(countryCode);
            var code = CountryCodeFunctions.GetCountryFlagCodeFromCode(countryCode);

            return (name, code);
        }

        public static string GetCountryAndFlagDiv(string countryCode, bool showCountryName = true)
        {
            var countryName = CountryCodeFunctions.GetCountryNameFromCode(countryCode);
            var flagCode = CountryCodeFunctions.GetCountryFlagCodeFromCode(countryCode);

            var result = $@"<div>
                          		<img src='https://flagcdn.com/16x12/{flagCode}.png' srcset='https://flagcdn.com/32x24/{flagCode}.png 2x,https://flagcdn.com/48x36/{flagCode}.png 3x' width='16' height='12' alt='{countryName}'>
                          		{(showCountryName ? $"<span class='text-black'>{countryName}</span>" : "")}
                          </div>
                          ";

            return result;
        }

        public static string GetColorForRating(int? rating2)
        {
            int rating = rating2 != null ? rating2.Value : 0;

            // Red (1) → Orange (5) → Green (10)
            if (rating <= 5)
            {
                double t = (rating - 1) / 4.0; // 0–1 from red to orange
                int r = 255;
                int g = (int)(128 * t);  // blend red→orange
                int b = 0;
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            else
            {
                double t = (rating - 5) / 5.0; // 0–1 from orange to green
                int r = (int)(255 * (1 - t));
                int g = (int)(128 + 127 * t);  // blend orange→green
                int b = 0;
                return $"#{r:X2}{g:X2}{b:X2}";
            }
        }
    }
}
