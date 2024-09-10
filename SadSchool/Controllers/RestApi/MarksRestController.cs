// <copyright file="MarksRestController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.Mongo;

    /// <summary>
    /// The controller serves marks processing.
    /// </summary>
    [ApiController]
    [Route("api/rest/marks")]
    public class MarksRestController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly MongoContext context;
        private readonly IConfiguration configuration;
        private readonly IMarksAnalyticsService marksAnalyticsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksRestController"/> class.
        /// </summary>
        /// <param name="context">Application DB context.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="markAnalyticSevice">MarkAnalytic service instance.</param>
        public MarksRestController(
            MongoContext context,
            IConfiguration configuration,
            IMarksAnalyticsService markAnalyticSevice)
        {
            this.context = context;
            this.configuration = configuration;
            this.apiKey = this.configuration["api-key"];
            this.marksAnalyticsService = markAnalyticSevice;
        }

        /// <summary>
        /// The method gets collection of marks from DB.
        /// </summary>
        /// <returns>The list of <see cref="Mark"/>.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (this.apiKey == null || apiKey == this.apiKey)
            {
                var marks = this.context.Marks.ToList();

                return this.Ok(marks);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets a particular mark by id.
        /// </summary>
        /// <param name="lessonId">Id of the desirable lesson.</param>
        /// <param name="studentId">Id of the desirable student.</param>
        /// <returns>The particalar <see cref="Mark"/>.</returns>
        [HttpGet("{lessonId}/{studentId}")]
        public IActionResult Get(int lessonId, int studentId)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var marks = this.context.Marks.Where(m => m.LessonId == lessonId && m.StudentId == studentId);

                return this.Ok(marks);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// The method gets a particular mark by id.
        /// </summary>
        /// <param name="markId">Id of the desirable mark.</param>
        /// <returns>The particalar <see cref="Mark"/>.</returns>
        [HttpGet("{markId}")]
        public IActionResult Get(string markId)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var mark = this.context.Marks.Find(ObjectId.Parse(markId));

                return this.Ok(mark);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Refreshes mark's data.
        /// </summary>
        /// <param name="lessonId">Desirable lesson Id.</param>
        /// <param name="studentId">Desirable student Id.</param>
        /// <param name="updateMark">New mark data.</param>
        /// <returns>The resulting <see cref="IActionResult"/>.</returns>
        [HttpPut("{lessonId}/{studentId}")]
        public IActionResult Put(int lessonId, int studentId, [FromBody] Mark updateMark)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var mark = this.context.Marks.FirstOrDefault(m => m.LessonId == lessonId
                    && m.StudentId == studentId);

                if (mark == null)
                {
                    return this.NotFound();
                }

                mark.Value = updateMark.Value;
                this.context.Marks.Update(mark);
                this.context.SaveChanges();

                return this.Ok();
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Creates list of average mark for parcticular student and particelar subject.
        /// </summary>
        /// <param name="studentId">Desirable student id.</param>
        /// <param name="subjectId">Desirable subject id.</param>
        /// <returns>List of <see cref="Dtos.AverageMarkDto"/>.</returns>
        [HttpGet("/ave/{studentId}/{subjectId}")]
        public IActionResult GetAverageMark(int studentId, int subjectId)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var marks = this.marksAnalyticsService.GetAverageMarks(studentId, subjectId);

                return this.Ok(marks);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Adds new mark to the DB.
        /// </summary>
        /// <param name="mark">New mark data.</param>
        /// <returns>The resulting <see cref="IActionResult"/>.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Mark mark)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                this.context.Marks.Add(mark);
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
