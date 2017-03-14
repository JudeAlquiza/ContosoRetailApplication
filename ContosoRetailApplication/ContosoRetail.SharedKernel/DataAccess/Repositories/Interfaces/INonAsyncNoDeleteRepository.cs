using System.Linq;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface INonAsyncNoDeleteRepository<TEntity, TKey>
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(TKey id);
        TKey Add(TEntity newEntity);
        TEntity Update(TEntity updatedEntity);
    }
}
