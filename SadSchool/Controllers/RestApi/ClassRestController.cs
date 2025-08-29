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
    public class ClassRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly IClassRepository classRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRestController"/> class.
        /// </summary>
        /// <param name="classRepository">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        public ClassRestController(
            IClassRepository classRepository,
            IConfiguration configuration)
        {
            this.classRepository = classRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// Gets all classes.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action results.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var classes = await this.classRepository.GetAllEntitiesAsync<Class>();

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var @class = await this.classRepository.GetEntityByIdAsync<Class>(id);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Class @class, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.classRepository.AddEntityAsync(@class);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Class @class, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                @class.Id = id;

                if (await this.classRepository.UpdateEntityAsync(@class))
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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                if (await this.classRepository.DeleteEntityAsync<Class>(id))
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
