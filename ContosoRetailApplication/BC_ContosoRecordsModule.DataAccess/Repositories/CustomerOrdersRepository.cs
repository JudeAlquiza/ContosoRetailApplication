using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using BC_ContosoRecordsModule.DataModel.Model;
using ContosoRetail.SharedKernel.DataAccess.Repositories;
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
            //var test = _ctx.CustomerOrders.Select("new (ProductCategoryName, ProductSubcategory, Product)");
            //var genericToList = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(new Type[] { test.ElementType });
            //var list = (IList)genericToList.Invoke(null, new[] { test });

            return _ctx.CustomerOrders;
        }
    }
}
