using OvulaeShared.Helpers.ModuleHelpers.DayLogging;
using OvulaeShared.Models.Ovulation;

namespace OvulaeDashboard.Helpers.Test
{
    public class OvulationLogGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly DateTime _startDate = DateTime.Now.AddDays(-29); // 30 days back

        public static List<OvulationCycleLog> Generate30DaysOvulationLogs(int ovulationTrackerLogId = 1)
        {
            var logs = new List<OvulationCycleLog>();

            for (int i = 0; i < 30; i++)
            {
                var logDate = _startDate.AddDays(i);
                var phaseName = GetPhaseForDate(logDate);
                logs.Add(GenerateDailyLog(ovulationTrackerLogId, phaseName, logDate, i + 1));
            }

            return logs;
        }

        private static OvulationCycleLog GenerateDailyLog(int trackerLogId, string phaseName, DateTime logDate, int entryId)
        {
            var hasGoodDay = _random.Next(0, 10) > 3; // 60% good days
            var hasComplication = _random.Next(0, 20) == 1; // 5% chance of complication
            var isBleedingDay = phaseName == "Menstrual" && _random.Next(0, 10) > 7; // 20% chance in menstrual phase

            // Base mood rating that influences other ratings
            var baseMoodRating = _random.Next(hasGoodDay ? 6 : 3, hasGoodDay ? 10 : 7);

            // Calculate all ratings first
            var moodsRating = baseMoodRating;
            var symptomsRating = CalculateSymptomsRating(phaseName);
            var emotionsRating = CalculateEmotionsRating(baseMoodRating);
            var cravingsRating = CalculateCravingsRating(phaseName);
            var energyRating = CalculateEnergyRating(hasGoodDay, phaseName);
            var lifestyleRating = CalculateLifestyleRating(hasGoodDay);
            var breastTendernessRating = CalculateBreastTendernessRating(phaseName);
            var intercourseRating = _random.Next(1, 11); // Random rating for intercourse

            var log = new OvulationCycleLog
            {
                EntryId = entryId,
                OvulationTrackerLogId = trackerLogId,
                LogDate = logDate,
                PhaseName = phaseName,
                ExpectedOvulationDate = CalculateExpectedOvulationDate(logDate),

                // Fertility & Intercourse
                HadIntercourse = _random.Next(0, 10) > 6, // 30% chance
                IntercourseTiming = GetIntercourseTiming(phaseName),
                UsedProtection = _random.Next(0, 10) > 4, // 50% chance
                ContraceptiveMethod = GetRandomItem(OvulationDayLogItems.ContraceptionMethods),

                // Mucus & Temp
                CervicalMucusType = GetCervicalMucusType(phaseName),
                BasalBodyTempCelsius = GetBasalBodyTemp(phaseName),

                // Bleeding Info
                HadBleeding = isBleedingDay,
                BleedingColor = isBleedingDay ? GetRandomItem(CycleTrackerDayLogItems.BloodColorOptions) : "",
                FlowIntensity = isBleedingDay ? GetRandomItem(CycleTrackerDayLogItems.FlowIntensityOptions) : "",
                FlowIntensityOption = isBleedingDay ? GetFlowIntensityOption() : "",
                BleedingDurationDays = isBleedingDay ? _random.Next(1, 3) : 0,
                BleedingPresence = isBleedingDay ? "Spotting" : "None",
                PainLevel = GetPainLevel(phaseName),

                // Mood & Symptoms with ratings
                Moods = GetRandomItems(OvulationDayLogItems.Moods, 1, 3),
                MoodsRating = moodsRating,
                MoodsNotes = GetMoodNote(moodsRating),

                Symptoms = GetRandomItems(OvulationDayLogItems.Symptoms, 0, 4),
                SymptomsRating = symptomsRating,
                SymptomsNotes = GetSymptomsNote(),

                // FIXED: Use OvulationDayLogItems instead of PeriodTrackerDayLogItems
                Emotions = GetRandomItems(PeriodTrackerDayLogItems.Emotions, 1, 2),
                EmotionsRating = emotionsRating,
                EmotionsNotes = GetEmotionsNote(),

                // FIXED: Use OvulationDayLogItems instead of PeriodTrackerDayLogItems
                Cravings = GetRandomItems(PeriodTrackerDayLogItems.Cravings, 0, 2),
                CravingsRating = cravingsRating,
                CravingsNotes = GetCravingsNote(),

                EnergyLevel = GetEnergyLevel(hasGoodDay, phaseName),
                EnergyRating = energyRating,
                EnergyNotes = GetEnergyNote(),

                // Lifestyle Factors
                LifestyleFactors = GetRandomItems(OvulationDayLogItems.LifestyleActivities, 1, 3),
                LifestyleRating = lifestyleRating,
                LifestyleNotes = GetLifestyleNote(),

                // User Notes
                Notes = GetDailyReflection(phaseName, baseMoodRating, isBleedingDay),

                // Diagnostic Observations
                LHTestResult = GetLHTestResult(phaseName),
                HadMidCycleSpotting = phaseName == "Ovulation" && _random.Next(0, 10) > 8, // 10% chance during ovulation
                SpottingDetails = GetSpottingDetails(),
                CervixPosition = GetCervixPosition(phaseName),
                CervixFeel = GetCervixFeel(phaseName),

                // Breast Tenderness
                BreastTendernessNotes = GetBreastTendernessNote(phaseName),
                BreastTendernessRating = breastTendernessRating,

                // Bowel Movements
                BowelMovementsRegularity = GetRandomItem(CycleTrackerDayLogItems.PeriodBowelMovementIrregularityOptions),
                HadBowelMovements = _random.Next(0, 10) > 1, // 80% chance
                BowelMovementsFrequency = GetRandomItem(CycleTrackerDayLogItems.PeriodBowelMovementFrequencyOptions),

                // Flags
                IsFertileWindow = phaseName == "Ovulation" || phaseName == "Follicular",
                MarkedAsPeakDay = phaseName == "Ovulation" && _random.Next(0, 10) > 7, // 20% chance

                // Doctor's Notes
                DoctorsNotes = hasComplication ? "Monitor fertility signs" : "",
                DoctorResponseTime = hasComplication ? DateTime.Now.AddHours(-_random.Next(1, 24)) : (DateTime?)null
            };

            // Debug: Check if ratings are being set
            Console.WriteLine($"Generated log - MoodsRating: {log.MoodsRating}, SymptomsRating: {log.SymptomsRating}, EmotionsRating: {log.EmotionsRating}, CravingsRating: {log.CravingsRating}");

            return log;
        }

