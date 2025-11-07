using OvulaeShared.Helpers.ModuleHelpers.DayLogging;
using OvulaeShared.Models.Menopause;

namespace OvulaeDashboard.Helpers.Test
{
    public class MenopauseLogGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly DateTime _startDate = DateTime.Now.AddDays(-29); // 30 days back

        public static List<MenopauseLogEntry> Generate30DaysMenopauseLogs(int menopauseTrackerLogId = 1)
        {
            var logs = new List<MenopauseLogEntry>();

            for (int i = 0; i < 30; i++)
            {
                var logDate = _startDate.AddDays(i);
                var phaseName = GetRandomPhase(); // Menopause phases are more variable
                logs.Add(GenerateDailyLog(menopauseTrackerLogId, phaseName, logDate, i + 1));
            }

            return logs;
        }

        private static MenopauseLogEntry GenerateDailyLog(int trackerLogId, string phaseName, DateTime logDate, int entryId)
        {
            var hasGoodDay = _random.Next(0, 10) > 3; // 60% good days
            var hasComplication = _random.Next(0, 20) == 1; // 5% chance of complication
            var hasBleeding = phaseName == "Perimenopause" && _random.Next(0, 10) > 7; // 20% chance in perimenopause

            // Base mood rating that influences other ratings
            var baseMoodRating = _random.Next(hasGoodDay ? 6 : 3, hasGoodDay ? 10 : 7);

            return new MenopauseLogEntry
            {
                EntryId = entryId,
                MenopauseTrackerLogId = trackerLogId,
                LogDate = logDate,
                PhaseName = phaseName,

                // Core Symptoms
                Symptoms = GetRandomItems(MenopauseTrackerDayLogItems.Symptoms, 1, 4),
                SleepQuality = GetSleepQuality(hasGoodDay, phaseName),
                Moods = GetRandomItems(MenopauseTrackerDayLogItems.Mood, 1, 3),

                // Vasomotor Symptoms
                HotFlashesSeverity = GetHotFlashSeverity(phaseName),
                NightSweatsSeverity = GetNightSweatSeverity(phaseName),
                Libido = GetLibidoLevel(phaseName),
                EnergyLevel = GetEnergyLevel(hasGoodDay, phaseName),

                // Hormone Therapy
                IsOnHormoneTherapy = _random.Next(0, 10) > 6, // 30% chance
                HormoneTherapyNotes = GetHormoneTherapyNotes(),

                // Bleeding
                HadBleeding = hasBleeding,
                BleedingType = hasBleeding ? GetRandomItem(MenopauseTrackerDayLogItems.BleedingTypes) : "",
                BleedingColor = hasBleeding ? GetRandomItem(MenopauseTrackerDayLogItems.BleedingColors) : "",

                // Health Red Flags
                HadAbnormalBleeding = hasComplication && hasBleeding,
                BleedingPattern = hasComplication ? GetRandomItem(MenopauseTrackerDayLogItems.BleedingPatterns) : "",
                HadPelvicPain = _random.Next(0, 10) > 8, // 10% chance
                PainSeverity = GetPainSeverity(),

                // Lifestyle
                LifestyleFactors = GetRandomItems(MenopauseTrackerDayLogItems.LifestyleFactors, 1, 3),
                Notes = GetDailyReflection(phaseName, baseMoodRating, hasBleeding),

                // Urinary Health
                UrinationRating = CalculateUrinationRating(phaseName),
                CoughWeeYesNo = _random.Next(0, 10) > 7, // 20% chance
                CoughWeeNotes = GetUrinaryNote("cough"),
                SneezeWeeYesNo = _random.Next(0, 10) > 7, // 20% chance
                SneezeWeeNotes = GetUrinaryNote("sneeze"),
                LaughWeeYesNo = _random.Next(0, 10) > 7, // 20% chance
                LaughWeeNotes = GetUrinaryNote("laugh"),
                BladderPainYesNo = _random.Next(0, 10) > 8, // 10% chance
                BladderPainNotes = GetBladderPainNote(),
                UTIOften = _random.Next(0, 20) == 1, // 5% chance

                // Physical Changes
                HairLossYesNo = _random.Next(0, 10) > 6, // 30% chance
                HairLossNotes = GetHairLossNote(),
                WeightGainYesNo = _random.Next(0, 10) > 5, // 40% chance
                WeightGainNotes = GetWeightGainNote(),
                WeightFluctuate = _random.Next(0, 10) > 4, // 50% chance
                WeightFluctuateNormal = _random.Next(0, 10) > 3, // 60% chance
                SkinDrynessRating = CalculateSkinDrynessRating(phaseName),
                BreastTendernessRating = CalculateBreastTendernessRating(phaseName),
                BlemishShowing = _random.Next(0, 10) > 7, // 20% chance

                // Blood Pressure
                BloodPressureDaily = _random.Next(0, 10) > 3, // 60% chance recorded
                BloodPressureReadings = GetBloodPressureReading(hasComplication),

                // Doctor's Notes
                DoctorsNotes = hasComplication ? "Follow up recommended" : "",
                DoctorResponseTime = hasComplication ? DateTime.Now.AddHours(-_random.Next(1, 24)) : (DateTime?)null
            };
        }

