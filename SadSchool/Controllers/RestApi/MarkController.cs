// <copyright file="MarkController.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Controllers.RestApi
{
    using Microsoft.AspNetCore.Mvc;
    using SadSchool.Models;
    using SadSchool.Services.ApiServices;

    /// <summary>
    /// The controller serves marks processing.
    /// </summary>
    [ApiController]
    [Route("api/MarkController")]
    public class MarkController : Controller
    {
        private readonly string? apiKey = string.Empty;
        private readonly SadSchoolContext context;
        private readonly IConfiguration configuration;
        private readonly IMarksAnalyticsService marksAnalyticsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkController"/> class.
        /// </summary>
        /// <param name="context">Application DB context.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="markAnalyticSevice">MarkAnalytic service instance.</param>
        public MarkController(SadSchoolContext context, IConfiguration configuration, IMarksAnalyticsService markAnalyticSevice)
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
        /// <param name="id">Id of the desirable mark.</param>
        /// <returns>The particalar <see cref="Mark"/>.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var marks = this.context.Marks.Where(m => m.Id == id);

                return this.Ok(marks);
            }
            else
            {
                return this.Unauthorized();
            }
        }

        /// <summary>
        /// Refreshes mark's data.
        /// </summary>
        /// <param name="id">Refreshed mark's id.</param>
        /// <param name="updateMark">Refreshing data.</param>
        /// <returns>The resulting <see cref="IActionResult"/>.</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Mark updateMark)
        {
            var apiKey = this.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == this.apiKey)
            {
                var mark = this.context.Marks.FirstOrDefault(m => m.Id == id);

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
        /// <returns>List of <see cref="AverageMark"/>.</returns>
        [HttpGet("{studentId}/{subjectId}")]
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
    }
}
