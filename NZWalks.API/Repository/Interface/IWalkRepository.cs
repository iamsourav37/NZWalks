using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository.Interface
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAll(string? filterOn, string? filterQuery);
        Task<Walk> GetById(Guid walkId);
        Task Create(Walk walk);
        Task Update(Walk walk);
        Task Delete(Guid walkId);
    }
}
