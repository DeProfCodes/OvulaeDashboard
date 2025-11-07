using OvulaeDashboard.Helpers.Enums;

namespace OvulaeDashboard.ViewModels.Components
{
    public class BreadcrumbViewModel
    {
        public string Icon { get; set; }

        public AppPageType PageType { get; set; }

        public int PageId { get; set; }

        public string PlanName { get; set; }

        public BreadcrumbButton ActionButton { get; set; } = new BreadcrumbButton();
    }

    public class BreadcrumbButton
    {
        public string ButtonText { get; set; }

        public AppPageType Page { get; set; }

        public string ButtonIconCss { get; set; }
    }
}
