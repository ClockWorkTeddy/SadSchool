// <copyright file="TeachersRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves teachers processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/teachers")]
    public class TeachersRestController : ControllerBase
    {
        private readonly string? apiKey;
        private readonly ITeacherRepository teacherRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachersRestController"/> class.
        /// </summary>
        /// <param name="teacherRepository">Teachers repo instance.</param>
        /// <param name="configuration">Application configuration.</param>
        public TeachersRestController(
            ITeacherRepository teacherRepository,
            IConfiguration configuration)
        {
            this.teacherRepository = teacherRepository;
            this.apiKey = configuration["api-key"];
        }

        /// <summary>
        /// The method gets collection of teachers from DB.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The list of <see cref="Teacher"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teachers = await this.teacherRepository.GetAllEntitiesAsync<Teacher>();

                return this.Ok(JsonSerializer.Serialize(teachers));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets teacher by id from DB.
        /// </summary>
        /// <param name="teacherId">Desirable teacher's id.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The particular <see cref="Teacher"/>.</returns>
        [HttpGet("{teacherId}")]
        public async Task<IActionResult> Get(int teacherId, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teacher = await this.teacherRepository.GetEntityByIdAsync<Teacher>(teacherId);

                return teacher != null ? this.Ok(teacher) : this.NotFound();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method adds a new teacher to DB.
        /// </summary>
        /// <param name="teacher">The teacher to add.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Teacher teacher, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.teacherRepository.AddEntityAsync(teacher);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method updates a teacher in DB.
        /// </summary>
        /// <param name="teacherId">Desirable teacher's id.</param>
        /// <param name="updateTeacher">New teacher data.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The result of the operation.</returns>
        [HttpPut("{teacherId}")]
        public async Task<IActionResult> Put(int teacherId, [FromBody] Teacher updateTeacher, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teacher = await this.teacherRepository.GetEntityByIdAsync<Teacher>(teacherId);

                if (teacher == null)
                {
                    return this.NotFound();
                }

                teacher.FirstName = updateTeacher.FirstName;
                teacher.LastName = updateTeacher.LastName;
                teacher.DateOfBirth = updateTeacher.DateOfBirth;
                teacher.Grade = updateTeacher.Grade;

                await this.teacherRepository.UpdateEntityAsync(teacher);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method deletes a teacher from DB.
        /// </summary>
        /// <param name="teacherId">The unique identifier of the teacher to delete.</param>
        /// <param name="apiKey">The API key provided in the request header for authentication.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{teacherId}")]
        public async Task<IActionResult> Delete(int teacherId, [FromHeader(Name = "api-key")] string apiKey)
        {
            if (this.apiKey == null || apiKey == this.apiKey)
            {
                await this.teacherRepository.DeleteEntityAsync<Teacher>(teacherId);

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
