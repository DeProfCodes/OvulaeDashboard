using Microsoft.AspNetCore.Mvc;
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
using OvulaeShared.Models.WebApi;
using OvulaeShared.Services.APIs.Affiliates;
using OvulaeShared.Services.APIs.Doctors;
using OvulaeShared.Services.APIs.ModuleServices;
using OvulaeShared.Services.APIs.Transactions;
using OvulaeShared.Services.APIs.Users;
using OvulaeShared.ViewModel.Affiliates;
using OvulaeShared.ViewModel.Doctor;
using OvulaeShared.ViewModel.User;

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
    }
}
