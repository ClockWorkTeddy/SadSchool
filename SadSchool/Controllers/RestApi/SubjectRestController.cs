// <copyright file="SubjectRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves subject processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/subjects")]
    public class SubjectRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly ISubjectRepository subjectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectRestController"/> class.
        /// </summary>
        /// <param name="subjectRepository">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        public SubjectRestController(
            ISubjectRepository subjectRepository,
            IConfiguration configuration)
        {
            this.subjectRepository = subjectRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// The method gets collection of subjects from DB.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var subjects = await this.subjectRepository.GetAllEntitiesAsync<Subject>();

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var subject = await this.subjectRepository.GetEntityByIdAsync<Subject>(id);

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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Subject subject, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                await this.subjectRepository.AddEntityAsync(subject);
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
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{subjectId}")]
        public async Task<IActionResult> Put(int subjectId, [FromBody] Subject subject, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var subjectToUpdate = await this.subjectRepository.GetEntityByIdAsync<Subject>(subjectId);

                if (subjectToUpdate != null)
                {
                    subjectToUpdate.Name = subject.Name;

                    await this.subjectRepository.UpdateEntityAsync(subjectToUpdate);

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
        /// Deletes a subject with the specified identifier.
        /// </summary>
        /// <param name="subjectId">The unique identifier of the subject to delete.</param>
        /// <param name="apiKey">The API key provided in the request header for authentication.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see cref="OkResult"/> if
        /// the subject is successfully deleted, or  <see cref="UnauthorizedResult"/> if the provided API key is
        /// invalid.</returns>
        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> Delete(int subjectId, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                await this.subjectRepository.DeleteEntityAsync<Subject>(subjectId);
                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
