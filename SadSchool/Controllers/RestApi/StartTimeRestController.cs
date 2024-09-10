// <copyright file="StartTimeRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves start time processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/starttimes")]
    public class StartTimeRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartTimeRestController"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="configuration">Configuration object.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public StartTimeRestController(
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
        /// The method gets collection of start times from DB.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTime = this.context.StartTimes.ToList();

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
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTime = this.cacheService.GetObject<StartTime>(id);

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
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] StartTime startTime)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.StartTimes.Add(startTime);
                this.context.SaveChanges();

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
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StartTime startTime)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTimeToUpdate = this.context.StartTimes.Find(id);
                if (startTimeToUpdate == null)
                {
                    return this.NotFound();
                }

                startTimeToUpdate.Value = startTime.Value;

                this.context.StartTimes.Update(startTimeToUpdate);
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
