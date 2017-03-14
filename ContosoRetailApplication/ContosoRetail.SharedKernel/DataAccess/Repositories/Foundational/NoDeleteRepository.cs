using ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational
{
    public abstract class NoDeleteRepository<TEntity, TKey> : NonAsyncNoDeleteRepository<TEntity, TKey>, IAsyncNoDeleteRepository<TEntity, TKey>
    {
        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TKey> AddAsync(TEntity newEntity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
