using Microsoft.AspNetCore.Mvc;
using Stellar.Shared.Models;
using Stellar.Shared.Services;
using System.Collections.Generic;

namespace Stellar.Shared.APIs
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApi<E, ID, RES, REQ, PAGE_RES> : ControllerBase
    {
        protected abstract IBaseService<E, ID, RES, REQ, PAGE_RES> Service { get; }

        protected HeaderContext Context =>
            HttpContext.RequestServices.GetService(typeof(HeaderContext)) as HeaderContext;

        [HttpPost]
        public virtual ActionResult<RES> Create([FromBody] REQ request)
        {
            return Ok(Service.Create(Context, request));
        }

        [HttpPut("{id}")]
        public virtual ActionResult<RES> Update(ID id, [FromBody] REQ request)
        {
            return Ok(Service.Update(Context, id, request));
        }

        [HttpGet("{id}")]
        public virtual ActionResult<RES> GetById(ID id)
        {
            return Ok(Service.GetById(Context, id));
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(ID id)
        {
            Service.Delete(Context, id);
            return NoContent();
        }

        [HttpGet]
        public virtual ActionResult<Page<PAGE_RES>> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? sort = null,
            [FromQuery] string? filter = null)
        {
            var filterMap = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                try
                {
                    filterMap = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(filter);
                }
                catch
                {
                    // Ignore parsing errors or log them? properties matching will just fail gracefully or be empty.
                }
            }
            
            // Call Service.GetAll
            return Ok(Service.GetAll(Context, search, page, size, sort, filterMap ?? new Dictionary<string, object>()));
        }
    }
}
