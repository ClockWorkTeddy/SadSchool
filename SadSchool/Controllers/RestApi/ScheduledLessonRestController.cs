// <copyright file="ScheduledLessonRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// The controller serves scheduled lesson processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/scheduledlessons")]
    public class ScheduledLessonRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly IScheduledLessonRepository scheduledLessonRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledLessonRestController"/> class.
        /// </summary>
        /// <param name="scheduledLessonRepository">Scheduled lesson repo instance.</param>
        /// <param name="configuration">Configuration of the app.</param>
        public ScheduledLessonRestController(
            IScheduledLessonRepository scheduledLessonRepository,
            IConfiguration configuration)
        {
            this.scheduledLessonRepository = scheduledLessonRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// The method gets collection of scheduled lessons from DB.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var scheduledLessons = await this.scheduledLessonRepository.GetAllEntitiesAsync<ScheduledLesson>();

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var scheduledLesson = await this.scheduledLessonRepository.GetEntityByIdAsync<ScheduledLesson>(id);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScheduledLesson scheduledLesson, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                await this.scheduledLessonRepository.AddEntityAsync(scheduledLesson);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ScheduledLesson scheduledLesson, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == this.apiKey)
            {
                var scheduledLessonForUpdate = await this.scheduledLessonRepository.GetEntityByIdAsync<ScheduledLesson>(id);

                if (scheduledLessonForUpdate == null)
                {
                    return this.NotFound();
                }

                scheduledLessonForUpdate.ClassId = scheduledLesson.ClassId;
                scheduledLessonForUpdate.Day = scheduledLesson.Day;
                scheduledLessonForUpdate.StartTimeId = scheduledLesson.StartTimeId;
                scheduledLessonForUpdate.SubjectId = scheduledLesson.SubjectId;
                scheduledLessonForUpdate.TeacherId = scheduledLesson.TeacherId;

                await this.scheduledLessonRepository.UpdateEntityAsync(scheduledLessonForUpdate);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == this.apiKey)
            {
                await this.scheduledLessonRepository.DeleteEntityAsync<ScheduledLesson>(id);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
