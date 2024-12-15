using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository.Interface
{
    public interface IDifficultyRepository
    {
        Task<IEnumerable<Difficulty>> GetAll();
        Task<Difficulty> GetById(Guid difficultyId);
    }
}
