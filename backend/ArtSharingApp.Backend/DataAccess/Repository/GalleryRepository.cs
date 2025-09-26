using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtSharingApp.Backend.DataAccess.Repository;

public class GalleryRepository : GenericRepository<Gallery>, IGalleryRepository
{
    public GalleryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Gallery>> GetGalleriesByName(string name)
    {
        return await _dbSet.Where(g => g.Name.ToLower().Contains(name.ToLower()))
            .Include(g => g.City).ToListAsync();
    }
}
