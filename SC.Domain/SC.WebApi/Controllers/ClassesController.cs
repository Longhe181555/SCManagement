using Microsoft.AspNetCore.Mvc;
using SC.Application.Common.Interfaces;
using SC.Application.Common.ViewModels;

namespace SC.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var classes = await _classService.GetAllClassesAsync();
            return Ok(classes);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> Get(int id)
        {
            var @class = await _classService.GetClassByIdAsync(id);
            if (@class == null) return NotFound();
            return Ok(@class);
        }

        [HttpGet("getByName")]
        public async Task<IActionResult> Get(string name)
        {
            var @class = await _classService.GetClassByNameAsync(name);
            if (@class == null) return NotFound();
            return Ok(@class);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classService.DeleteClassAsync(id);
            if (!result) return NotFound();
            return Ok("Deleted Successfully");
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] EnrollViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _classService.UpdateClassAsync(request);
            if (result == null) return NotFound();
            return Ok("Class updated successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnrollViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _classService.CreateClassAsync(request);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}