        // ========== PHASE AND SEVERITY CALCULATION METHODS ==========

        private static string GetRandomPhase()
        {
            var phases = new[] { "Perimenopause", "Menopause", "Postmenopause" };
            return phases[_random.Next(phases.Length)];
        }

        private static string GetHotFlashSeverity(string phaseName)
        {
            // Hot flashes most common in perimenopause and early menopause
            return phaseName switch
            {
                "Perimenopause" => _random.Next(0, 10) > 3 ? "Moderate" : "Severe",
                "Menopause" => _random.Next(0, 10) > 5 ? "Mild" : "Moderate",
                _ => _random.Next(0, 10) > 7 ? "None" : "Mild" // Postmenopause
            };
        }

        private static string GetNightSweatSeverity(string phaseName)
        {
            // Similar pattern to hot flashes
            return phaseName switch
            {
                "Perimenopause" => _random.Next(0, 10) > 4 ? "Moderate" : "Severe",
                "Menopause" => _random.Next(0, 10) > 6 ? "Mild" : "Moderate",
                _ => _random.Next(0, 10) > 8 ? "None" : "Mild"
            };
        }

        private static string GetLibidoLevel(string phaseName)
        {
            // Libido can vary widely during menopause transition
            var levels = new[] { "Low", "Normal", "High" };
            return phaseName switch
            {
                "Perimenopause" => _random.Next(0, 10) > 6 ? "Low" : "Normal",
                "Menopause" => _random.Next(0, 10) > 7 ? "Low" : "Normal",
                _ => levels[_random.Next(levels.Length)] // Postmenopause more variable
            };
        }

        private static string GetEnergyLevel(bool hasGoodDay, string phaseName)
        {
            // Energy often affected during transition
            if (hasGoodDay) return "High";

            return phaseName switch
            {
                "Perimenopause" => _random.Next(0, 10) > 6 ? "Low" : "Normal",
                "Menopause" => _random.Next(0, 10) > 5 ? "Low" : "Normal",
                _ => "Normal" // Postmenopause often more stable
            };
        }

        private static string GetSleepQuality(bool hasGoodDay, string phaseName)
        {
            // Sleep often disrupted by night sweats
            if (hasGoodDay) return "Good";

            return phaseName switch
            {
                "Perimenopause" => _random.Next(0, 10) > 6 ? "Poor" : "Moderate",
                "Menopause" => _random.Next(0, 10) > 5 ? "Poor" : "Moderate",
                _ => _random.Next(0, 10) > 7 ? "Good" : "Moderate"
            };
        }

        // ========== RATING CALCULATION METHODS ==========

        private static int CalculateUrinationRating(string phaseName)
        {
            // Urinary symptoms can increase during menopause
            return phaseName switch
            {
                "Perimenopause" => _random.Next(5, 9),
                "Menopause" => _random.Next(4, 8),
                _ => _random.Next(3, 7) // Postmenopause
            };
        }

        private static int CalculateSkinDrynessRating(string phaseName)
        {
            // Skin dryness often increases with declining estrogen
            return phaseName switch
            {
                "Perimenopause" => _random.Next(4, 8),
                "Menopause" => _random.Next(5, 9),
                _ => _random.Next(6, 10) // Postmenopause
            };
        }

