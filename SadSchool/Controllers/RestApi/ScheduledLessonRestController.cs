// <copyright file="ScheduledLessonRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// The controller serves scheduled lesson processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/scheduledlessons")]
    public class ScheduledLessonRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledLessonRestController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="configuration">Configuration of the app.</param>
        /// <param name="cacheService">Cache service object.</param>
        public ScheduledLessonRestController(
            SadSchoolContext context,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            this.context = context;
            this.configuration = configuration;
            this.apiKey = this.configuration["api-key"];
            this.cacheService = cacheService;
        }

        /// <summary>
        /// The method gets collection of scheduled lessons from DB.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var scheduledLessons = this.context.ScheduledLessons.ToList();

                return this.Ok(JsonSerializer.Serialize(scheduledLessons));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets scheduled lesson by id.
        /// </summary>
        /// <param name="id">Desirable scheduled lesson.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var scheduledLesson = this.cacheService.GetObject<ScheduledLesson>(id);

                return this.Ok(JsonSerializer.Serialize(scheduledLesson));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method adds a new scheduled lesson to DB.
        /// </summary>
        /// <param name="scheduledLesson">Scheduled lesson for the creation.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] ScheduledLesson scheduledLesson)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.ScheduledLessons.Add(scheduledLesson);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method updates scheduled lesson in DB.
        /// </summary>
        /// <param name="id">Selected lesson's id.</param>
        /// <param name="scheduledLesson">Scheduled lesson's data for update.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ScheduledLesson scheduledLesson)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var scheduledLessonForUpdate = this.context.ScheduledLessons.Find(id);

                if (scheduledLessonForUpdate == null)
                {
                    return this.NotFound();
                }

                scheduledLessonForUpdate.ClassId = scheduledLesson.ClassId;
                scheduledLessonForUpdate.Day = scheduledLesson.Day;
                scheduledLessonForUpdate.StartTimeId = scheduledLesson.StartTimeId;
                scheduledLessonForUpdate.SubjectId = scheduledLesson.SubjectId;
                scheduledLessonForUpdate.TeacherId = scheduledLesson.TeacherId;

                this.context.ScheduledLessons.Update(scheduledLessonForUpdate);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method deletes scheduled lesson from DB.
        /// </summary>
        /// <param name="id">Deleted lesson's id.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var scheduledLesson = this.context.ScheduledLessons.Find(id);

                if (scheduledLesson == null)
                {
                    return this.NotFound();
                }

                this.context.ScheduledLessons.Remove(scheduledLesson);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
