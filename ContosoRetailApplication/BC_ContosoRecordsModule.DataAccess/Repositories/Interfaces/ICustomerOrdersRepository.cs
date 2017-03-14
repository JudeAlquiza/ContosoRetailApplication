using BC_ContosoRecordsModule.Core.Entities;
using ContosoRetail.SharedKernel.DataAccess.Repositories.Interfaces;

namespace BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces
{
    public interface ICustomerOrdersRepository : IAsyncNoDeleteRepository<CustomerOrders, int>, INonAsyncNoDeleteRepository<CustomerOrders, int>
    {
    }
}
