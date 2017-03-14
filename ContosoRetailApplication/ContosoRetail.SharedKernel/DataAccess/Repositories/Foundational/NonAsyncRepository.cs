using System;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational
{
    public abstract class NonAsyncRepository<TEntity, TKey> : NonAsyncNoDeleteRepository<TEntity, TKey>
    {
        public TEntity DeleteById(TKey id)
        {
            throw new NotImplementedException();
        }
    }
}
