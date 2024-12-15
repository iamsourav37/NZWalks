using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository.Interface;

namespace NZWalks.API.Repository.Implementation
{
    public class DifficultyRepository : IDifficultyRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public DifficultyRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IEnumerable<Difficulty>> GetAll()
        {
            return await _dbContext.Difficulties.ToListAsync();
        }

        public async Task<Difficulty> GetById(Guid difficultyId)
        {
            return await _dbContext.Difficulties.FindAsync(difficultyId);
        }
    }
}
