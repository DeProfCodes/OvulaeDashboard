using OvulaeShared.Enums.Status;
using OvulaeShared.Enums.Transactions;
using OvulaeShared.Enums.User;
using OvulaeShared.Helpers.CommonFunctions;
using OvulaeShared.Models.Affiliate;
using OvulaeShared.Models.Transactions;
using OvulaeShared.Models.User;
using OvulaeShared.ViewModel.Affiliates;

namespace OvulaeDashboard.Helpers
{
    public static class AffiliateMockDataGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly string[] _countries = { "US", "UK", "CA", "AU", "DE", "FR", "BR", "ZA", "NG", "KE" };
        private static readonly string[] _firstNames = { "Emma", "Liam", "Olivia", "Noah", "Ava", "Sophia", "Jackson", "Mia", "Lucas", "Isabella" };
        private static readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };

        public static AffiliateDashboardViewModel GenerateSuccessfulAffiliateData(string userId)
        {
            var data = new AffiliateDashboardViewModel
            {
                Clicks = GenerateImpressiveClicks(userId),
                Transactions = GenerateSuccessfulTransactions(userId),
                Wallet = GenerateProfitableWallet(userId),
                ReferalsDetails = GenerateHighConvertingReferrals(),
                User = GenerateSuccessfulUser(userId)
            };

            return data;
        }

        private static AffiliateClicks GenerateImpressiveClicks(string userId)
        {
            return new AffiliateClicks
            {
                Id = 1,
                AffiliateUserId = userId,
                AndroidClicks = _random.Next(2500, 5000), // 2.5K-5K clicks
                AndroidReferalLink = Guid.NewGuid().ToString("N"),
                IOSClicks = _random.Next(1800, 3500),     // 1.8K-3.5K clicks
                IOSReferalLink = Guid.NewGuid().ToString("N"),
                CreateDate = DateTime.Now.AddMonths(-6),
                LastUpdateDate = DateTime.Now
            };
        }

        private static List<Transaction> GenerateSuccessfulTransactions(string userId)
        {
            var transactions = new List<Transaction>();
            var baseDate = DateTime.Now.AddMonths(-6);

            // Monthly payouts (showing consistent earnings)
            for (int i = 0; i < 6; i++)
            {
                transactions.Add(new Transaction
                {
                    Id = i + 1,
                    UserId = userId,
                    Type = TransactionType.Deposit,
                    Amount = 28.5, // $1.2K-$3.5K monthly
                    CreateDate = baseDate.AddMonths(i),
                    LastUpdateDate = baseDate.AddMonths(i).AddDays(2),
                    Comment = $"Affiliate commission payout - Month {i + 1}",
                    Status = StatusType.Paid,
                    Reference = $"PAY_{DateTime.Now:yyyyMM}_{i + 1}"
                });
            }

            return transactions;
        }

        private static AffiliateWallet GenerateProfitableWallet(string userId)
        {
            return new AffiliateWallet
            {
                Id = 1,
                AffiliateUserId = userId,
                TotalRevenue = 8250.80,    // $15K-$25K total revenue
                TotalPaidOut = 2305.55,     // $8K-$15K paid out
                Status = StatusType.Active,
                CreateDate = DateTime.Now.AddMonths(-6),
                LastUpdateDate = DateTime.Now
            };
        }

        private static List<UserAffiliateReferalDetails> GenerateHighConvertingReferrals()
        {
            var referrals = new List<UserAffiliateReferalDetails>();
            var baseDate = DateTime.Now.AddMonths(-6);
            var referralCount = _random.Next(620, 1250);

            // Define weighted country code distribution
            var countryCodeWeights = new Dictionary<string, int>
            {
                { "+27", 120 },  // Highest priority
                { "+1", 80 },    // Second priority
                { "+44", 50 }    // Third priority
            };

            // Get all country codes from source
            var allCountryCodes = CountryCodeFunctions.COUNTRY_CODE_FLAG
                .Select(x => x.Split(" ")[1])
                .Distinct()
                .ToList();

            // Add remaining country codes with minimal weight
            foreach (var code in allCountryCodes)
            {
                if (!countryCodeWeights.ContainsKey(code))
                {
                    countryCodeWeights[code] = 1;
                }
            }

            // Build weighted country code list
            var weightedCountryCodes = new List<string>();
            foreach (var kvp in countryCodeWeights)
            {
                weightedCountryCodes.AddRange(Enumerable.Repeat(kvp.Key, kvp.Value));
            }

            // Shuffle to avoid strict grouping while preserving bias
            weightedCountryCodes = weightedCountryCodes.OrderBy(x => _random.Next()).ToList();

            // Trim or expand to match referralCount
            while (weightedCountryCodes.Count < referralCount)
            {
                weightedCountryCodes.Add(weightedCountryCodes[_random.Next(weightedCountryCodes.Count)]);
            }
            if (weightedCountryCodes.Count > referralCount)
            {
                weightedCountryCodes = weightedCountryCodes.Take(referralCount).ToList();
            }

            // Generate referral entries
            foreach (var countryCode in weightedCountryCodes)
            {
                var joinDate = baseDate.AddDays(_random.Next(0, 180));
                var deviceType = _random.Next(0, 2) == 0 ? MobileDeviceType.Android : MobileDeviceType.IOS;
                var subscriptionType = GetRandomSubscriptionType();

                referrals.Add(new UserAffiliateReferalDetails
                {
                    JoinDate = joinDate,
                    ReferalId = $"REF_{Guid.NewGuid().ToString("N").Substring(0, 8)}",
                    DeviceType = deviceType,
                    SubscriptionType = subscriptionType,
                    CountryCode = countryCode,
                    Commission = CalculateCommission(subscriptionType),
                    ReferalStatus = StatusType.Active
                });
            }

            return referrals;
        }


        private static string GetRandomCountryCode()
        {
            var countriesCodes = CountryCodeFunctions.COUNTRY_CODE_FLAG.Select(x => x.Split(" ")[1]).ToList();

            // Define your priority codes
            var priorityCodes = new[] { "+27", "+1", "+44" };

            // Create a weighted list
            var weightedCodes = new List<string>();

            foreach (var code in countriesCodes)
            {
                int weight = code switch
                {
                    "+27" => 10,  // Highest priority
                    "+1" => 6,   // Second priority
                    "+44" => 3,   // Third priority
                    _ => 1    // All others
                };
                weightedCodes.AddRange(Enumerable.Repeat(code, weight));
            }

            // Random selection from weighted list
            return weightedCodes[_random.Next(weightedCodes.Count)];

        }
        private static SubscriptionType GetRandomSubscriptionType()
        {
            var values = Enum.GetValues(typeof(SubscriptionType))
                             .Cast<SubscriptionType>()
                             .Where(x => x != SubscriptionType.None)
                             .ToList();

            // Create a weighted list with more entries for Monthly
            var weightedValues = new List<SubscriptionType>();

            foreach (var value in values)
            {
                int weight = value == SubscriptionType.PremiumMonthly ? 5 : 1;
                weightedValues.AddRange(Enumerable.Repeat(value, weight));
            }

            return weightedValues[_random.Next(weightedValues.Count)];
        }


        private static double CalculateCommission(SubscriptionType subscriptionType)
        {
            return subscriptionType switch
            {
                SubscriptionType.PremiumYearly => 1440,
                SubscriptionType.PremiumMonthly => 28.5,
                _ => _random.Next(5, 12)
            };
        }

        private static UserModel GenerateSuccessfulUser(string userId)
        {
            return new UserModel
            {
                UserId = userId,
                Firstname = "Luther",
                Lastname = "Smith",
                Username = "topaffiliate",
                Email = "successful.affiliate@example.com",
                CountryCode = "US",
                PhoneNumber = "+1-555-0123",
                MobileDevice = "iPhone 15 Pro",
                UserRole = UserRoleType.Affiliate,
                AccountStatus = AccountStatusType.Active,
                CreateDate = DateTime.Now.AddYears(-1)
            };
        }
    }
}
