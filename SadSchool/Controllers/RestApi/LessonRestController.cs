// <copyright file="LessonRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// REST API for <see cref="Lesson"/> entities.
    /// </summary>
    [ApiController]
    [Route("api/rest/lessons")]
    public class LessonRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly ILessonRepository lessonRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRestController"/> class.
        /// </summary>
        /// <param name="lessonRepository">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        public LessonRestController(
            ILessonRepository lessonRepository,
            IConfiguration configuration)
        {
            this.lessonRepository = lessonRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// Gets all lessons.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var lessons = await this.lessonRepository.GetAllEntitiesAsync<Lesson>();

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var lesson = await this.lessonRepository.GetEntityByIdAsync<Lesson>(id);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lesson lesson, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.lessonRepository.AddEntityAsync(lesson);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Lesson lesson, [FromHeader(Name = "api-key")] string apiKey)
        {
            lesson.Id = id;

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.lessonRepository.UpdateEntityAsync(lesson))
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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action data.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.lessonRepository.DeleteEntityAsync<Lesson>(id))
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
