namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface INonAsyncRepository<TEntity, TKey> : INonAsyncNoDeleteRepository<TEntity, TKey>
    {
        TEntity DeleteById(TKey id);
    }
}
