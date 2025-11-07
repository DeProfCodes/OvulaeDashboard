using OvulaeDashboard.ViewModels.Shared;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.User;
using OvulaeShared.Models.Affiliate;
using OvulaeShared.Models.User;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.Helpers
{
    public class ChartsDataHelper
    {
        public static List<List<ChartViewModel>> GetAffiliateEarningsOverTime(List<UserAffiliateReferalDetails> referals, bool calculateNet, double exchangeRate = 0)
        {
            var result = new List<List<ChartViewModel>>();
            var allTimeSorted = referals.OrderBy(r => r.JoinDate).ToList();
            double runningTotal = 0;

            // TODAY (hourly)
            var today = DateTime.Today;
            var todayData = allTimeSorted
                .Where(r => r.JoinDate.Date == today)
                .GroupBy(r => r.JoinDate.Hour)
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    double value1 = g.Sum(x => x.Commission);
                    runningTotal += value1;

                    return new ChartViewModel
                    {
                        Label = $"{g.Key}:00",
                        Value1 = (int)value1,
                        Value2 = calculateNet ? (int)runningTotal : 0,
                        DurationFilter = "Today"
                    };
                }).ToList();

            result.Add(todayData);
            runningTotal = 0;

            // LAST 7 DAYS (daily)
            var start7Days = today.AddDays(-6);
            var last7DaysData = allTimeSorted
                .Where(r => r.JoinDate.Date >= start7Days)
                .GroupBy(r => r.JoinDate.DayOfWeek)
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    double value1 = g.Sum(x => x.Commission);
                    runningTotal += value1;

                    return new ChartViewModel
                    {
                        Label = g.Key.ToString(),
                        Value1 = (int)value1,
                        Value2 = calculateNet ? (int)runningTotal : 0,
                        DurationFilter = "Last7Days"
                    };
                }).ToList();

            result.Add(last7DaysData);
            runningTotal = 0;

            // LAST MONTH (weekly buckets)
            var startMonth = today.AddMonths(-1);
            var lastMonthData = allTimeSorted
                .Where(r => r.JoinDate.Date >= startMonth)
                .GroupBy(r =>
                {
                    var daysAgo = (today - r.JoinDate.Date).Days;
                    return $"Week {(4 - (daysAgo / 7))}";
                })
                .GroupBy(g => g.Key)
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    var flat = g.SelectMany(x => x);
                    double value1 = flat.Sum(x => x.Commission);
                    runningTotal += value1;

                    return new ChartViewModel
                    {
                        Label = g.Key,
                        Value1 = (int)value1,
                        Value2 = calculateNet ? (int)runningTotal : 0,
                        DurationFilter = "LastMonth"
                    };
                }).ToList();

            result.Add(lastMonthData);
            runningTotal = 0;

            // LAST 3 MONTHS (monthly)
            var start3Months = today.AddMonths(-2); // Includes this month and two before
            var last3MonthsData = allTimeSorted
                .Where(r => r.JoinDate.Date >= start3Months)
                .GroupBy(r => r.JoinDate.ToString("MMM"))
                .OrderBy(g => DateTime.ParseExact(g.Key, "MMM", null))
                .Select(g =>
                {
                    double value1 = g.Sum(x => x.Commission);
                    runningTotal += value1;

                    return new ChartViewModel
                    {
                        Label = g.Key, // e.g., "Apr", "May"
                        Value1 = (int)value1,
                        Value2 = calculateNet ? (int)runningTotal : 0,
                        DurationFilter = "Last3Months"
                    };
                }).ToList();

            result.Add(last3MonthsData);
            runningTotal = 0;

            // LAST YEAR (quarterly)
            var startYear = today.AddYears(-1);
            var yearData = allTimeSorted.Where(r => r.JoinDate.Date >= startYear)
                .GroupBy(r =>
                {
                    int m = r.JoinDate.Month;
                    return m <= 3 ? "Q1" : m <= 6 ? "Q2" : m <= 9 ? "Q3" : "Q4";
                })
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    double value1 = g.Sum(x => x.Commission);
                    runningTotal += value1;

                    return new ChartViewModel
                    {
                        Label = g.Key,
                        Value1 = (int)value1,
                        Value2 = calculateNet ? (int)runningTotal : 0,
                        DurationFilter = "LastYear"
                    };
                }).ToList();

            result.Add(yearData);

            return result;
        }

        public static List<List<ChartViewModel>> GetAdminNewJoinsOverTime(List<UserSubscription> userSubscriptions, MobileDeviceType platformFilter)
        {
            var result = new List<List<ChartViewModel>>();
            var today = DateTime.Today;

            var filteredSubs = userSubscriptions
                .Where(u => u.MobileDeviceType == platformFilter && u.Status == StatusType.Active)
                .ToList();

            List<ChartViewModel> BuildList(DateTime start, DateTime end, string durationLabel)
            {
                var list = new List<ChartViewModel>();

                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var count = filteredSubs.Count(u => u.ActiveDate.Date == date);
                    var active = filteredSubs.Count(u => u.ActiveDate.Date <= date);

                    list.Add(new ChartViewModel
                    {
                        Label = date.ToString("dd MMM"),
                        Value1 = count,
                        Value2 = active,
                        DurationFilter = durationLabel
                    });
                }

                return list;
            }

            // TODAY
            result.Add(BuildList(today, today, "Today"));

            // LAST 7 DAYS
            result.Add(BuildList(today.AddDays(-6), today, "Last7Days"));

            // LAST 30 DAYS
            result.Add(BuildList(today.AddDays(-29), today, "Last30Days"));

            // LAST 90 DAYS
            result.Add(BuildList(today.AddDays(-89), today, "Last90Days"));

            // LAST YEAR (monthly)
            var lastYearList = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    var monthDate = today.AddMonths(-i);
                    var count = filteredSubs.Count(u => u.ActiveDate.Month == monthDate.Month && u.ActiveDate.Year == monthDate.Year);
                    var active = filteredSubs.Count(u => u.ActiveDate <= monthDate);

                    return new ChartViewModel
                    {
                        Label = monthDate.ToString("MMM yy"),
                        Value1 = count,
                        Value2 = active,
                        DurationFilter = "LastYear"
                    };
                })
                .Reverse()
                .ToList();
            result.Add(lastYearList);

            return result;
        }

        public static List<List<UserTypeChartViewModel>> GetAdminUserTypesOverTime(List<UserModel> allUsers)
        {
            var result = new List<List<UserTypeChartViewModel>>();
            var today = DateTime.Today;

            // Only users with AccountStatus == Active
            var activeUsers = allUsers
                .Where(u => u.AccountStatus == AccountStatusType.Active)
                .ToList();

            Func<IEnumerable<UserModel>, UserTypeChartViewModel> buildCounts = users => new UserTypeChartViewModel
            {
                Premium = users.Count(u => u.UserRole == UserRoleType.Client),
                Affiliates = users.Count(u => u.UserRole == UserRoleType.Affiliate),
                Doctors = users.Count(u => u.UserRole == UserRoleType.Doctor)
            };

            // TODAY
            var todayList = activeUsers
                .Where(u => u.CreateDate.Date == today)
                .DefaultIfEmpty()
                .GroupBy(_ => 0)
                .Select(g => buildCounts(g.Where(x => x != null)))
                .ToList();
            result.Add(todayList);

            // LAST 7 DAYS
            var start7 = today.AddDays(-6);
            var last7List = activeUsers
                .Where(u => u.CreateDate.Date >= start7)
                .DefaultIfEmpty()
                .GroupBy(_ => 0)
                .Select(g => buildCounts(g.Where(x => x != null)))
                .ToList();
            result.Add(last7List);

            // LAST 30 DAYS
            var start30 = today.AddDays(-29);
            var last30List = activeUsers
                .Where(u => u.CreateDate.Date >= start30)
                .DefaultIfEmpty()
                .GroupBy(_ => 0)
                .Select(g => buildCounts(g.Where(x => x != null)))
                .ToList();
            result.Add(last30List);

            // LAST 3 MONTHS
            var start90 = today.AddMonths(-3);
            var last90List = activeUsers
                .Where(u => u.CreateDate.Date >= start90)
                .DefaultIfEmpty()
                .GroupBy(_ => 0)
                .Select(g => buildCounts(g.Where(x => x != null)))
                .ToList();
            result.Add(last90List);

            // LAST YEAR
            var startYear = today.AddYears(-1);
            var lastYearList = activeUsers
                .Where(u => u.CreateDate.Date >= startYear)
                .DefaultIfEmpty()
                .GroupBy(_ => 0)
                .Select(g => buildCounts(g.Where(x => x != null)))
                .ToList();
            result.Add(lastYearList);

            return result;
        }
    }
}
