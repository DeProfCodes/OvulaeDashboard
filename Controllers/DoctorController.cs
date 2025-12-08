using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OvulaeDashboard.Helpers;
using OvulaeDashboard.Helpers.Constants;
using OvulaeDashboard.Helpers.Test;
using OvulaeDashboard.Helpers.ViewHelper;
using OvulaeDashboard.Services;
using OvulaeDashboard.ViewModels.Affiliate;
using OvulaeDashboard.ViewModels.Doctor;
using OvulaeShared.Enums.Affiliate;
using OvulaeShared.Enums.App;
using OvulaeShared.Enums.User;
using OvulaeShared.Models.Menopause;
using OvulaeShared.Models.Ovulation;
using OvulaeShared.Models.PeriodTracker;
using OvulaeShared.Models.Pregnancy;
using OvulaeShared.Models.WebApi;
using OvulaeShared.Services.APIs.Affiliates;
using OvulaeShared.Services.APIs.Doctors;
using OvulaeShared.Services.APIs.ModuleServices;
using OvulaeShared.Services.APIs.Transactions;
using OvulaeShared.Services.APIs.Users;
using OvulaeShared.ViewModel.Affiliates;
using OvulaeShared.ViewModel.Doctor;
using OvulaeShared.ViewModel.User;
using System.Linq;