        private static int CalculateBreastTendernessRating(string phaseName)
        {
            // Breast tenderness can fluctuate during transition
            return phaseName switch
            {
                "Perimenopause" => _random.Next(5, 9), // Often higher due to hormonal fluctuations
                "Menopause" => _random.Next(3, 7),
                _ => _random.Next(2, 6) // Postmenopause
            };
        }

        // ========== NOTE GENERATION METHODS ==========

        private static string GetHormoneTherapyNotes()
        {
            var notes = new[]
            {
            "Taking hormone therapy as prescribed",
            "Adjusting well to hormone treatment",
            "Noticing improvement in symptoms with therapy",
            "Following up with doctor about hormone therapy",
            "Managing symptoms with current hormone regimen"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetUrinaryNote(string trigger)
        {
            var notes = new[]
            {
            $"Noticed leakage when {trigger}ing, using pelvic floor exercises",
            $"Managing {trigger}-induced leakage with lifestyle adjustments",
            $"Occasional leakage when {trigger}ing, monitoring pattern",
            $"Using protection for {trigger}-related leakage",
            $"Working on pelvic floor strength for {trigger} leakage"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetBladderPainNote()
        {
            var notes = new[]
            {
            "Mild bladder discomfort, staying hydrated",
            "Some bladder pain, monitoring for UTI symptoms",
            "Bladder sensitivity, avoiding irritants",
            "Managing bladder pain with heat therapy",
            "Noticing bladder changes with hormonal fluctuations"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetHairLossNote()
        {
            var notes = new[]
            {
            "Noticing some hair thinning, normal for this phase",
            "Hair loss seems to be stabilizing",
            "Using gentle hair care for thinning hair",
            "Hair changes consistent with hormonal shifts",
            "Monitoring hair loss pattern with doctor"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetWeightGainNote()
        {
            var notes = new[]
            {
            "Weight changes consistent with menopause transition",
            "Managing weight with diet and exercise adjustments",
            "Noticing metabolic changes, adapting lifestyle",
            "Weight fluctuations are normal during this phase",
            "Working on maintaining healthy weight during transition"
        };
            return notes[_random.Next(notes.Length)];
        }

        private static string GetPainSeverity()
        {
            var severities = new[] { "Mild", "Moderate", "Severe" };
            return severities[_random.Next(severities.Length)];
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

        private static string GetDailyReflection(string phaseName, int moodRating, bool hasBleeding)
        {
            var goodReflections = new[]
            {
            $"Managing {phaseName} symptoms well today, feeling positive",
            "Adapting well to body changes during this transition",
            "Feeling empowered and informed about menopause journey",
            "Good day despite hormonal changes, staying proactive",
            "Appreciating the wisdom that comes with this life phase"
        };

            var normalReflections = new[]
            {
            $"Typical {phaseName} day, managing symptoms as expected",
            "Navigating the changes of this transition phase",
            "Learning to listen to my body's new rhythms",
            "Taking each day as it comes during this transition",
            "Noticing the gradual changes of this life phase"
        };

            var lowReflections = new[]
            {
            "Menopause symptoms making today challenging",
            "Feeling the effects of hormonal fluctuations today",
            "Taking it easy due to menopausal discomfort",
            "Reminding myself this transition is natural and temporary",
            "Using self-care to manage tougher menopausal days"
        };

            var bleedingReflections = new[]
            {
            "Unexpected bleeding today, monitoring closely",
            "Managing perimenopausal bleeding patterns",
            "Noticing changes in menstrual patterns during transition",
            "Tracking irregular bleeding as part of menopause journey",
            "Adapting to changing menstrual patterns"
        };

            if (hasBleeding) return bleedingReflections[_random.Next(bleedingReflections.Length)];

            return moodRating switch
            {
                >= 8 => goodReflections[_random.Next(goodReflections.Length)],
                >= 5 => normalReflections[_random.Next(normalReflections.Length)],
                _ => lowReflections[_random.Next(lowReflections.Length)]
            };
        }
    }

}
