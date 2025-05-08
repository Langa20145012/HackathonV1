using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackath_Application_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatterController : ControllerBase
    {
        private readonly IMatterService _matterService;

        public MatterController(IMatterService matterService)
        {
            _matterService = matterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatters()
        {
            var matters = await _matterService.GetAllMattersAsync();
            return Ok(matters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatterById(int id)
        {
            var matter = await _matterService.GetMatterByIdAsync(id);

            if (matter == null)
                return NotFound();

            return Ok(matter);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatter([FromBody] Matter matter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMatter = await _matterService.CreateMatterAsync(matter);
            return CreatedAtAction(nameof(GetMatterById), new { id = createdMatter.MatterId }, createdMatter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatter(int id, [FromBody] Matter matter)
        {
            if (id != matter.MatterId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedMatter = await _matterService.UpdateMatterAsync(matter);

            if (updatedMatter == null)
                return NotFound();

            return Ok(updatedMatter);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatter(int id)
        {
            var result = await _matterService.DeleteMatterAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
