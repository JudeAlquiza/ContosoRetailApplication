using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational;
using System.Linq;

namespace BC_ContosoRecordsModule.DataAccess.Repositories
{
    public class CustomerOrdersRepository : NoDeleteRepository<CustomerOrders, int>, ICustomerOrdersRepository
    {
        public override IQueryable<CustomerOrders> GetAll()
        {
            // TODO:
        }
    }
}
