using TSNO.Models;

namespace TSNO.Services.Expiration
{
    public interface IExpirationService
    {
        bool IsExpired(Entity entity);
        Task DeleteExpiredEntitiesAsync();
    }
}
