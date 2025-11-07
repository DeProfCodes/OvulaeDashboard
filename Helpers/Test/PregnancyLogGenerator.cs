using Newtonsoft.Json;
using OvulaeShared.Helpers.ModuleHelpers.DayLogging;
using OvulaeShared.Models.Pregnancy;

namespace OvulaeDashboard.Helpers.Test
{
    public class PregnancyLogGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly DateTime _startDate = DateTime.Now.AddDays(-29); // 30 days back

        public static List<PregnancyLogEntryItem> Generate30DaysPregnancyLogs(int pregnancyTrackerLogId = 2, int startWeek = 11)
        {
            var logs = new List<PregnancyLogEntryItem>();

            for (int i = 0; i < 30; i++)
            {
                var logDate = _startDate.AddDays(i);
                var currentWeek = startWeek + (i / 7); // Progress through weeks
                var dayOfWeek = ((int)logDate.DayOfWeek + 6) % 7 + 1; // Monday = 1, Sunday = 7

                logs.Add(GenerateDailyLog(pregnancyTrackerLogId, currentWeek, dayOfWeek, logDate, i + 1));
            }

            return logs;
        }

        private static PregnancyLogEntryItem GenerateDailyLog(int trackerLogId, int week, int dayOfWeek, DateTime logDate, int entryId)
        {
            var hasGoodDay = _random.Next(0, 10) > 3; // 60% good days
            var hasComplication = _random.Next(0, 20) == 1; // 5% chance of complication

            // Base mood rating that influences other ratings
            var baseMoodRating = _random.Next(hasGoodDay ? 6 : 3, hasGoodDay ? 10 : 7);

            return new PregnancyLogEntryItem
            {
                EntryId = entryId,
                PregnancyTrackerLogId = trackerLogId,
                Week = week,
                DayOfWeekNo = dayOfWeek,
                LogDate = logDate,

                // Moods - with rating 1-10
                Moods = GetRandomItems(PregnancyDayLogItems.Moods, 1, 3),
                MoodsRating = baseMoodRating,
                MoodsNotes = GetMoodNote(baseMoodRating),

                // Stress - with rating 1-10 (inverse of mood)
                StressLevel = GetRandomItem(PregnancyDayLogItems.StressLevels),
                StressRating = 10 - baseMoodRating + _random.Next(-1, 2), // Inverse correlation with mood
                StressNotes = GetStressNote(10 - baseMoodRating),

                // Symptoms - with rating 1-10
                Symptoms = GetRandomItems(PregnancyDayLogItems.Symptoms, 1, 4),
                SymptomsRating = CalculateSymptomsRating(week, hasGoodDay),
                SymptomsNotes = GetSymptomsNote(),

                // Cravings - with rating 1-10
                Cravings = GetRandomItems(PregnancyDayLogItems.Cravings, 0, 2),
                CravingsRating = _random.Next(1, 11),
                CravingsNotes = GetCravingsNote(),

                // Appetite - with rating 1-10
                AppetiteLevel = GetRandomItem(PregnancyDayLogItems.AppetiteLevels),
                AppetiteRating = CalculateAppetiteRating(hasGoodDay),
                AppetiteNotes = GetAppetiteNote(),

                // Energy - with rating 1-10
                EnergyLevel = GetRandomItem(PregnancyDayLogItems.EnergyLevels),
                EnergyRating = CalculateEnergyRating(week, hasGoodDay),
                EnergyNotes = GetEnergyNote(),

                // Baby Movements - with rating 1-10
                BabyMovements = GetRandomItems(PregnancyDayLogItems.BabyMovements, 1, 2),
                BabyMovementsRating = CalculateBabyMovementsRating(week),
                BabyMovementsNotes = GetBabyMovementsNote(),
                FeltBabyMove = _random.Next(0, 10) > 1, // 80% chance felt movement
                MovementFrequency = week > 16 ? "Frequent" : "Occasional",

                // Activities - with rating 1-10
                Activities = GetRandomItems(PregnancyDayLogItems.PregnancyActivities, 1, 3),
                ActivitiesRating = CalculateActivitiesRating(hasGoodDay, baseMoodRating),
                ActivitiesNotes = GetActivitiesNote(),

                // Vitamins - with rating 1-10
                Vitamins = GetRandomItems(PregnancyDayLogItems.VitaminSupplements, 1, 2),
                VitaminsRating = CalculateVitaminsRating(),
                VitaminsNotes = GetVitaminsNote(),

                // Checkups - occasional doctor visits
                Checkups = _random.Next(0, 10) == 1 ? GetRandomItems(PregnancyDayLogItems.Checkups, 1, 1) : new List<string>(),

                // Sleep - with rating 1-10
                SleepQuality = GetRandomItem(PregnancyDayLogItems.SleepQuality),
                SleepRating = CalculateSleepRating(hasGoodDay, week),
                SleepNotes = GetSleepNote(),

                // Discomforts - with rating 1-10
                HadUnusualDiscomfort = _random.Next(0, 10) > 6, // 30% chance
                DiscomfortDescription = GetRandomItems(PregnancyDayLogItems.Discomforts, 0, 2),
                DiscomfortRating = CalculateDiscomfortRating(week),
                DiscomfortNotes = GetDiscomfortNote(),

                // Bleeding/Leakage - with rating 1-10 (only if experienced)
                ExperiencedBleedingOrLeakage = hasComplication,
                BleedingDetails = hasComplication ? GetRandomItem(PregnancyDayLogItems.BleedingOrLeakage) : "",
                BleedingRating = hasComplication ? _random.Next(4, 9) : 0,
                BleedingNotes = hasComplication ? "Monitoring closely" : "",

                // Reflection - realistic daily thoughts
                Reflection = GetDailyReflection(week, baseMoodRating, hasComplication),

                // Bowel movements
                BowelMovementsRegularity = _random.Next(0, 10) > 2 ? "Regular" : "Irregular",
                HadBowelMovements = _random.Next(0, 10) > 1, // 80% chance
                BowelMovementsFrequency = _random.Next(0, 10) > 5 ? "Once daily" : "Twice daily",

                // Water intake - using your actual UI options
                WaterIntake = GetRandomItem(PregnancyDayLogItems.WaterIntakeCups),
                NighlyUrination = GetRandomItem(PregnancyDayLogItems.NightUrination),

                // Blood pressure - realistic ranges
                BloodPressureDaily = _random.Next(0, 10) > 2, // 70% chance recorded
                BloodPressureReadings = GetBloodPressureReading(hasComplication),
                OnBloodPressureMedication = false,

                // Breast feeling - with rating 1-10
                BrestFeeling = GetRandomItem(PregnancyDayLogItems.BrestFeeeling),
                BrestFeelingRating = CalculateBreastFeelingRating(week),
                BrestFeelingNotes = GetBreastFeelingNote(),

                // UTI
                UTIOften = _random.Next(0, 20) == 1 // 5% chance
            };
        }

