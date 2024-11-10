using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSNO.Data;
using TSNO.Models;

namespace TSNO.Services.Expiration
{
    public class ExpirationService : IExpirationService
    {
        private readonly ApplicationDbContext _context;

        public ExpirationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsExpired(Entity entity)
        {
            return (DateTime.UtcNow - entity.CreatedAt).TotalMinutes >= 5;
        }

        public async Task DeleteExpiredEntitiesAsync()
        {
            var expiredEntities = await _context.Entities
                                                .Where(entity => (DateTime.UtcNow - entity.CreatedAt).TotalMinutes >= 5)
                                                .ToListAsync();

            if (expiredEntities.Any())
            {
                _context.Entities.RemoveRange(expiredEntities);
                await _context.SaveChangesAsync();
            }
        }

    }
}
