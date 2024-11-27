using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TSNO.Data;
using TSNO.Models;
using TSNO.Models.DTO;
using TSNO.Models.ResponseDTO;
using TSNO.Services.Expiration;

namespace TSNO.Controllers
{
    [ApiController]
    [Route("api")]
    [EnableRateLimiting("FixedPolicy")]
    public class MainController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpirationService _expirationService;

        public MainController(ApplicationDbContext dbContext, IExpirationService expirationService)
        {
            _dbContext = dbContext;
            _expirationService = expirationService;
        }
        [HttpGet("view-stats")]
        public async Task<IActionResult> ViewStats()
        {
            var stats = await _dbContext.Stats.FirstAsync();
            return Ok(new List<StatsDTO>
    {
                new StatsDTO
                {
                    Title = "Total Messages Transferred",
                    Value = stats.TotalNotes
                },
                new StatsDTO
                {
                    Title = "Active Messages Transferred",
                    Value = stats.ActiveNotes
                }
            });
        }



        [HttpPost("add-note")]
        public async Task<IActionResult> AddNote([FromBody] AddDTO newNote)
        {
            if (newNote == null || string.IsNullOrWhiteSpace(newNote.Notes))
            {
                return BadRequest("Note content is required.");
            }

            // Fetch existing codes and available codes
            var existingCodes = new HashSet<int>(await _dbContext.Entities.Select(e => e.Code).ToListAsync());
            var availableCodes = Enumerable.Range(0, 10000).Where(code => !existingCodes.Contains(code)).ToList();

            if (!availableCodes.Any())
            {
                return BadRequest("No codes are available. Please try again later.");
            }

            var random = new Random();
            var generatedCode = availableCodes[random.Next(availableCodes.Count)];

            // Create the new note entity
            var addNote = new Entity
            {
                Code = generatedCode,
                Notes = newNote.Notes,
                DeleteWhenOpen = newNote.DeleteWhenOpen,
            };

            // Save the note to the database
            await _dbContext.Entities.AddAsync(addNote);
            await _dbContext.SaveChangesAsync();

            // Update Stats in the database
            var stats = await _dbContext.Stats.FirstAsync();
            stats.TotalNotes++;
            stats.ActiveNotes = await _dbContext.Entities
                                                .CountAsync(e => e.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
            await _dbContext.SaveChangesAsync();

            addNote.Notes = "";
            return Ok(new { Message = "Note added successfully!", addNote });
        }



        [HttpGet("view-note")]
        public async Task<IActionResult> ViewNote([FromQuery] ViewDTO viewDto)
        {
            if (viewDto == null || string.IsNullOrWhiteSpace(viewDto.Code))
            {
                return BadRequest("Code is required.");
            }

            if (!int.TryParse(viewDto.Code, out int parsedCode) || parsedCode < 0 || parsedCode > 9999)
            {
                return BadRequest("Invalid code format.");
            }

            var note = await _dbContext.Entities.FirstOrDefaultAsync(e => e.Code == parsedCode);

            if (note == null)
            {
                return NotFound("Note with the specified code does not exist.");
            }
            if (note.DeleteWhenOpen == true)
            {
                _dbContext.Remove(note);
                await _dbContext.SaveChangesAsync();
            }
            return Ok(new { Id = note.Id, Notes = note.Notes }); //here I had to choose between deleting the note either on background in backend or frontend returning the Id, the safe way would be doing in background, but that should be done only if the client want the note to self destruct on view
        }

    }
}
