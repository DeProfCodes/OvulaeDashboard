namespace OvulaeDashboard.ViewModels.Doctor
{
    // Helper classes needed for the graph functionality
    public class BaseLogEntry
    {
        public DateTime LogDate { get; set; }
        public string TrackerType { get; set; }

        // Only include properties that exist in most models
        public double? MoodsRating { get; set; }
        public double? SymptomsRating { get; set; }

        // We'll derive other ratings if possible
        public double DerivedEnergyRating { get; set; }
        public double DerivedSleepRating { get; set; }
        public double DerivedStressRating { get; set; }
        public double DerivedAppetiteRating { get; set; }

        public string Notes { get; set; }
    }


    public class PatientTrendsViewModel
    {
        public string PatientUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Period { get; set; }
        public string TrackerType { get; set; }
        public List<LogDataPoint> DataPoints { get; set; }
        public TrendsSummary Summary { get; set; }
    }

    public class LogDataPoint
    {
        public string DateLabel { get; set; }
        public int LogCount { get; set; }
        public double MoodRating { get; set; }
        public double EnergyRating { get; set; }
        public double SleepRating { get; set; }
        public double SymptomsRating { get; set; }
        public double StressRating { get; set; }
        public double AppetiteRating { get; set; }
        public string TrackerType { get; set; }
        public string Notes { get; set; }
    }

    public class TrendsSummary
    {
        public double AvgMood { get; set; }
        public double AvgEnergy { get; set; }
        public double AvgSleep { get; set; }
        public double AvgSymptoms { get; set; }
        public double AvgStress { get; set; }
        public double AvgAppetite { get; set; }
        public int TotalLogs { get; set; }
        public Dictionary<string, int> TrackerDistribution { get; set; }
        public string MostCommonSymptom { get; set; }
        public string PeakMoodDay { get; set; }
    }
}