        // ========== PHASE AND CYCLE CALCULATION METHODS ==========

        private static string GetPhaseForDate(DateTime date)
        {
            var dayOfCycle = (date.Day % 28) + 1; // Simulate 28-day cycle

            return dayOfCycle switch
            {
                <= 5 => "Menstrual",
                <= 13 => "Follicular",
                <= 15 => "Ovulation",
                _ => "Luteal"
            };
        }

        private static DateTime? CalculateExpectedOvulationDate(DateTime logDate)
        {
            var dayOfCycle = (logDate.Day % 28) + 1;
            if (dayOfCycle == 14) // Typical ovulation day
            {
                return logDate;
            }
            return null;
        }

        private static string GetIntercourseTiming(string phaseName)
        {
            return phaseName switch
            {
                "Follicular" => "Before Ovulation",
                "Ovulation" => "Peak Day",
                "Luteal" => "After Ovulation",
                _ => "Not Tracked"
            };
        }

        private static string GetCervicalMucusType(string phaseName)
        {
            return phaseName switch
            {
                "Menstrual" => "Blood",
                "Follicular" => _random.Next(0, 10) > 5 ? "Sticky" : "Creamy",
                "Ovulation" => "Egg-white",
                "Luteal" => _random.Next(0, 10) > 5 ? "Creamy" : "Sticky",
                _ => "Not Tracked"
            };
        }

        private static double? GetBasalBodyTemp(string phaseName)
        {
            // BBT typically rises after ovulation
            return phaseName switch
            {
                "Follicular" => 36.2 + _random.NextDouble() * 0.3,
                "Ovulation" => 36.4 + _random.NextDouble() * 0.2,
                "Luteal" => 36.6 + _random.NextDouble() * 0.2,
                _ => 36.3 + _random.NextDouble() * 0.3
            };
        }

