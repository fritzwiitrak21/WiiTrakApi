using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.Services.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundJobController : ControllerBase
    {
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public BackgroundJobController(IBackgroundJobService backgroundJobService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobService = backgroundJobService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }


        [HttpGet]
        public ActionResult CreateReccuringJob()
        {
            // cron expression for every 8 hours:  0 0 0/8 ? * * *
            _recurringJobManager.AddOrUpdate("resetCartData", () => _backgroundJobService.ResetCartData(), Cron.Hourly);
            return Ok();
        }

    }
}
