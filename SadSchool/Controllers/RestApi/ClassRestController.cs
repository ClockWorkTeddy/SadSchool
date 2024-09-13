// <copyright file="ClassRestController.cs" company="ClockWorkTeddy">
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
    /// REST API for <see cref="Class"/> entities.
    /// </summary>
    [ApiController]
    [Route("api/rest/classes")]
    public class ClassRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRestController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache instance.</param>
        public ClassRestController(
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
        /// Gets all classes.
        /// </summary>
        /// <returns>Action results.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var classes = this.context.Classes.ToList();

                return this.Ok(JsonSerializer.Serialize(classes));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Gets a class by id.
        /// </summary>
        /// <param name="id">Target class id.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var @class = this.context.Classes.Find(id);

                return this.Ok(JsonSerializer.Serialize(@class));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Adds new class.
        /// </summary>
        /// <param name="class">Created class data.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Class @class)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.Classes.Add(@class);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Updates class by id.
        /// </summary>
        /// <param name="id">Updated class id.</param>
        /// <param name="class">Updated class data.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Class @class)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var editedClass = this.context.Classes.Find(id);

                if (editedClass != null)
                {
                    editedClass.Name = @class.Name;
                    editedClass.TeacherId = @class.TeacherId;
                    editedClass.LeaderId = @class.LeaderId;

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

        /// <summary>
        /// Deletes class by id.
        /// </summary>
        /// <param name="id">Deleted class id.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var @class = this.context.Classes.Find(id);

                if (@class != null)
                {
                    this.context.Classes.Remove(@class);
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
