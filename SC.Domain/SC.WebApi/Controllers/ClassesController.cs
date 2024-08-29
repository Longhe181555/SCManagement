using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SC.Application.Common.Interfaces;
using SC.Application.Common.ViewModels;

namespace SC.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly ILogger<ClassController> _logger;

        public ClassController(IClassService classService, ILogger<ClassController> logger)
        {
            _classService = classService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all classes");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var classRequest = await _classService.GetClassByIdAsync(id);
                if (classRequest == null) return NotFound();
                return Ok(classRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the class with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getByName")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var classRequest = await _classService.GetClassByNameAsync(name);
                if (classRequest == null) return NotFound();
                return Ok(classRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the class with name {name}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);
                if (!result) return NotFound();
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the class with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] EnrollViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _classService.UpdateClassAsync(request);
                if (result == null) return NotFound();

                return Ok("Class updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the class");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnrollViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _classService.CreateClassAsync(request);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new class");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