        // ========== RATING CALCULATION METHODS ==========

        private static int CalculateSymptomsRating(int week, bool hasGoodDay)
        {
            // Symptoms tend to increase in first trimester, decrease in second
            var baseRating = week < 14 ? _random.Next(5, 9) : _random.Next(3, 7);
            return hasGoodDay ? baseRating - 1 : baseRating + 1;
        }

        private static int CalculateAppetiteRating(bool hasGoodDay)
        {
            return hasGoodDay ? _random.Next(6, 10) : _random.Next(3, 7);
        }

        private static int CalculateEnergyRating(int week, bool hasGoodDay)
        {
            // Energy typically improves in second trimester
            var baseRating = week < 14 ? _random.Next(3, 7) : _random.Next(5, 9);
            return hasGoodDay ? baseRating + 1 : baseRating - 1;
        }

        private static int CalculateBabyMovementsRating(int week)
        {
            // Movements increase as pregnancy progresses
            return week < 16 ? _random.Next(4, 7) : _random.Next(6, 10);
        }

        private static int CalculateActivitiesRating(bool hasGoodDay, int moodRating)
        {
            // Activities correlate with mood and energy
            return (moodRating + (hasGoodDay ? 2 : 0)) / 2 + _random.Next(-1, 2);
        }

        private static int CalculateVitaminsRating()
        {
            // Most people are consistent with vitamins
            return _random.Next(0, 10) > 1 ? _random.Next(8, 11) : _random.Next(3, 6);
        }

        private static int CalculateSleepRating(bool hasGoodDay, int week)
        {
            // Sleep quality often decreases as pregnancy progresses
            var baseRating = week < 14 ? _random.Next(6, 9) : _random.Next(4, 7);
            return hasGoodDay ? baseRating + 1 : baseRating - 1;
        }

        private static int CalculateDiscomfortRating(int week)
        {
            // Discomfort increases with pregnancy progression
            return week < 14 ? _random.Next(2, 5) : _random.Next(4, 8);
        }

        private static int CalculateBreastFeelingRating(int week)
        {
            // Breast tenderness often highest in first trimester
            return week < 14 ? _random.Next(5, 9) : _random.Next(3, 6);
        }

        // ========== NOTE GENERATION METHODS ==========

        private static string GetMoodNote(int rating)
        {
            return rating switch
            {
                >= 9 => "Excellent mood today, feeling very positive",
                >= 7 => "Good mood, feeling content and happy",
                >= 5 => "Neutral mood, typical day",
                >= 3 => "Feeling a bit low, but managing",
                _ => "Difficult day emotionally"
            };
        }

