namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    [ApiController]
    [Route("api/rest/students")]
    public class StudentRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

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
