using OvulaeDashboard.Helpers.Enums;

using OvulaeShared.Enums.User;

namespace OvulaeDashboard.ViewModels.Layouts
{
    public class LeftNavigationViewModel
    {
        public UserRoleType UserRole { get; set; }

        public int PendingUsersCount { get; set; }

        public int ActivePremiums { get; set; }

        public int FreeTrialCount { get; set; }

        public int PendingPremiums { get; set; }
    }
}
