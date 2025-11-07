using OvulaeShared.Enums.App;
using OvulaeShared.Models.Menopause;
using OvulaeShared.Models.Ovulation;
using OvulaeShared.Models.PeriodTracker;
using OvulaeShared.Models.Pregnancy;

namespace OvulaeDashboard.ViewModels.Doctor
{
    public class UpdateDoctorsNotesDTO
    {
        public ModuleType ModuleType { get; set; }

        public string PatientUserId { get; set; }

        public PregnancyLogEntryItem PregnancyLog { get; set; }

        public int ModuleLogId { get; set; }

        public int EntryId { get; set; }

        public PeriodLogEntry PeriodTrackerLog { get; set; }

        public MenopauseLogEntry MenopauseLog { get; set; }

        public OvulationTrackerLog OvulationLog { get; set; }

        public string Notes { get; set; }
    }
}
