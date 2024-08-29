using Microsoft.AspNetCore.Mvc;
using SC.Application.Common.Interfaces;
using SC.Domain.Entities;

namespace SC.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all students");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null) return NotFound();
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the student with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("byName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var student = await _studentService.GetStudentByNameAsync(name);
                if (student == null) return NotFound();
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the student with name {name}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);
                if (!result) return NotFound();
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the student with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _studentService.UpdateStudentAsync(student);
                if (!result) return NotFound();
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the student");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdStudent = await _studentService.CreateStudentAsync(student);
                return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new student");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
