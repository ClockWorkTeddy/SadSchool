// <copyright file="TeachersRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves teachers processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/teachers")]
    public class TeachersRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly SadSchoolContext context;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachersRestController"/> class.
        /// </summary>
        /// <param name="context">Application DB context.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public TeachersRestController(
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
        /// The method gets collection of teachers from DB.
        /// </summary>
        /// <returns>The list of <see cref="Teacher"/>.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teachers = this.context.Teachers.ToList();

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
        /// <param name="teacherId">Desirable tescher's id.</param>
        /// <returns>The particular <see cref="Teacher"/>.</returns>
        [HttpGet("{teacherId}")]
        public IActionResult Get(int teacherId)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teacher = this.cacheService.GetObject<Teacher>(teacherId);

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
        /// <returns>The result of the operation.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Teacher teacher)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                this.context.Teachers.Add(teacher);
                this.context.SaveChanges();

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
        /// <returns>The result of the operation.</returns>
        [HttpPut("{teacherId}")]
        public IActionResult Put(int teacherId, [FromBody] Teacher updateTeacher)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var teacher = this.context.Teachers.Find(teacherId);

                if (teacher == null)
                {
                    return this.NotFound();
                }

                teacher.FirstName = updateTeacher.FirstName;
                teacher.LastName = updateTeacher.LastName;
                teacher.DateOfBirth = updateTeacher.DateOfBirth;
                teacher.Grade = updateTeacher.Grade;

                this.context.Teachers.Update(teacher);
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
