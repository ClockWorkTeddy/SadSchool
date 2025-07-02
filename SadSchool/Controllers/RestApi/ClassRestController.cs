// <copyright file="ClassRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.Contracts.Repositories;
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
        private readonly IClassRepository classRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRestController"/> class.
        /// </summary>
        /// <param name="classRepository">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache instance.</param>
        public ClassRestController(
            IClassRepository classRepository,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            this.classRepository = classRepository;
            this.configuration = configuration;
            this.apiKey = this.configuration["api-key"];
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Gets all classes.
        /// </summary>
        /// <returns>Action results.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var classes = await this.classRepository.GetAllClassesAsync();

                return this.Ok(classes);
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
        public async Task<IActionResult> Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var @class = await this.classRepository.GetClassByIdAsync(id);

                return this.Ok(@class);
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
        public async Task<IActionResult> Post([FromBody] Class @class)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.classRepository.AddClassAsync(@class);

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
        public async Task<IActionResult> Put(int id, [FromBody] Class @class)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.classRepository.UpdateClassAsync(@class))
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
        /// Deletes class by id.
        /// </summary>
        /// <param name="id">Deleted class id.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.classRepository.DeleteClassByIdAsync(id))
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
