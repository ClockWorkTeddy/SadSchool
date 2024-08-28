namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    [ApiController]
    [Route("api/rest/subjects")]
    public class SubjectRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

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
