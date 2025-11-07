using OvulaeShared.Models.Menopause;
using OvulaeShared.Models.Ovulation;
using OvulaeShared.Models.PeriodTracker;
using OvulaeShared.Models.Pregnancy;

namespace OvulaeDashboard.ViewModels.Doctor
{
    public class PregnancyLogsViewModel
    {
        public List<PregnancyLogEntryItem> Logs { get; set; }

        public string PatientUserId { get; set; }
    }

    public class MenopauseLogsViewModel
    {
        public List<MenopauseLogEntry> Logs { get; set; }

        public string PatientUserId { get; set; }
    }

    public class PeriodLogsViewModel
    {
        public List<PeriodLogEntry> Logs { get; set; }

        public string PatientUserId { get; set; }
    }

    public class OvulationLogsViewModel
    {
        public List<OvulationCycleLog> Logs { get; set; }

        public string PatientUserId { get; set; }
    }
}
