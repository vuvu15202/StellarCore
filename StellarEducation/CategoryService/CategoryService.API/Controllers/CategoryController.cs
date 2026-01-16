using CategoryService.Application.DTOs;
using CategoryService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CategoryService.API.Controllers
{
    [Route("api/categories/{mapping}")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetAll(string mapping)
        {
            return Ok(await _service.GetAllAsync(mapping));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create(string mapping, CategoryDTO dto)
        {
            var result = await _service.CreateAsync(mapping, dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string mapping, Guid id, CategoryDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            await _service.UpdateAsync(mapping, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string mapping, Guid id)
        {
            await _service.DeleteAsync(mapping, id);
            return NoContent();
        }
    }
}
