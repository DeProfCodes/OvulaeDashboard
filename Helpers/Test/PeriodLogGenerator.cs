using OvulaeShared.Helpers.ModuleHelpers.DayLogging;
using OvulaeShared.Models.PeriodTracker;

namespace OvulaeDashboard.Helpers.Test
{
    public class PeriodLogGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly DateTime _startDate = DateTime.Now.AddDays(-29); // 30 days back

        public static List<PeriodLogEntry> Generate30DaysPeriodLogs(int periodTrackerLogId = 1)
        {
            var logs = new List<PeriodLogEntry>();

            for (int i = 0; i < 30; i++)
            {
                var logDate = _startDate.AddDays(i);
                var phaseName = GetPhaseForDate(logDate); // Determine phase based on cycle timing
                logs.Add(GenerateDailyLog(periodTrackerLogId, phaseName, logDate, i + 1));
            }

            return logs;
        }

        private static PeriodLogEntry GenerateDailyLog(int trackerLogId, string phaseName, DateTime logDate, int entryId)
        {
            var hasGoodDay = _random.Next(0, 10) > 3; // 60% good days
            var isBleedingDay = IsBleedingDay(logDate, phaseName); // Determine if bleeding based on cycle
            var hasComplication = _random.Next(0, 20) == 1; // 5% chance of complication

            // Base mood rating that influences other ratings
            var baseMoodRating = _random.Next(hasGoodDay ? 6 : 3, hasGoodDay ? 10 : 7);

            return new PeriodLogEntry
            {
                EntryId = entryId,
                PeriodTrackerLogId = trackerLogId,
                LogDate = logDate,
                PhaseName = phaseName,

                // Period Start
                DidPeriodStartToday = isBleedingDay && _random.Next(0, 10) > 7, // 20% chance period starts today if bleeding day
                PeriodStartDate = null, // Optional backdate
                PeriodStartOption = GetPeriodStartOption(isBleedingDay),
                PeriodTimingOption = GetPeriodTimingOption(),

                // Bleeding Details
                HadBleeding = isBleedingDay,
                BleedingColor = isBleedingDay ? GetRandomItem(CycleTrackerDayLogItems.BloodColorOptions) : "",
                FlowIntensity = isBleedingDay ? GetRandomItem(CycleTrackerDayLogItems.FlowIntensityOptions) : "",
                FlowIntensityOption = isBleedingDay ? GetFlowIntensityOption() : "",
                DurationDays = isBleedingDay ? _random.Next(1, 8) : 0,
                BleedingPresence = isBleedingDay ? "Present" : "None",

                // Pain
                PainLevel = GetPainLevel(phaseName, isBleedingDay),

                // Medications
                UsedMedication = _random.Next(0, 10) > 6, // 30% chance
                Medications = GetRandomItems(PeriodTrackerDayLogItems.CommonMedications, 0, 2),
                MedicationMethodNotes = GetMedicationNotes(),

                // Daily Health & Symptoms
                Moods = GetRandomItems(PeriodTrackerDayLogItems.Moods, 1, 3),
                MoodsRating = baseMoodRating,
                MoodsNotes = GetMoodNote(baseMoodRating),

                Symptoms = GetRandomItems(PeriodTrackerDayLogItems.Symptoms, 0, 4),
                SymptomsRating = CalculateSymptomsRating(phaseName, isBleedingDay),
                SymptomsNotes = GetSymptomsNote(),

                Emotions = GetRandomItems(PeriodTrackerDayLogItems.Emotions, 1, 2),
                EmotionsRating = CalculateEmotionsRating(baseMoodRating),
                EmotionsNotes = GetEmotionsNote(),

                Cravings = GetRandomItems(PeriodTrackerDayLogItems.Cravings, 0, 2),
                CravingsRating = CalculateCravingsRating(phaseName),
                CravingsNotes = GetCravingsNote(),

                // Lifestyle Factors
                LifestyleFactors = GetRandomItems(PeriodTrackerDayLogItems.LifestyleFactors, 1, 3),
                LifestyleRating = CalculateLifestyleRating(hasGoodDay),
                LifestyleNotes = GetLifestyleNote(),

                // Intercourse
                IntercourseInfo = GetRandomItems(PeriodTrackerDayLogItems.IntercourseOptions, 0, 1),
                IntercourseRating = CalculateIntercourseRating(),
                IntercourseNotes = GetIntercourseNote(),

                ContraceptionMethodUsedToday = GetRandomItem(PeriodTrackerDayLogItems.ContraceptionMethods),
                ContraceptionsMethodNotes = GetContraceptionNote(),

                // User Notes
                Notes = GetDailyReflection(phaseName, baseMoodRating, isBleedingDay),

                // New Fields
                BreastTendernessNotes = GetBreastTendernessNote(phaseName),
                BreastTendernessRating = CalculateBreastTendernessRating(phaseName),

                BowelMovementsRegularity = GetRandomItem(CycleTrackerDayLogItems.PeriodBowelMovementIrregularityOptions),
                HadBowelMovements = _random.Next(0, 10) > 1, // 80% chance
                BowelMovementsFrequency = GetRandomItem(CycleTrackerDayLogItems.PeriodBowelMovementFrequencyOptions),

                DoctorsNotes = hasComplication ? "Follow up recommended" : "",
                DoctorResponseTime = hasComplication ? DateTime.Now.AddHours(-_random.Next(1, 24)) : (DateTime?)null
            };
        }

        // ========== PHASE AND CYCLE CALCULATION METHODS ==========

