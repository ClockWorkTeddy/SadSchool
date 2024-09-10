// <copyright file="SubjectRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves subject processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/subjects")]
    public class SubjectRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectRestController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public SubjectRestController(
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
        /// The method gets collection of subjects from DB.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var subjects = this.context.Subjects.ToList();

                return this.Ok(JsonSerializer.Serialize(subjects));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets subject by id.
        /// </summary>
        /// <param name="id">Desirable subject id.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var subject = this.cacheService.GetObject<Subject>(id);

                return this.Ok(JsonSerializer.Serialize(subject));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method adds a new subject to DB.
        /// </summary>
        /// <param name="subject">Created subject data.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Subject subject)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.Subjects.Add(subject);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method updates subject data.
        /// </summary>
        /// <param name="subjectId">Edited subject id.</param>
        /// <param name="subject">Edited subject data.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{subjectId}")]
        public IActionResult Put(int subjectId, [FromBody] Subject subject)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var subjectToUpdate = this.context.Subjects.Find(subjectId);

                if (subjectToUpdate != null)
                {
                    subjectToUpdate.Name = subject.Name;

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
