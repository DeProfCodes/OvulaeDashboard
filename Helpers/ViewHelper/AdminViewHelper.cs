using OvulaeDashboard.ViewModels.Admin;
using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;
using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using OvulaeShared.Helpers.Constants;
using OvulaeShared.ViewModel.Admin;

namespace OvulaeDashboard.Helpers.ViewHelper
{
    public class AdminViewHelper
    {
        public static AdminDashViewModel GetAdminDashboardData(AdminDashboardViewModel apiData)
        {
            try
            {
                var model = new AdminDashViewModel();

                //Header Data
                model.PremiumUsers = apiData.AllSubscriptions.Count(x => x.Status == StatusType.Active);
                model.Doctors = apiData.AllUsers.Count(x => x.UserRole == UserRoleType.Doctor && x.AccountStatus == AccountStatusType.Active);
                model.Affiliates = apiData.AllUsers.Count(x => x.UserRole == UserRoleType.Affiliate && x.AccountStatus == AccountStatusType.Active);

                var monthlyCount = apiData.AllSubscriptions.Count(x => x.SubscriptionType == SubscriptionType.PremiumMonthly && x.Status == StatusType.Active);
                var yearlyCount = apiData.AllSubscriptions.Count(x => x.SubscriptionType == SubscriptionType.PremiumYearly && x.Status == StatusType.Active);

                var totalEarned = (monthlyCount * OvulaeConstants.MONTHLY_PREMIUM_SUBSCRIPTION) + (yearlyCount * OvulaeConstants.YEARLY_PREMIUM_SUBSCRIPTION);

                model.TotalSubscriptionsAmount = ConversionsHelpers.GetAmountInCurrency(totalEarned, "R", 2);
                model.TotalYearlyPremiumsOverall = yearlyCount;
                model.TotalMonthlyPremiumsOverall = monthlyCount;

                var thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                //Left Column Details
                model.ThisMonthSubscribers = apiData.AllSubscriptions.Count(s => s.ActiveDate >= thisMonth && s.Status == StatusType.Active);
                model.OverallSubscribers = apiData.AllSubscriptions.Count(x => x.Status == StatusType.Active);
                model.CurrentSubscriptionLevel = OvulaeLevelsHelper.GetSubscriptionLevel(model.OverallSubscribers);
                model.NextSubscriptionLevel = OvulaeLevelsHelper.GetSubscriptionNextLevel(model.OverallSubscribers);

                //Right Column Chart Data
                model.NewJoinsAndroid = ChartsDataHelper.GetAdminNewJoinsOverTime(apiData.AllSubscriptions, MobileDeviceType.Android);
                model.NewJoinsIOS = ChartsDataHelper.GetAdminNewJoinsOverTime(apiData.AllSubscriptions, MobileDeviceType.IOS);
                model.NewJoinsAll = model.NewJoinsAndroid.Zip(model.NewJoinsIOS, (androidList, iosList) => androidList.Concat(iosList).ToList()).ToList();
                model.UserTypes = ChartsDataHelper.GetAdminUserTypesOverTime(apiData.AllUsers);

                model.IOSUsers = apiData.AllSubscriptions.Count(s => s.MobileDeviceType == MobileDeviceType.IOS && s.Status == StatusType.Active);
                model.AndroidUsers = apiData.AllSubscriptions.Count(s => s.MobileDeviceType == MobileDeviceType.Android && s.Status == StatusType.Active);

                var topCountryGroup = apiData.AllUsers.Where(x => x.AccountStatus == AccountStatusType.Active).GroupBy(r => r.CountryCode)
                                                      .OrderByDescending(g => g.Count()).FirstOrDefault();

                model.TopCountryCode = topCountryGroup?.Key ?? "Unknown";
                model.TopCountryUsers = topCountryGroup?.Count() ?? 0;

                var topAffiliate = apiData.AllAffiliatesData.Where(a => a.User != null && a.User.UserRole == UserRoleType.Affiliate)
                                                            .OrderByDescending(a => a.ReferalsDetails?.Count ?? 0).FirstOrDefault();

                if (topAffiliate != null)
                {
                    model.TopAffiliateFullname = $"{topAffiliate.User.Firstname} {topAffiliate.User.Lastname}";
                    model.TopAffiliateReferals = topAffiliate.ReferalsDetails?.Count ?? 0;
                }

                var topDoctor = apiData.AllAffiliatesData.Where(a => a.User != null && a.User.UserRole == UserRoleType.Doctor)
                                                                        .OrderByDescending(a => a.ReferalsDetails?.Count ?? 0).FirstOrDefault();

                if (topDoctor != null)
                {
                    model.TopDoctorName = $"Dr. {topDoctor.User.Firstname[0]}. {topDoctor.User.Lastname}";
                    model.TopDoctorReferals = topDoctor.ReferalsDetails?.Count ?? 0;
                }

                model.TotalClicks = apiData.AllClicks.Sum(c => c.AndroidClicks + c.IOSClicks);
                model.TotalUsers = apiData.AllUsers.Count(u => u.AccountStatus == AccountStatusType.Active);

                var totalPaidOut = apiData.AllWallets.Sum(x => x.TotalPaidOut);
                model.TotalPaidToAffiliates = ConversionsHelpers.GetAmountInCurrency(totalPaidOut, "R", 2);
                model.TotalPendingPayouts = ConversionsHelpers.GetAmountInCurrency((int)apiData.AllWallets.Sum(x => (x.TotalRevenue - x.TotalPaidOut)), "R", 2);

                model.NetRevenue = ConversionsHelpers.GetAmountInCurrency(totalEarned - totalPaidOut, "R", 2);

                model.TodayNewUsers = apiData.AllUsers.Count(u => u.CreateDate >= DateTime.Today);
                model.TodayNewSubscription = apiData.AllSubscriptions.Count(s => s.ActiveDate >= DateTime.Today);
                model.TodayTotalClicks = apiData.AllClicks.Count(c => c.CreateDate >= DateTime.Today);
                model.TotalCountries = apiData.AllUsers.Select(x => x.CountryCode).Distinct().Count();

                DefaultValueHelper.SetDefaults(model);

                return model;
            }
            catch
            {
                return new();
            }
        }
    }
}
