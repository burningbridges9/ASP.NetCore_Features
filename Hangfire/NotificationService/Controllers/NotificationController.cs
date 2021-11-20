using Microsoft.AspNetCore.Mvc;
using System;

namespace Hangfire.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        [Route("fire-and-forget")]
        public IActionResult FireAndForget(string username)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"{username} 5 minutes to go."));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("delayed")]
        public IActionResult Delayed(string username)
        {
            string jobId = BackgroundJob.Schedule(() =>
                Console.WriteLine($"{username} 5 minutes to go."), TimeSpan.FromSeconds(60));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Wake up, it's 7am"), Cron.Daily);

            return Ok();
        }

        [HttpPost]
        [Route("continuations")]
        public IActionResult Continuations(string username)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"Prepare dinner for {username}"));

            BackgroundJob.ContinueJobWith(jobId, () =>
                Console.WriteLine($"Request payment from {username}."));

            return Ok();
        }
    }
}
