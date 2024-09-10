// <copyright file="StudentRestController.cs" company="ClockWorkTeddy">
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
    /// The controller serves student processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/students")]
    public class StudentRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRestController"/> class.
        /// </summary>
        /// <param name="context">DB context instance.</param>
        /// <param name="configuration">Configuration object instance.</param>
        /// <param name="cacheService">Cache service instance.</param>
        public StudentRestController(
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
        /// The method gets collection of students from DB.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var students = this.context.Students.ToList();

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
        /// <returns>Action result.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var student = this.cacheService.GetObject<Student>(id);

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
        /// <returns>Action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                this.context.Students.Add(student);
                this.context.SaveChanges();

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
        /// <returns>Action result.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Student student)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var studentToUpdate = this.context.Students.Find(id);

                if (studentToUpdate == null)
                {
                    return this.NotFound();
                }

                studentToUpdate.FirstName = student.FirstName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.DateOfBirth = student.DateOfBirth;
                studentToUpdate.ClassId = student.ClassId;
                studentToUpdate.Sex = student.Sex;

                this.context.Students.Update(studentToUpdate);
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
