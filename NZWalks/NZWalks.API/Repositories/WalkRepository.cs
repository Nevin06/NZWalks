using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(w => w.Region)
                .Include(w => w.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk?> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(w => w.Region)
                .Include(w => w.WalkDifficulty)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign New ID
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);
            if (walk == null) 
            {
                return null;
            }

            // Delete the region
            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync(true);
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            //existingWalk.Region = walk.Region;
            //existingWalk.WalkDifficulty = walk.WalkDifficulty;

            await nZWalksDbContext.SaveChangesAsync();
            //return existingWalk;
            return await GetAsync(existingWalk.Id);
        }
    }
}
