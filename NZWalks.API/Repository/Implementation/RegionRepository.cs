using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository.Interface;

namespace NZWalks.API.Repository.Implementation
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _context;
        public RegionRepository(NZWalksDbContext context)
        {
            _context = context;
        }


        public async Task CreateRegion(Region region)
        {
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRegion(Guid regionId)
        {
            var existingRegion = await _context.Regions.FirstOrDefaultAsync(region => region.Id == regionId);
            if (existingRegion != null)
            {
                _context.Regions.Remove(existingRegion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Region>> GetAll()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task<Region> GetById(Guid regionId)
        {
            return await _context.Regions.FindAsync(regionId);
        }

        public async Task UpdateRegion(Region region)
        {
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
        }
    }
}
