using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IGalleryRepository : IGenericRepository<Gallery>
{
    Task<IEnumerable<Gallery>> GetGalleriesByName(string name);
}