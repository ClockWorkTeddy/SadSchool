// <copyright file="LessonRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
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
        private readonly ILessonRepository lessonRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRestController"/> class.
        /// </summary>
        /// <param name="lessonRepository">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache service instanse.</param>
        public LessonRestController(
            ILessonRepository lessonRepository,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            this.lessonRepository = lessonRepository;
            this.configuration = configuration;
            this.apiKey = this.configuration["api-key"];
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets all lessons.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var lessons = await this.lessonRepository.GetAllLessonsAsync();

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
        public async Task<IActionResult> Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var lesson = await this.lessonRepository.GetLessonByIdAsync(id);

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
        public async Task<IActionResult> Post([FromBody] Lesson lesson)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.lessonRepository.AddLessonAsync(lesson);

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
        public async Task<IActionResult> Put(int id, [FromBody] Lesson lesson)
        {
            lesson.Id = id;

            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.lessonRepository.UpdateLessonAsync(lesson))
                {
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

        /// <summary>
        /// Deletes lesson.
        /// </summary>
        /// <param name="id">Deleted lesson's id.</param>
        /// <returns>Action data.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.lessonRepository.DeleteLessonAsync(id))
                {
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
