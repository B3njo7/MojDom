using Microsoft.AspNetCore.Mvc;
using MojDom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MojDom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _context.Cities
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name, c.Country })
                .ToListAsync();

            return Ok(cities);
        }
    }
}