namespace OvulaeDashboard.Controllers
{
    //[Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorsApi _doctorsApi;
        private readonly IAffiliatesApi _affApi;
        private readonly ISessionService _session;
        private readonly ITransactionsApi _transApi;
        private readonly IUsersApi _usersApi;
        private readonly IModuleLogsApi _moduleLogsApi;
        public DoctorController(IDoctorsApi doctorsApi, IAffiliatesApi affApi, ISessionService session, ITransactionsApi transApi, IUsersApi usersApi, IModuleLogsApi moduleLogsApi)
        {
            _doctorsApi = doctorsApi;
            _affApi = affApi;
            _session = session;
            _transApi = transApi;
            _usersApi = usersApi;
            _moduleLogsApi = moduleLogsApi;
        }
        //elmarie.w@ovulae.com -> Elmarie1!
        [HttpGet]
        public async Task<IActionResult> DoctorDashboard()
        {
            try
            {
                var drDashboardData = await _doctorsApi.GetDoctorDashboardDetails(_session.GetSecureApiRequestDto());
                
                var drDataVM = AffiliateViewsHelper.GetDoctorDashboardViewModel(drDashboardData, _session.GetDashboardCurrency());
                
                return PartialView(AppPagesViewsUrl.DoctorDashboardPageLink, drDataVM);
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Logout", "Authentication");
            }
            return View(new AffiliateMainDashboardViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> DoctorPatients()
        {
            var result = new List<UserDoctorPatientDetailsViewModel>();
            try
            {
                var data = await _doctorsApi.GetDoctorAllPatientsList(_session.GetSecureApiRequestDto());
                result = data;
            }
            catch
            {

            }
            return PartialView(AppPagesViewsUrl.DoctorPatientsPageLink, result);
        }

        [HttpGet]
        public async Task<IActionResult> DoctorsPatientDetails(string patientUserId)
        {
            var result = new PatientProfileViewModel();
            try
            {
                var dto = new DoctorPatientRequest
                {
                    DoctorUserId = _session.GetSecureApiRequestDto().UserId,
                    PatientUserId = patientUserId,
                    DoctorEmail = "",
                    PatientEmail = ""
                };
                
                var data = await _doctorsApi.GetDoctorsPatientProfile(dto);

                result = data;
            }
            catch
            {

            }
            return PartialView(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateModuleLogDoctorsNotes(UpdateDoctorsNotesDTO updateDrNotesDTO)
        {
            try
            {
                var result = new GenericResult();
                var success = false;
                if (updateDrNotesDTO.ModuleType == ModuleType.Pregnancy)
                {
                    var dbLog = await _moduleLogsApi.GetPregnancyLogs(updateDrNotesDTO.PatientUserId);

                    var logEntry = dbLog.Entries.FirstOrDefault(x => x.EntryId == updateDrNotesDTO.EntryId && x.PregnancyTrackerLogId == updateDrNotesDTO.ModuleLogId);

                    logEntry.DoctorsNotes = updateDrNotesDTO.Notes;
                    logEntry.DoctorResponseTime = DateTime.Now;

                    result = await _moduleLogsApi.UpdatePregnancyLogEntryDR(updateDrNotesDTO.PatientUserId, logEntry);
                    success = result.Success;
                }
                else if (updateDrNotesDTO.ModuleType == ModuleType.PeriodTracker)
                {
                    var dbLog = await _moduleLogsApi.GetPeriodLogs(updateDrNotesDTO.PatientUserId);

                    var logEntry = dbLog.Logs.FirstOrDefault(x => x.EntryId == updateDrNotesDTO.EntryId && x.PeriodTrackerLogId == updateDrNotesDTO.ModuleLogId);

                    logEntry.DoctorsNotes = updateDrNotesDTO.Notes;
                    logEntry.DoctorResponseTime = DateTime.Now;

                    result = await _moduleLogsApi.UpdatePeriodLogEntryDR(updateDrNotesDTO.PatientUserId, logEntry);
                    success = result.Success;
                }
                else if (updateDrNotesDTO.ModuleType == ModuleType.MenopauseTracker)
                {
                    var dbLog = await _moduleLogsApi.GetMenopauseLogs(updateDrNotesDTO.PatientUserId);

                    var logEntry = dbLog.Entries.FirstOrDefault(x => x.EntryId == updateDrNotesDTO.EntryId && x.MenopauseTrackerLogId == updateDrNotesDTO.ModuleLogId);

                    logEntry.DoctorsNotes = updateDrNotesDTO.Notes;
                    logEntry.DoctorResponseTime = DateTime.Now;

                    result = await _moduleLogsApi.UpdateMenopauseLogEntryDR(updateDrNotesDTO.PatientUserId, logEntry);
                    success = result.Success;
                }
                else if (updateDrNotesDTO.ModuleType == ModuleType.Ovulation)
                {
                    var dbLog = await _moduleLogsApi.GetOvulationLogs(updateDrNotesDTO.PatientUserId);

                    var logEntry = dbLog.CycleTrackingHistory.FirstOrDefault(x => x.EntryId == updateDrNotesDTO.EntryId && x.OvulationTrackerLogId == updateDrNotesDTO.ModuleLogId);

                    logEntry.DoctorsNotes = updateDrNotesDTO.Notes;
                    logEntry.DoctorResponseTime = DateTime.Now;

                    result = await _moduleLogsApi.UpdateOvulationLogEntryDR(updateDrNotesDTO.PatientUserId, logEntry);
                    success = result.Success;
                }

                if (success)
                {
                    return Ok(new { success = result.Success });
                }
                else
                {
                    return BadRequest(new { success = false, error = result.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = "Unknown error encounted." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> DoctorDetails()
        {
            try
            {
                var affiliateDashboardData = await _affApi.GetAffiliateDashboardDetails(_session.GetSecureApiRequestDto());
                if (_session.GetSecureApiRequestDto().Email == "test5000@gmail.com")
                {
                    affiliateDashboardData = AffiliateMockDataGenerator.GenerateSuccessfulAffiliateData("affiliate_123");
                }
                var viewModel = AffiliateViewsHelper.GetAffiliateDetailsViewModel(affiliateDashboardData, _session.GetDashboardCurrency());

                return PartialView(AppPagesViewsUrl.DoctorDetailsPageLink, viewModel);
            }
            catch
            {
                return PartialView(AppPagesViewsUrl.DoctorDetailsPageLink, new AffiliateDetailsViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoctorTransactions()
        {
            try
            {
                var transactions = await _transApi.GetUserTransactions(_session.GetSecureApiRequestDto());
                transactions = transactions != null ? transactions : new();

                return PartialView(AppPagesViewsUrl.DoctorTransactionsPageLink, transactions);
            }
            catch
            {
                return PartialView(AppPagesViewsUrl.DoctorTransactionsPageLink, new AffiliateDetailsViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientTrends(string patientUserId, DateTime startDate, DateTime endDate, string period = "week", string trackerType = "all", string metric = "mood")
        {
            try
            {
                var dto = new DoctorPatientRequest
                {
                    DoctorUserId = _session.GetSecureApiRequestDto().UserId,
                    PatientUserId = patientUserId,
                    DoctorEmail = "",
                    PatientEmail = ""
                };
                var patientProfile = await _doctorsApi.GetDoctorsPatientProfile(dto);
                var filteredLogs = FilterAndCombineLogs(patientProfile, startDate, endDate, trackerType);
                var aggregatedData = AggregateLogsByPeriod(filteredLogs, startDate, endDate, period);
                var model = new PatientTrendsViewModel
                {
                    PatientUserId = patientUserId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Period = period,
                    TrackerType = trackerType,
                    DataPoints = aggregatedData,
                    Summary = CalculateSummary(aggregatedData)
                };
                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientLogsByDateRange(string patientUserId, DateTime startDate, DateTime endDate, string trackerType = "all", string currentTracker = "")
        {
            try
            {
                var dto = new DoctorPatientRequest
                {
                    DoctorUserId = _session.GetSecureApiRequestDto().UserId,
                    PatientUserId = patientUserId,
                    DoctorEmail = "",
                    PatientEmail = ""
                };
                var patientProfile = await _doctorsApi.GetDoctorsPatientProfile(dto);
                var allLogs = FilterAndCombineLogsDetailed(patientProfile, startDate, endDate, trackerType);
                var currentTrackerLogs = new List<DetailedLogEntry>();
                var otherTrackerLogs = new List<DetailedLogEntry>();

                foreach (var log in allLogs)
                {
                    if (log.TrackerType.ToLower() == currentTracker.ToLower())
                        currentTrackerLogs.Add(log);
                    else
                        otherTrackerLogs.Add(log);
                }

                currentTrackerLogs = currentTrackerLogs.OrderByDescending(l => l.LogDate).ToList();
                otherTrackerLogs = otherTrackerLogs.OrderByDescending(l => l.LogDate).ToList();

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        currentTrackerLogs,
                        otherTrackerLogs,
                        dateRange = $"{startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}",
                        totalLogs = allLogs.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult RenderLogPartial([FromBody] RenderPartialRequest request)
        {
            try
            {
                object viewModel = null;
                switch (request.ViewModelType)
                {
                    case "PregnancyLogsViewModel":
                        viewModel = JsonConvert.DeserializeObject<PregnancyLogsViewModel>(JsonConvert.SerializeObject(request.ViewModel));
                        break;
                    case "PeriodLogsViewModel":
                        viewModel = JsonConvert.DeserializeObject<PeriodLogsViewModel>(JsonConvert.SerializeObject(request.ViewModel));
                        break;
                    case "OvulationLogsViewModel":
                        viewModel = JsonConvert.DeserializeObject<OvulationLogsViewModel>(JsonConvert.SerializeObject(request.ViewModel));
                        break;
                    case "MenopauseLogsViewModel":
                        viewModel = JsonConvert.DeserializeObject<MenopauseLogsViewModel>(JsonConvert.SerializeObject(request.ViewModel));
                        break;
                }
                ViewBag.IsCurrent = request.IsCurrent;
                return PartialView(request.PartialViewName, viewModel);
            }
            catch (Exception ex)
            {
                return Content($"<div class='alert alert-danger'>Error rendering view: {ex.Message}</div>");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientLogsWithDetails(string patientUserId, DateTime? startDate, DateTime? endDate, string trackerType = "all", string currentTracker = "")
        {
            try
            {
                var dto = new DoctorPatientRequest
                {
                    DoctorUserId = _session.GetSecureApiRequestDto().UserId,
                    PatientUserId = patientUserId,
                    DoctorEmail = "",
                    PatientEmail = ""
                };

                var patientProfile = await _doctorsApi.GetDoctorsPatientProfile(dto);
                startDate ??= DateTime.Now.AddDays(-30);
                endDate ??= DateTime.Now;

                var result = new
                {
                    PregnancyLogs = new List<PregnancyLogEntryItem>(),
                    PeriodLogs = new List<PeriodLogEntry>(),
                    OvulationLogs = new List<OvulationCycleLog>(),
                    MenopauseLogs = new List<MenopauseLogEntry>(),
                    CurrentTracker = currentTracker
                };

                // Filter and prepare logs based on tracker type
                if (trackerType == "all" || trackerType == "pregnancy")
                {
                    var pregLogs = patientProfile.PregnancyLogs?.Where(x => x.LogDate >= startDate && x.LogDate <= endDate).ToList() ?? new List<PregnancyLogEntryItem>();
                    result = new
                    {
                        PregnancyLogs = pregLogs,
                        PeriodLogs = result.PeriodLogs,
                        OvulationLogs = result.OvulationLogs,
                        MenopauseLogs = result.MenopauseLogs,
                        CurrentTracker = currentTracker
                    };
                }

                if (trackerType == "all" || trackerType == "period")
                {
                    var periodLogs = patientProfile.PeriodLogs?.Where(x => x.LogDate >= startDate && x.LogDate <= endDate).ToList() ?? new List<PeriodLogEntry>();
                    result = new
                    {
                        PregnancyLogs = result.PregnancyLogs,
                        PeriodLogs = periodLogs,
                        OvulationLogs = result.OvulationLogs,
                        MenopauseLogs = result.MenopauseLogs,
                        CurrentTracker = currentTracker
                    };
                }

                if (trackerType == "all" || trackerType == "ovulation")
                {
                    var ovulationLogs = patientProfile.OvulationLogs?.Where(x => x.LogDate >= startDate && x.LogDate <= endDate).ToList() ?? new List<OvulationCycleLog>();
                    result = new
                    {
                        PregnancyLogs = result.PregnancyLogs,
                        PeriodLogs = result.PeriodLogs,
                        OvulationLogs = ovulationLogs,
                        MenopauseLogs = result.MenopauseLogs,
                        CurrentTracker = currentTracker
                    };
                }

                if (trackerType == "all" || trackerType == "menopause")
                {
                    var menopauseLogs = patientProfile.MenopauseLogs?.Where(x => x.LogDate >= startDate && x.LogDate <= endDate).ToList() ?? new List<MenopauseLogEntry>();
                    result = new
                    {
                        PregnancyLogs = result.PregnancyLogs,
                        PeriodLogs = result.PeriodLogs,
                        OvulationLogs = result.OvulationLogs,
                        MenopauseLogs = menopauseLogs,
                        CurrentTracker = currentTracker
                    };
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #region Helper Methods

        private List<BaseLogEntry> FilterAndCombineLogs(PatientProfileViewModel patientProfile, DateTime startDate, DateTime endDate, string trackerType)
        {
            var allLogs = new List<BaseLogEntry>();

            if (trackerType == "all" || trackerType == "pregnancy")
            {
                var pregnancyLogs = patientProfile.PregnancyLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new BaseLogEntry
                    {
                        LogDate = log.LogDate,
                        TrackerType = "pregnancy",
                        MoodsRating = log.MoodsRating,
                        SymptomsRating = log.SymptomsRating,
                        DerivedEnergyRating = log.EnergyRating ?? 0,
                        DerivedSleepRating = log.SleepRating ?? 0,
                        DerivedStressRating = log.StressRating ?? 0,
                        DerivedAppetiteRating = log.AppetiteRating ?? 0,
                        Notes = !string.IsNullOrEmpty(log.Reflection) ? log.Reflection :
                               !string.IsNullOrEmpty(log.SymptomsNotes) ? log.SymptomsNotes : ""
                    }) ?? new List<BaseLogEntry>();
                allLogs.AddRange(pregnancyLogs);
            }

            if (trackerType == "all" || trackerType == "period")
            {
                var periodLogs = patientProfile.PeriodLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new BaseLogEntry
                    {
                        LogDate = log.LogDate,
                        TrackerType = "period",
                        MoodsRating = log.MoodsRating,
                        SymptomsRating = log.SymptomsRating,
                        DerivedEnergyRating = 0,
                        DerivedSleepRating = 0,
                        DerivedStressRating = 0,
                        DerivedAppetiteRating = 0,
                        Notes = !string.IsNullOrEmpty(log.Notes) ? log.Notes : ""
                    }) ?? new List<BaseLogEntry>();
                allLogs.AddRange(periodLogs);
            }

            if (trackerType == "all" || trackerType == "ovulation")
            {
                var ovulationLogs = patientProfile.OvulationLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new BaseLogEntry
                    {
                        LogDate = log.LogDate,
                        TrackerType = "ovulation",
                        MoodsRating = log.MoodsRating,
                        SymptomsRating = log.SymptomsRating,
                        DerivedEnergyRating = log.EnergyRating ?? 0,
                        DerivedSleepRating = 0,
                        DerivedStressRating = 0,
                        DerivedAppetiteRating = 0,
                        Notes = !string.IsNullOrEmpty(log.Notes) ? log.Notes : ""
                    }) ?? new List<BaseLogEntry>();
                allLogs.AddRange(ovulationLogs);
            }

            return allLogs.OrderBy(log => log.LogDate).ToList();
        }

        private List<LogDataPoint> AggregateLogsByPeriod(List<BaseLogEntry> logs, DateTime startDate, DateTime endDate, string period)
        {
            var dataPoints = new List<LogDataPoint>();
            if (!logs.Any()) return dataPoints;

            IEnumerable<IGrouping<string, BaseLogEntry>> groupedLogs = period switch
            {
                "week" => logs.GroupBy(log => $"{log.LogDate:yyyy-MM-dd}"),
                "month" => logs.GroupBy(log => $"{log.LogDate:yyyy-MM}"),
                "year" => logs.GroupBy(log => log.LogDate.Year.ToString()),
                _ => logs.GroupBy(log => $"{log.LogDate:yyyy-MM-dd}")
            };

            foreach (var group in groupedLogs)
            {
                var logsInGroup = group.ToList();
                var moodRatings = logsInGroup.Where(l => l.MoodsRating.HasValue && l.MoodsRating.Value > 0).Select(l => l.MoodsRating.Value);
                var symptomsRatings = logsInGroup.Where(l => l.SymptomsRating.HasValue && l.SymptomsRating.Value > 0).Select(l => l.SymptomsRating.Value);

                var dataPoint = new LogDataPoint
                {
                    DateLabel = group.Key,
                    LogCount = logsInGroup.Count,
                    MoodRating = moodRatings.Any() ? moodRatings.Average() : 0,
                    SymptomsRating = symptomsRatings.Any() ? symptomsRatings.Average() : 0,
                    EnergyRating = logsInGroup.Any() ? logsInGroup.Average(l => l.DerivedEnergyRating) : 0,
                    SleepRating = logsInGroup.Any() ? logsInGroup.Average(l => l.DerivedSleepRating) : 0,
                    StressRating = logsInGroup.Any() ? logsInGroup.Average(l => l.DerivedStressRating) : 0,
                    AppetiteRating = logsInGroup.Any() ? logsInGroup.Average(l => l.DerivedAppetiteRating) : 0,
                    TrackerType = GetMostCommonTracker(logsInGroup),
                    Notes = string.Join(" | ", logsInGroup.Select(g => g.Notes).Where(n => !string.IsNullOrEmpty(n)).Take(3))
                };
                dataPoints.Add(dataPoint);
            }
            return dataPoints.OrderBy(d => d.DateLabel).ToList();
        }

        private string GetMostCommonTracker(List<BaseLogEntry> logs)
        {
            if (!logs.Any()) return "mixed";
            return logs.GroupBy(l => l.TrackerType).OrderByDescending(g => g.Count()).First().Key;
        }

        private TrendsSummary CalculateSummary(List<LogDataPoint> dataPoints)
        {
            if (!dataPoints.Any())
            {
                return new TrendsSummary
                {
                    AvgMood = 0,
                    AvgEnergy = 0,
                    AvgSleep = 0,
                    AvgSymptoms = 0,
                    AvgStress = 0,
                    AvgAppetite = 0,
                    TotalLogs = 0,
                    TrackerDistribution = new Dictionary<string, int>(),
                    MostCommonSymptom = "No data",
                    PeakMoodDay = "No data"
                };
            }

            var validDataPoints = dataPoints.Where(dp => dp.LogCount > 0).ToList();
            if (!validDataPoints.Any())
            {
                return new TrendsSummary
                {
                    AvgMood = 0,
                    AvgEnergy = 0,
                    AvgSleep = 0,
                    AvgSymptoms = 0,
                    AvgStress = 0,
                    AvgAppetite = 0,
                    TotalLogs = dataPoints.Sum(dp => dp.LogCount),
                    TrackerDistribution = new Dictionary<string, int>(),
                    MostCommonSymptom = "No data",
                    PeakMoodDay = "No data"
                };
            }

            return new TrendsSummary
            {
                AvgMood = validDataPoints.Average(dp => dp.MoodRating),
                AvgEnergy = validDataPoints.Average(dp => dp.EnergyRating),
                AvgSleep = validDataPoints.Average(dp => dp.SleepRating),
                AvgSymptoms = validDataPoints.Average(dp => dp.SymptomsRating),
                AvgStress = validDataPoints.Average(dp => dp.StressRating),
                AvgAppetite = validDataPoints.Average(dp => dp.AppetiteRating),
                TotalLogs = dataPoints.Sum(dp => dp.LogCount),
                TrackerDistribution = dataPoints.GroupBy(dp => dp.TrackerType).ToDictionary(g => g.Key, g => g.Sum(dp => dp.LogCount)),
                PeakMoodDay = validDataPoints.OrderByDescending(dp => dp.MoodRating).FirstOrDefault()?.DateLabel ?? "No data"
            };
        }

        private List<DetailedLogEntry> FilterAndCombineLogsDetailed(PatientProfileViewModel patientProfile, DateTime startDate, DateTime endDate, string trackerType)
        {
            var allLogs = new List<DetailedLogEntry>();

            if (trackerType == "all" || trackerType == "pregnancy")
            {
                var pregnancyLogs = patientProfile.PregnancyLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new DetailedLogEntry
                    {
                        EntryId = log.EntryId,
                        LogDate = log.LogDate,
                        TrackerType = "pregnancy",
                        MoodRating = log.MoodsRating ?? 0,
                        EnergyRating = log.EnergyRating ?? 0,
                        SleepRating = log.SleepRating ?? 0,
                        SymptomsRating = log.SymptomsRating ?? 0,
                        StressRating = log.StressRating ?? 0,
                        AppetiteRating = log.AppetiteRating ?? 0,
                        Notes = !string.IsNullOrEmpty(log.Reflection) ? log.Reflection :
                               !string.IsNullOrEmpty(log.SymptomsNotes) ? log.SymptomsNotes : "",
                        Week = log.Week,
                        Symptoms = log.Symptoms ?? new List<string>(),
                        Moods = log.Moods ?? new List<string>()
                    }) ?? new List<DetailedLogEntry>();
                allLogs.AddRange(pregnancyLogs);
            }

            if (trackerType == "all" || trackerType == "period")
            {
                var periodLogs = patientProfile.PeriodLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new DetailedLogEntry
                    {
                        EntryId = log.EntryId,
                        LogDate = log.LogDate,
                        TrackerType = "period",
                        MoodRating = log.MoodsRating ?? 0,
                        SymptomsRating = log.SymptomsRating ?? 0,
                        Notes = !string.IsNullOrEmpty(log.Notes) ? log.Notes : "",
                        PhaseName = log.PhaseName,
                        HadBleeding = log.HadBleeding,
                        Symptoms = log.Symptoms ?? new List<string>(),
                        Moods = log.Moods ?? new List<string>()
                    }) ?? new List<DetailedLogEntry>();
                allLogs.AddRange(periodLogs);
            }

            if (trackerType == "all" || trackerType == "ovulation")
            {
                var ovulationLogs = patientProfile.OvulationLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new DetailedLogEntry
                    {
                        EntryId = log.EntryId,
                        LogDate = log.LogDate,
                        TrackerType = "ovulation",
                        MoodRating = log.MoodsRating ?? 0,
                        EnergyRating = log.EnergyRating ?? 0,
                        SymptomsRating = log.SymptomsRating ?? 0,
                        Notes = !string.IsNullOrEmpty(log.Notes) ? log.Notes : "",
                        PhaseName = log.PhaseName,
                        Symptoms = log.Symptoms ?? new List<string>(),
                        Moods = log.Moods ?? new List<string>()
                    }) ?? new List<DetailedLogEntry>();
                allLogs.AddRange(ovulationLogs);
            }

            if (trackerType == "all" || trackerType == "menopause")
            {
                var menopauseLogs = patientProfile.MenopauseLogs?
                    .Where(log => log.LogDate.Date >= startDate.Date && log.LogDate.Date <= endDate.Date)
                    .Select(log => new DetailedLogEntry
                    {
                        EntryId = log.EntryId,
                        LogDate = log.LogDate,
                        TrackerType = "menopause",
                        Notes = !string.IsNullOrEmpty(log.Notes) ? log.Notes : "",
                        PhaseName = log.PhaseName,
                        Symptoms = log.Symptoms ?? new List<string>(),
                        Moods = log.Moods ?? new List<string>()
                    }) ?? new List<DetailedLogEntry>();
                allLogs.AddRange(menopauseLogs);
            }

            return allLogs.OrderBy(log => log.LogDate).ToList();
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetFilteredTrackerLogs([FromQuery] string trackerType, [FromQuery] string patientUserId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                // Get the patient profile data
                var dto = new DoctorPatientRequest
                {
                    DoctorUserId = _session.GetSecureApiRequestDto().UserId,
                    PatientUserId = patientUserId,
                    DoctorEmail = "",
                    PatientEmail = ""
                };

                var data = await _doctorsApi.GetDoctorsPatientProfile(dto);

                if (data == null)
                    return PartialView("_ErrorPartial", "Failed to load patient data");

                // Create the view model
                var viewModel = new PatientProfileViewModel
                {
                    UserFullProfile = data.UserFullProfile,
                    CurrentTracker = trackerType,
                    FilterStartDate = startDate,
                    FilterEndDate = endDate
                };

                // Filter logs based on date range
                var allPregnancyLogs = data.PregnancyLogs ?? new List<PregnancyLogEntryItem>();
                var allPeriodLogs = data.PeriodLogs ?? new List<PeriodLogEntry>();
                var allOvulationLogs = data.OvulationLogs ?? new List<OvulationCycleLog>();
                var allMenopauseLogs = data.MenopauseLogs ?? new List<MenopauseLogEntry>();

                // Apply date filtering
                viewModel.PregnancyLogs = allPregnancyLogs
                    .Where(x => x.LogDate >= startDate && x.LogDate <= endDate)
                    .ToList();

                viewModel.PeriodLogs = allPeriodLogs
                    .Where(x => x.LogDate >= startDate && x.LogDate <= endDate)
                    .ToList();

                viewModel.OvulationLogs = allOvulationLogs
                    .Where(x => x.LogDate >= startDate && x.LogDate <= endDate)
                    .ToList();

                viewModel.MenopauseLogs = allMenopauseLogs
                    .Where(x => x.LogDate >= startDate && x.LogDate <= endDate)
                    .ToList();

                // Create the appropriate view model for the partial
                switch (trackerType.ToLower())
                {
                    case "pregnancy":
                        var pregnancyViewModel = new PregnancyLogsViewModel
                        {
                            PatientUserId = patientUserId,
                            Logs = viewModel.PregnancyLogs
                        };
                        return PartialView("_PregnancyLogs", pregnancyViewModel);

                    case "periodtracker":
                        var periodViewModel = new PeriodLogsViewModel
                        {
                            PatientUserId = patientUserId,
                            Logs = viewModel.PeriodLogs
                        };
                        return PartialView("_PeriodLogs", periodViewModel);

                    case "ovulation":
                        var ovulationViewModel = new OvulationLogsViewModel
                        {
                            PatientUserId = patientUserId,
                            Logs = viewModel.OvulationLogs
                        };
                        return PartialView("_OvulationLogs", ovulationViewModel);

                    case "menopausetracker":
                        var menopauseViewModel = new MenopauseLogsViewModel
                        {
                            PatientUserId = patientUserId,
                            Logs = viewModel.MenopauseLogs
                        };
                        return PartialView("_MenopauseLogs", menopauseViewModel);

                    default:
                        return PartialView("_ErrorPartial", $"Unknown tracker type: {trackerType}");
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorPartial", "An error occurred while loading logs");
            }
        }
    }
}
