// <copyright file="StudentRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts.Repositories;
    using SadSchool.Models.SqlServer;

    /// <summary>
    /// The controller serves student processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/students")]
    public class StudentRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly IDerivedRepositories derivedRepositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRestController"/> class.
        /// </summary>
        /// <param name="derivedRepositories">Derived repo instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        public StudentRestController(
            IDerivedRepositories derivedRepositories,
            IConfiguration configuration)
        {
            this.derivedRepositories = derivedRepositories;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// The method gets collection of students from DB.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var students = await this.derivedRepositories.StudentRepository.GetAllEntitiesAsync<Student>();

                return this.Ok(JsonSerializer.Serialize(students));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets student by id from DB.
        /// </summary>
        /// <param name="id">Target student's id.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == null || apiKey == this.apiKey)
            {
                var student = await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Student>(id);

                return this.Ok(JsonSerializer.Serialize(student));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method adds a new student to DB.
        /// </summary>
        /// <param name="student">Created student's name.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == this.apiKey)
            {
                await this.derivedRepositories.StudentRepository.AddEntityAsync(student);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method updates student in DB.
        /// </summary>
        /// <param name="id">Updated student's id.</param>
        /// <param name="student">Updated student's data.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Student student, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == this.apiKey)
            {
                var studentToUpdate = await this.derivedRepositories.StudentRepository.GetEntityByIdAsync<Student>(id);

                if (studentToUpdate == null)
                {
                    return this.NotFound();
                }

                studentToUpdate.FirstName = student.FirstName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.DateOfBirth = student.DateOfBirth;
                studentToUpdate.ClassId = student.ClassId;
                studentToUpdate.Sex = student.Sex;

                await this.derivedRepositories.StudentRepository.UpdateEntityAsync(studentToUpdate);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Deletes a student entity with the specified identifier.
        /// </summary>
        /// <remarks>The API key must match the expected value for the operation to proceed. If the API
        /// key is invalid, the request will be rejected with an unauthorized response.</remarks>
        /// <param name="id">The unique identifier of the student entity to delete.</param>
        /// <param name="apiKey">The API key provided in the request header for authentication.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see cref="OkResult"/> if
        /// the deletion is successful, or <see cref="UnauthorizedResult"/> if the provided API key is invalid.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (apiKey == this.apiKey)
            {
                await this.derivedRepositories.StudentRepository.DeleteEntityAsync<Student>(id);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
