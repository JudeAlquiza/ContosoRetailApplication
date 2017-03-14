using System.Collections.Generic;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface INonAsyncNoDeleteRepository<TEntity, TKey>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(TKey id);
        TKey Add(TEntity newEntity);
        TEntity Update(TEntity updatedEntity);
    }
}
