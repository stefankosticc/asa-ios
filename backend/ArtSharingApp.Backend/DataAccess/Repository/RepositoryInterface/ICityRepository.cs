using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetCitiesByName(string name);
}