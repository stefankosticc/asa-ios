using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T obj);
    void Update(T obj);
    Task DeleteAsync(object id);
    Task SaveAsync();
}
