using Hackath_Application_API.Interfaces;
using Hackathon_Application_Database.DatabaseContext;
using Hackathon_Application_Database.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace Hackath_Application_API.Services
{
    public class MatterService : IMatterService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public MatterService(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<IEnumerable<Matter>> GetAllMattersAsync()
        {
            string cacheKey = "AllMatters";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<Matter>>(cachedData);
            }

            var matters = _dbContext.Matters.ToList(); // Replace with Async.........

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(matters),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiration }
            );

            return matters;
        }

        public async Task<Matter> GetMatterByIdAsync(int matterId)
        {
            string cacheKey = $"Matter_{matterId}";
            string cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<Matter>(cachedData);
            }

            var matter = await _dbContext.Matters.FindAsync(matterId);

            if (matter != null)
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(matter),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiration }
                );
            }

            return matter;
        }

        public async Task<Matter> CreateMatterAsync(Matter matter)
        {
            matter.CreatedDate = DateTime.UtcNow;
            matter.LastModifiedDate = DateTime.UtcNow;

            _dbContext.Matters.Add(matter);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync("AllMatters");

            return matter;
        }

        public async Task<Matter> UpdateMatterAsync(Matter matter)
        {
            var existingMatter = await _dbContext.Matters.FindAsync(matter.MatterId);

            if (existingMatter == null)
                return null;

            existingMatter.AccountNumber = matter.AccountNumber;
            existingMatter.Description = matter.Description;
            existingMatter.StatusId = matter.StatusId;
            //existingMatter.ClientEmail = matter.ClientEmail;
            existingMatter.LastModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync("AllMatters");
            await _cache.RemoveAsync($"Matter_{matter.MatterId}");

            return existingMatter;
        }

        public async Task<bool> DeleteMatterAsync(int matterId)
        {
            var matter = await _dbContext.Matters.FindAsync(matterId);

            if (matter == null)
                return false;

            _dbContext.Matters.Remove(matter);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _cache.RemoveAsync("AllMatters");
            await _cache.RemoveAsync($"Matter_{matterId}");

            return true;
        }
    }
}
