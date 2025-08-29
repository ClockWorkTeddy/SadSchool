// <copyright file="StartTimeRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// The controller serves start time processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/starttimes")]
    public class StartTimeRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly IStartTimeRepository startTimeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTimeRestController"/> class.
        /// </summary>
        /// <param name="startTimeRepository">Start time repo instanse.</param>
        /// <param name="configuration">Configuration object.</param>
        public StartTimeRestController(
            IStartTimeRepository startTimeRepository,
            IConfiguration configuration)
        {
            this.startTimeRepository = startTimeRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// The method gets collection of start times from DB.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTime = await this.startTimeRepository.GetAllEntitiesAsync<StartTime>();

                return this.Ok(JsonSerializer.Serialize(startTime));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets start time by id.
        /// </summary>
        /// <param name="id">Target instanse's id.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTime = await this.startTimeRepository.GetEntityByIdAsync<StartTime>(id);

                return this.Ok(JsonSerializer.Serialize(startTime));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method adds a new start time to DB.
        /// </summary>
        /// <param name="startTime">Start time object's data for the creation.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StartTime startTime, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                await this.startTimeRepository.AddEntityAsync(startTime);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method updates start time data.
        /// </summary>
        /// <param name="id">Target start time's id.</param>
        /// <param name="startTime">Start time object's date for update.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StartTime startTime, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTimeToUpdate = await this.startTimeRepository.GetEntityByIdAsync<StartTime>(id);
                if (startTimeToUpdate == null)
                {
                    return this.NotFound();
                }

                startTimeToUpdate.Value = startTime.Value;

                await this.startTimeRepository.UpdateEntityAsync(startTimeToUpdate);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method deletes start time by id.
        /// </summary>
        /// <param name="id">Start Time Id.</param>
        /// <param name="apiKey">The Api Key.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                await this.startTimeRepository.DeleteEntityAsync<StartTime>(id);
                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
