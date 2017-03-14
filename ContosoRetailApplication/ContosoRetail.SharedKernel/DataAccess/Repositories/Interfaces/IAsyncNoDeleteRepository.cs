using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface IAsyncNoDeleteRepository<TEntity, TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TKey> AddAsync(TEntity newEntity);
        Task<TEntity> UpdateAsync(TEntity updatedEntity);
    }
}
