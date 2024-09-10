using Microsoft.AspNetCore.Mvc;
using SadSchool.Contracts;
using SadSchool.DbContexts;
using SadSchool.Models.SqlServer;
using System.Text.Json;

namespace SadSchool.Controllers.RestApi
{
    [ApiController]
    [Route("api/rest/scheduledlessons")]

    public class ScheduledLessonRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

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
