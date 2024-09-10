
namespace SadSchool.Controllers.RestApi
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;

    [ApiController]
    [Route("api/rest/starttimes")]
    public class StartTimeRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly SadSchoolContext context;

        public StartTimeRestController(
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
                var startTime = this.context.StartTimes.ToList();

                return this.Ok(JsonSerializer.Serialize(startTime));
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
                var startTime = this.cacheService.GetObject<StartTime>(id);

                return this.Ok(JsonSerializer.Serialize(startTime));
            }
            else
            {
                return this.Unauthorized();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] StartTime startTime)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                this.context.StartTimes.Add(startTime);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StartTime startTime)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == null || apiKey == this.apiKey)
            {
                var startTimeToUpdate = this.context.StartTimes.Find(id);
                if (startTimeToUpdate == null)
                {
                    return this.NotFound();
                }

                startTimeToUpdate.Value = startTime.Value;

                this.context.StartTimes.Update(startTimeToUpdate);
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
