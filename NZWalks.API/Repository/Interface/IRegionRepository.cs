using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository.Interface
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAll();
        Task<Region> GetById(Guid regionId);
        Task CreateRegion(Region region);
        Task UpdateRegion(Region region);
        Task DeleteRegion(Guid regionId);
    }
}
