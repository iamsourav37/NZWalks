using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository.Interface;

namespace NZWalks.API.Repository.Implementation
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public WalkRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task Create(Walk walk)
        {
            _dbContext.Walks.Add(walk);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid walkId)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(walk => walk.Id == walkId);

            if (existingWalk != null)
            {
                _dbContext.Walks.Remove(existingWalk);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Walk>> GetAll(string? filterOn = null, string? filterQuery = null)
        {
            var query = _dbContext.Walks.AsQueryable();

            if (!string.IsNullOrEmpty(filterOn))
            {
                filterQuery = filterQuery.ToLower();
                if (filterOn.Equals("name"))
                {
                    query = query.Where(walk => walk.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("lengthinkm"))
                {
                    var doubleValue = double.Parse(filterQuery);
                    query = query.Where(walk => walk.LengthInKm == doubleValue);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Walk> GetById(Guid walkId)
        {
            return await _dbContext.Walks.FindAsync(walkId);
        }

        public async Task Update(Walk walk)
        {
            _dbContext.Walks.Update(walk);
            await _dbContext.SaveChangesAsync();
        }
    }
}