        private static string GetStressNote(int rating)
        {
            return rating switch
            {
                >= 8 => "High stress levels today",
                >= 6 => "Moderate stress, but coping",
                >= 4 => "Manageable stress",
                >= 2 => "Low stress, relaxed day",
                _ => "Very relaxed, no stress"
            };
        }

        private static string GetSymptomsNote()
        {
            var notes = new[]
            {
            "Typical pregnancy symptoms today",
            "Managing symptoms with rest and hydration",
            "Symptoms are noticeable but not overwhelming",
            "Some discomfort but able to continue daily activities"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetCravingsNote()
        {
            var notes = new[]
            {
            "Craving sweets today",
            "Wanted salty snacks",
            "No strong cravings",
            "Really wanted fruits",
            "Cravings manageable with healthy alternatives"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetAppetiteNote()
        {
            var notes = new[]
            {
            "Eating well despite nausea",
            "Good appetite today",
            "Appetite affected by symptoms",
            "Eating small, frequent meals",
            "Following hunger cues"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetEnergyNote()
        {
            var notes = new[]
            {
            "Good energy for activities",
            "Needed more rest today",
            "Energy levels fluctuating",
            "Paced myself throughout the day",
            "Feeling energetic and productive"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetBabyMovementsNote()
        {
            var notes = new[]
            {
            "Baby active after meals",
            "Felt regular movements throughout day",
            "Quiet periods followed by activity",
            "Stronger movements noticed",
            "Baby responding to touch and voice"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetActivitiesNote()
        {
            var notes = new[]
            {
            "Stayed active as recommended",
            "Light activity felt good",
            "Took it easy today",
            "Enjoyed gentle movement",
            "Balanced activity with rest"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetVitaminsNote()
        {
            var notes = new[]
            {
            "Took supplements with breakfast",
            "Remembered all vitamins today",
            "Missed one supplement",
            "Consistent with vitamin routine",
            "Taking vitamins as prescribed"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetSleepNote()
        {
            var notes = new[]
            {
            "Slept well through the night",
            "Frequent bathroom trips disrupted sleep",
            "Comfortable sleeping position found",
            "Restless night but some good sleep",
            "Used pillows for better support"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetDiscomfortNote()
        {
            var notes = new[]
            {
            "Managed with rest and hydration",
            "Using recommended positions for relief",
            "Mild discomfort, not limiting activities",
            "Taking it easy due to discomfort",
            "Discomfort improved with movement"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetBreastFeelingNote()
        {
            var notes = new[]
            {
            "Normal pregnancy changes",
            "Some tenderness but manageable",
            "Using supportive bras for comfort",
            "Breast changes progressing as expected",
            "Noticing growth and sensitivity"
        };
            return notes[_random.Next(notes.Length)];
        }

        // ========== HELPER METHODS ==========

        private static List<string> GetRandomItems(List<string> source, int min, int max)
        {
            var count = _random.Next(min, max + 1);
            return source.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        private static string GetRandomItem(List<string> source)
        {
            return source[_random.Next(source.Count)];
        }

        private static string GetBloodPressureReading(bool hasComplication)
        {
            if (hasComplication)
            {
                return _random.Next(130, 145) + "/" + _random.Next(80, 95);
            }
            else
            {
                return _random.Next(100, 120) + "/" + _random.Next(60, 80);
            }
        }

        private static string GetDailyReflection(int week, int moodRating, bool hasComplication)
        {
            var goodReflections = new[]
            {
            $"Feeling baby move more in week {week} - such a wonderful feeling!",
            "Enjoying this pregnancy journey, feeling connected to my baby",
            "Had a great day, baby seems happy and active",
            "Feeling grateful for this experience, can't wait to meet my little one",
            "Noticed baby responding to music today - amazing!"
        };

            var normalReflections = new[]
            {
            $"Week {week} brings new changes, adjusting well",
            "Typical pregnancy day, managing symptoms okay",
            "Baby is growing well, feeling the changes in my body",
            "Thinking about baby names and nursery preparations",
            "Learning more about pregnancy each day"
        };

            var lowReflections = new[]
            {
            "Feeling tired but pushing through",
            "Some tough moments but focusing on the positive",
            "Taking it one day at a time",
            "Reminding myself this is temporary and worth it",
            "Leaning on support system today"
        };

            var complicationReflections = new[]
            {
            "Noticed some concerning symptoms today, will monitor closely",
            "Feeling a bit worried, will contact doctor if continues",
            "Taking it easy today due to some discomfort",
            "Being extra cautious and resting more",
            "Monitoring symptoms and staying in touch with healthcare provider"
        };

            if (hasComplication) return complicationReflections[_random.Next(complicationReflections.Length)];

            return moodRating switch
            {
                >= 8 => goodReflections[_random.Next(goodReflections.Length)],
                >= 5 => normalReflections[_random.Next(normalReflections.Length)],
                _ => lowReflections[_random.Next(lowReflections.Length)]
            };
        }
    }
}