        // ========== RATING CALCULATION METHODS ==========

        private static int CalculateSymptomsRating(string phaseName)
        {
            return phaseName switch
            {
                "Ovulation" => _random.Next(5, 9), // Mittelschmerz possible
                "Luteal" => _random.Next(4, 8), // PMS symptoms
                "Menstrual" => _random.Next(3, 7),
                _ => _random.Next(2, 6) // Follicular - usually lower symptoms
            };
        }

        private static int CalculateEmotionsRating(int baseMoodRating)
        {
            return Math.Clamp(baseMoodRating + _random.Next(-1, 2), 1, 10);
        }

        private static int CalculateCravingsRating(string phaseName)
        {
            // Cravings often higher in luteal phase
            return phaseName switch
            {
                "Luteal" => _random.Next(6, 10),
                "Ovulation" => _random.Next(4, 8),
                _ => _random.Next(3, 7)
            };
        }

        private static int CalculateEnergyRating(bool hasGoodDay, string phaseName)
        {
            var baseRating = phaseName switch
            {
                "Ovulation" => _random.Next(6, 10), // Often higher energy
                "Follicular" => _random.Next(5, 9),
                "Luteal" => _random.Next(3, 7),
                _ => _random.Next(4, 8) // Menstrual
            };
            return Math.Clamp(hasGoodDay ? baseRating + 1 : baseRating - 1, 1, 10);
        }

        private static int CalculateLifestyleRating(bool hasGoodDay)
        {
            return hasGoodDay ? _random.Next(7, 11) : _random.Next(4, 8);
        }

        private static int CalculateBreastTendernessRating(string phaseName)
        {
            // Breast tenderness often highest in luteal phase
            return phaseName switch
            {
                "Luteal" => _random.Next(6, 10),
                "Ovulation" => _random.Next(4, 8),
                _ => _random.Next(2, 6)
            };
        }

        // ========== NOTE GENERATION METHODS ==========

        private static string GetMoodNote(int rating)
        {
            return rating switch
            {
                >= 9 => "Excellent mood today, feeling very positive",
                >= 7 => "Good mood, feeling balanced and content",
                >= 5 => "Neutral mood, typical hormonal fluctuations",
                >= 3 => "Feeling a bit sensitive and emotional",
                _ => "Difficult day emotionally, hormones affecting mood"
            };
        }

