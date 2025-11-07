using OvulaeDashboard.ViewModels.Dashboard;
using OvulaeDashboard.ViewModels.Shared;


namespace OvulaeDashboard.ViewModels.Admin
{
    public class AdminDashViewModel
    {
        public int PremiumUsers { get; set; }

        public int Doctors { get; set; }

        public int Affiliates { get; set; }

        public string TotalSubscriptionsAmount { get; set; }

        public string NetRevenue { get; set; }

        public int TotalMonthlyPremiumsOverall { get; set; }

        public int TotalYearlyPremiumsOverall { get; set; }

        public int TotalActiveSubscriptions {get; set; }

        public int TotalInActiveSubscriptions { get; set; }

        public int ThisMonthSubscribers { get; set; }

        public int CurrentSubscriptionLevel { get; set; }

        public int NextSubscriptionLevel { get; set; }

        public int OverallSubscribers { get; set; }

        public List<List<ChartViewModel>> NewJoinsAndroid { get; set; }

        public List<List<ChartViewModel>> NewJoinsIOS { get; set; }

        public List<List<ChartViewModel>> NewJoinsAll { get; set; }

        public List<List<UserTypeChartViewModel>> UserTypes { get; set; }

        public int IOSUsers { get; set; }

        public int AndroidUsers { get; set; }

        public string TopCountryCode { get; set; }

        public int TopCountryUsers { get; set; }

        public string TopAffiliateFullname { get; set; }

        public int TopAffiliateReferals { get; set; }

        public string TopDoctorName { get; set; }

        public int TopDoctorReferals { get; set; }

        public int TotalClicks { get; set; }

        public int TotalUsers { get; set; }

        public string TotalPaidToAffiliates { get; set; }

        public string TotalDueToAffiliates { get; set; }

        public string TotalPendingPayouts { get; set; }

        public double AveragePayoutToAffiliateThisMonth { get; set; }

        public int TodayNewUsers { get; set; }

        public int TodayTotalClicks { get; set; }

        public int TodayNewSubscription { get; set; }

        public int TotalCountries { get; set; }
    }
}
