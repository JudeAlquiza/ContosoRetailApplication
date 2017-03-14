using System;
using System.Threading.Tasks;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational
{
    public abstract class Repository<TKey, TEntity> : NoDeleteRepository<TEntity, TKey>
    {
        public TEntity DeleteById(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeleteByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }
    }
}
