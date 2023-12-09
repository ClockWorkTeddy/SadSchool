using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SadSchool.Models;
using System.Diagnostics;
using System.Configuration;

namespace SadSchool.Controllers.RestApi
{
    [ApiController]
    [Route("api/MarkController")]
    public class MarkController : Controller
    {
        private readonly SadSchoolContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey = string.Empty;
        public MarkController(SadSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _apiKey = _configuration["api-key"];
        }

        [HttpGet]
        public IActionResult Get()
        {
            var apiKey = HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == _apiKey )
            {
                var marks = _context.Marks.ToList();

                return Ok(marks);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var apiKey = HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == _apiKey)
            {
                var marks = _context.Marks.Where(m => m.Id == id);

                return Ok(marks);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Mark updateMark)
        {
            var apiKey = HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (apiKey == _apiKey)
            {
                var mark = _context.Marks.FirstOrDefault(m => m.Id == id);
                
                if (mark == null)
                    return NotFound();
                
                mark.Value = updateMark.Value;
                _context.Marks.Update(mark);
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
