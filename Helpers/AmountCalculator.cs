using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using OvulaeShared.Models.Affiliate;
using OvulaeShared.Models.Transactions;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.Helpers
{
    public class AmountCalculator
    {
        public static (double currentPending, double nextPending) GetAffiliatePendingAmountFromCurrentMonthAffiliates(List<UserAffiliateReferalDetails> userAffiliates,
                                                                                                                  Transaction lastAffiliatePayout)
        {
            var today = DateTime.Today;
            var thisMonth20th = new DateTime(today.Year, today.Month, 20);

            double currentPending = 0.0;
            double nextPending = 0.0;

            foreach (var affiliate in userAffiliates)
            {
                // Only consider affiliates that joined after the last payout
                if (affiliate.JoinDate <= lastAffiliatePayout.LastUpdateDate)
                    continue;

                var commission = PricingHelperFunctions.GetAmountFromSubscriptionType(affiliate.SubscriptionType);

                if (today < thisMonth20th)
                {
                    currentPending += commission;
                }
                else
                {
                    if (lastAffiliatePayout.LastUpdateDate >= thisMonth20th)
                    {
                        nextPending += commission;
                    }
                    else
                    {
                        if (affiliate.JoinDate < thisMonth20th)
                            currentPending += commission;
                        
                        else
                            nextPending += commission;
                    }
                }
            }

            return (currentPending, nextPending);
        }
    }
}
