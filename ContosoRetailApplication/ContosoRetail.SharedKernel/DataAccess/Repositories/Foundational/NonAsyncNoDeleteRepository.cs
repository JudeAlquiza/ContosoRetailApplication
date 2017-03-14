using ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational
{
    public abstract class NonAsyncNoDeleteRepository<TEntity, TKey> : INonAsyncNoDeleteRepository<TEntity, TKey>
    {
        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual TEntity GetById(TKey id)
        {
            throw new NotImplementedException();
        }

        public virtual TKey Add(TEntity newEntity)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Update(TEntity updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
