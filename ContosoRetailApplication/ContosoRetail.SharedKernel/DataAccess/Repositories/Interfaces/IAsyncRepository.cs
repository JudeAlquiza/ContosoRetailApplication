using System.Threading.Tasks;

namespace ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces
{
    public interface IAsyncRepository<TEntity, TKey> : IAsyncNoDeleteRepository<TEntity, TKey>
    {
        Task<TEntity> DeleteByIdAsync(TKey id);
    }
}
