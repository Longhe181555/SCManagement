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

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet("byName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var student = await _studentService.GetStudentByNameAsync(name);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result) return NotFound();
            return Ok("Deleted Successfully");
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.UpdateStudentAsync(student);
            if (!result) return NotFound();
            return Ok("Updated Successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }
    }
}
