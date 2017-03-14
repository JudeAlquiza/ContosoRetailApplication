using System.Linq;
using System.Threading.Tasks;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface IAsyncNoDeleteRepository<TEntity, TKey>
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TKey> AddAsync(TEntity newEntity);
        Task<TEntity> UpdateAsync(TEntity updatedEntity);
    }
}
