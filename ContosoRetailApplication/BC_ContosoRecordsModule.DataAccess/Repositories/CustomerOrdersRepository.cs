using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using BC_ContosoRecordsModule.DataModel.Model;
using ContosoRetail.SharedKernel.DataAccess.Repositories.Foundational;
using System.Linq;

namespace BC_ContosoRecordsModule.DataAccess.Repositories
{
    public class CustomerOrdersRepository : NoDeleteRepository<CustomerOrders, int>, ICustomerOrdersRepository
    {
        private ContosoRecordsModuleContext _ctx;

        public CustomerOrdersRepository(ContosoRecordsModuleContext ctx)
        {
            _ctx = ctx;
        }

        public override IQueryable<CustomerOrders> GetAll()
        {
            return _ctx.CustomerOrders;
        }
    }
}
