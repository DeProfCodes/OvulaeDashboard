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

    public class DetailedLogEntry
    {
        public int EntryId { get; set; }
        public DateTime LogDate { get; set; }
        public string TrackerType { get; set; }
        public double MoodRating { get; set; }
        public double EnergyRating { get; set; }
        public double SleepRating { get; set; }
        public double SymptomsRating { get; set; }
        public double StressRating { get; set; }
        public double AppetiteRating { get; set; }
        public string Notes { get; set; }
        public string PhaseName { get; set; }
        public bool HadBleeding { get; set; }
        public int Week { get; set; }
        public List<string> Symptoms { get; set; }
        public List<string> Moods { get; set; }
    }

    public class RenderPartialRequest
    {
        public string PartialViewName { get; set; }
        public object ViewModel { get; set; }
        public string ViewModelType { get; set; }
        public bool IsCurrent { get; set; }
    }

}