        private static string GetSymptomsNote()
        {
            var notes = new[]
            {
                "Typical cycle symptoms today",
                "Managing symptoms with self-care",
                "Symptoms are noticeable but manageable",
                "Some discomfort but able to continue daily activities",
                "Using heat and rest for symptom relief"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetEmotionsNote()
        {
            var notes = new[]
            {
                "Emotions feel balanced today",
                "Feeling more sensitive than usual",
                "Managing emotional fluctuations well",
                "Taking time for self-care and relaxation",
                "Noticing hormonal effects on emotions"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetCravingsNote()
        {
            var notes = new[]
            {
                "Craving sweets and comfort foods",
                "Wanted salty snacks throughout the day",
                "No strong cravings today",
                "Managing cravings with healthy alternatives",
                "Gave in to some cravings but balanced with nutrition"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetEnergyNote()
        {
            var notes = new[]
            {
                "Good energy for activities today",
                "Feeling energetic and productive",
                "Energy levels fluctuating",
                "Needed more rest today",
                "Paced myself throughout the day"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetLifestyleNote()
        {
            var notes = new[]
            {
                "Maintaining healthy habits despite symptoms",
                "Good balance of activity and rest",
                "Staying hydrated and eating well",
                "Managing stress with relaxation techniques",
                "Listening to body's needs today"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetIntercourseNote()
        {
            var notes = new[]
            {
                "Normal sexual activity today",
                "Libido affected by cycle phase",
                "Using protection as planned",
                "No intercourse today",
                "Comfortable with intimacy level"
            };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetBreastTendernessNote(string phaseName)
        {
            return phaseName switch
            {
                "Luteal" => "Normal breast tenderness for this phase",
                "Ovulation" => "Some breast sensitivity as expected",
                _ => "Minimal breast tenderness today"
            };
        }

        // ========== HELPER METHODS ==========

        private static string GetPainLevel(string phaseName)
        {
            return phaseName switch
            {
                "Ovulation" => _random.Next(0, 10) > 7 ? "😣 Moderate" : "😐 Mild", // Mittelschmerz
                "Menstrual" => _random.Next(0, 10) > 6 ? "😣 Moderate" : "😐 Mild",
                "Luteal" => "😐 Mild",
                _ => "😌 None"
            };
        }

        private static string GetEnergyLevel(bool hasGoodDay, string phaseName)
        {
            if (hasGoodDay) return "High";

            return phaseName switch
            {
                "Ovulation" => "High",
                "Follicular" => "Normal",
                "Luteal" => _random.Next(0, 10) > 6 ? "Low" : "Normal",
                _ => "Normal"
            };
        }

        private static string GetLHTestResult(string phaseName)
        {
            return phaseName switch
            {
                "Ovulation" => _random.Next(0, 10) > 3 ? "Positive" : "Negative",
                _ => "NotTaken"
            };
        }

        private static string GetSpottingDetails()
        {
            var details = new[]
            {
                "Light pink spotting",
                "Brown discharge",
                "Minimal spotting noticed",
                "Mid-cycle bleeding"
            };
            return details[_random.Next(details.Length)];
        }

        private static string GetCervixPosition(string phaseName)
        {
            return phaseName switch
            {
                "Ovulation" => "High",
                "Menstrual" => "Low",
                _ => _random.Next(0, 10) > 5 ? "High" : "Low"
            };
        }

        private static string GetCervixFeel(string phaseName)
        {
            return phaseName switch
            {
                "Ovulation" => "Soft",
                "Menstrual" => "Firm",
                _ => _random.Next(0, 10) > 5 ? "Soft" : "Firm"
            };
        }

        private static string GetFlowIntensityOption()
        {
            var options = new[] { "💧 Light", "🩸 Spotting", "❌ None" };
            return options[_random.Next(options.Length)];
        }

        private static List<string> GetRandomItems(List<string> source, int min, int max)
        {
            if (!source.Any()) return new List<string>();
            var count = _random.Next(min, Math.Min(max + 1, source.Count));
            return source.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        private static string GetRandomItem(List<string> source)
        {
            return source.Any() ? source[_random.Next(source.Count)] : "";
        }

        private static string GetDailyReflection(string phaseName, int moodRating, bool isBleedingDay)
        {
            var goodReflections = new[]
            {
                $"Feeling good during {phaseName} phase, body feels balanced",
                "Managing cycle symptoms well today",
                "In tune with my body's fertility signs",
                "Appreciating the natural cycle of my body",
                "Good energy and mood despite hormonal changes"
            };

            var normalReflections = new[]
            {
                $"Typical {phaseName} phase symptoms, managing okay",
                "Noticing normal hormonal fluctuations",
                "Body going through expected cycle changes",
                "Tracking fertility signs as expected",
                "Learning more about my ovulation patterns"
            };

            var lowReflections = new[]
            {
                "Cycle symptoms making today challenging",
                "Feeling the effects of hormonal changes",
                "Taking it easy due to menstrual discomfort",
                "Reminding myself this phase is temporary",
                "Using self-care to manage tougher cycle days"
            };

            var bleedingReflections = new[]
            {
                "Managing menstrual flow comfortably",
                "Using period products that work well for me",
                "Resting during menstrual days",
                "Staying hydrated and comfortable during period",
                "Listening to my body's needs during menstruation"
            };

            if (isBleedingDay) return bleedingReflections[_random.Next(bleedingReflections.Length)];

            return moodRating switch
            {
                >= 8 => goodReflections[_random.Next(goodReflections.Length)],
                >= 5 => normalReflections[_random.Next(normalReflections.Length)],
                _ => lowReflections[_random.Next(lowReflections.Length)]
            };
        }
    }
}
