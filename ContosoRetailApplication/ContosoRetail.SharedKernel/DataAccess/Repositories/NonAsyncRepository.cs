using System;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories
{
    public abstract class NonAsyncRepository<TEntity, TKey> : NonAsyncNoDeleteRepository<TEntity, TKey>
    {
        public TEntity DeleteById(TKey id)
        {
            throw new NotImplementedException();
        }
    }
}
