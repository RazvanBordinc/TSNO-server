using Microsoft.AspNetCore.Mvc;
using TSNO.Data;
using TSNO.Models;
using TSNO.Models.ResponseDTO;
using TSNO.Services.Expiration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TSNO.Controllers
{
    [ApiController]
    [Route("api")]
    public class MainController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpirationService _expirationService;

        public MainController(ApplicationDbContext dbContext, IExpirationService expirationService)
        {
            _dbContext = dbContext;
            _expirationService = expirationService;
        }

        [HttpPost("add-note")]
        public async Task<IActionResult> AddNote([FromBody] AddDTO newNote)
        {
            if (newNote == null || string.IsNullOrWhiteSpace(newNote.Notes))
            {
                return BadRequest("Note content is required.");
            }

            var existingCodes = new HashSet<int>(await _dbContext.Entities.Select(e => e.Code).ToListAsync());


            var availableCodes = Enumerable.Range(0, 10000).Where(code => !existingCodes.Contains(code)).ToList();

            if (!availableCodes.Any())
            {
                return BadRequest("No codes are available. Please try again later.");
            }

            var random = new Random();
            var generatedCode = availableCodes[random.Next(availableCodes.Count)];

            var addNote = new Entity
            {
                Code = generatedCode,
                Notes = newNote.Notes
            };

            await _dbContext.Entities.AddAsync(addNote);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "Note added successfully!", addNote });
        }

    }
}