        private static string GetPhaseForDate(DateTime date)
        {
            // Simple simulation of menstrual cycle phases
            var dayOfCycle = (date.Day % 28) + 1; // Simulate 28-day cycle

            return dayOfCycle switch
            {
                <= 7 => "Menstrual",
                <= 14 => "Follicular",
                <= 21 => "Ovulatory",
                _ => "Luteal"
            };
        }

        private static bool IsBleedingDay(DateTime date, string phaseName)
        {
            // Bleeding typically occurs during menstrual phase
            if (phaseName == "Menstrual")
            {
                var dayOfCycle = (date.Day % 28) + 1;
                // Bleeding days 1-5 of menstrual phase
                return dayOfCycle <= 5;
            }
            return false;
        }

        private static string GetPeriodStartOption(bool isBleedingDay)
        {
            if (!isBleedingDay) return "❌ No";
            return _random.Next(0, 10) > 7 ? "✅ Yes, today 🩸" : "❌ No";
        }

        private static string GetPeriodTimingOption()
        {
            var options = new[] { "🕛 Today", "⏪ Yesterday", "❌ No period" };
            return options[_random.Next(options.Length)];
        }

        private static string GetFlowIntensityOption()
        {
            var options = new[] { "🌋 Heavy", "💧 Medium", "💧 Light", "🩸 Spotting" };
            return options[_random.Next(options.Length)];
        }

        // ========== RATING CALCULATION METHODS ==========

        private static int CalculateSymptomsRating(string phaseName, bool isBleedingDay)
        {
            // Symptoms typically highest during menstrual and luteal phases
            return phaseName switch
            {
                "Menstrual" => _random.Next(6, 10),
                "Luteal" => _random.Next(5, 9),
                "Ovulatory" => _random.Next(3, 7),
                _ => _random.Next(2, 6) // Follicular - usually lower symptoms
            };
        }

        private static int CalculateEmotionsRating(int baseMoodRating)
        {
            // Emotions correlate with mood but can be more intense
            return baseMoodRating + _random.Next(-1, 2);
        }

        private static int CalculateCravingsRating(string phaseName)
        {
            // Cravings often higher in luteal phase
            return phaseName switch
            {
                "Luteal" => _random.Next(6, 10),
                "Menstrual" => _random.Next(5, 9),
                _ => _random.Next(3, 7)
            };
        }

        private static int CalculateLifestyleRating(bool hasGoodDay)
        {
            return hasGoodDay ? _random.Next(7, 11) : _random.Next(4, 8);
        }

        private static int CalculateIntercourseRating()
        {
            // Libido often higher around ovulation
            return _random.Next(1, 11);
        }

        private static int CalculateBreastTendernessRating(string phaseName)
        {
            // Breast tenderness often highest in luteal phase
            return phaseName switch
            {
                "Luteal" => _random.Next(6, 10),
                "Menstrual" => _random.Next(5, 8),
                _ => _random.Next(2, 6)
            };
        }

        // ========== NOTE GENERATION METHODS ==========

        private static string GetMoodNote(int rating)
        {
            return rating switch
            {
                >= 9 => "Excellent mood today, feeling very positive and energetic",
                >= 7 => "Good mood, feeling content and balanced",
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

        private static string GetContraceptionNote()
        {
            var notes = new[]
            {
            "Using contraception as prescribed",
            "Consistent with birth control method",
            "No contraception needed today",
            "Following contraception schedule",
            "Comfortable with current method"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetMedicationNotes()
        {
            var notes = new[]
            {
            "Took medication as needed for symptoms",
            "Using heat therapy for cramp relief",
            "Managing symptoms without medication",
            "Herbal remedies helping with discomfort",
            "Pain relief effective for symptoms"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetBreastTendernessNote(string phaseName)
        {
            return phaseName switch
            {
                "Luteal" => "Normal breast tenderness for this phase",
                "Menstrual" => "Some breast sensitivity as expected",
                _ => "Minimal breast tenderness today"
            };
        }

        private static string GetPainLevel(string phaseName, bool isBleedingDay)
        {
            if (!isBleedingDay && phaseName != "Menstrual")
            {
                var noPainOptions = new[] { "😌 None", "😊 Minimal" };
                return noPainOptions[_random.Next(noPainOptions.Length)];
            }

            // Higher pain during bleeding days
            return phaseName switch
            {
                "Menstrual" when isBleedingDay => _random.Next(0, 10) > 7 ? "🤯 Severe" : "😣 Moderate",
                "Menstrual" => "😣 Moderate",
                "Luteal" => "😐 Mild",
                _ => "😌 None"
            };
        }

        private static List<string> GetRandomItems(List<string> source, int min, int max)
        {
            var count = _random.Next(min, max + 1);
            return source.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        private static string GetRandomItem(List<string> source)
        {
            return source[_random.Next(source.Count)];
        }

        private static string GetDailyReflection(string phaseName, int moodRating, bool isBleedingDay)
        {
            var goodReflections = new[]
            {
            $"Feeling good during {phaseName} phase, body feels balanced",
            "Managing cycle symptoms well today",
            "In tune with my body's rhythms",
            "Appreciating the natural cycle of my body",
            "Good energy and mood despite hormonal changes"
        };

            var normalReflections = new[]
            {
            $"Typical {phaseName} phase symptoms, managing okay",
            "Noticing normal hormonal fluctuations",
            "Body going through expected cycle changes",
            "Taking each day as it comes in my cycle",
            "Learning more about my cycle patterns"
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
            "Resting during heavier flow days",
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
