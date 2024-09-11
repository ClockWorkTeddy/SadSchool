// <copyright file="LessonRestController.cs" company="ClockWorkTeddy">
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
    /// REST API for <see cref="Lesson"/> entities.
    /// </summary>
    [ApiController]
    [Route("api/rest/lessons")]
    public class LessonRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRestController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache service instanse.</param>
        public LessonRestController(
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
        /// Gets all lessons.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var lessons = this.context.Lessons.ToList();

                return this.Ok(JsonSerializer.Serialize(lessons));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Gets lesson by id.
        /// </summary>
        /// <param name="id">Target lesson id.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var lesson = this.context.Lessons.Find(id);

                return this.Ok(JsonSerializer.Serialize(lesson));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Adds new lesson.
        /// </summary>
        /// <param name="lesson">Created lesson data.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Lesson lesson)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.Lessons.Add(lesson);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Updates lesson.
        /// </summary>
        /// <param name="id">Updated lesson's id.</param>
        /// <param name="lesson">Updated lesson's data.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Lesson lesson)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var lessonToUpdate = this.context.Lessons.Find(id);

                if (lessonToUpdate == null)
                {
                    return this.NotFound();
                }

                lessonToUpdate.Date = lesson.Date;
                lessonToUpdate.ScheduledLessonId = lesson.ScheduledLessonId;

                this.context.Lessons.Update(lessonToUpdate);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Deletes lesson.
        /// </summary>
        /// <param name="id">Deleted lesson's id.</param>
        /// <returns>Action data.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var lesson = this.context.Lessons.Find(id);

                if (lesson != null)
                {
                    this.context.Lessons.Remove(lesson);
                    this.context.SaveChanges();

                    return this.Ok();
                }
                else
                {
                    return this.NotFound();
                }
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
