using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<City>> GetCitiesByName(string name)
    {
        return await _dbSet.Where(c => c.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}