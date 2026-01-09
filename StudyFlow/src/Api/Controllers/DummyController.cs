using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyFlow.src.Application.Abstractions;

namespace StudyFlow.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        private readonly IDummyCourseService _courses;
        private readonly IDummySessionService _sessions;

        public DummyController(IDummyCourseService courses, IDummySessionService sessions)
        {
            _courses = courses;
            _sessions = sessions;
        }

        // GET: api/Dummy/courses
        [HttpGet("courses")]
        public IActionResult Courses() => Ok(_courses.GetCourses());

        // POST: api/Dummy/courses
        [HttpPost("courses")]
        public IActionResult CreateCourse(CreateDummyCourseDto dto)
        {
            var created = _courses.CreateCourse(dto);
            return StatusCode(StatusCodes.Status201Created, created);
        }

        // GET: api/Dummy/sessions
        [HttpGet("sessions")]
        public IActionResult Sessions() => Ok(_sessions.GetSessions());

        // POST: api/Dummy/sessions
        [HttpPost("sessions")]
        public IActionResult CreateSession(CreateDummySessionDto dto)
        {
            var created = _sessions.CreateSession(dto);
            return StatusCode(StatusCodes.Status201Created, created);
        }
    }
}
