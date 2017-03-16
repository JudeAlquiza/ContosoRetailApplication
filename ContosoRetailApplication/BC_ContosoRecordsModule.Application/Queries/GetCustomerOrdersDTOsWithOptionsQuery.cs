using AutoMapper;
using BC_ContosoRecordsModule.Application.LoadOptions;
using BC_ContosoRecordsModule.Application.Queries.Interfaces;
using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using DevExtreme.AspNet.Data;
using System.Linq;

namespace BC_ContosoRecordsModule.Application.Queries
{
    public class GetCustomerOrdersDTOsWithOptionsQuery : IGetCustomerOrdersDTOsWithOptionsQuery
    {
        private ICustomerOrdersRepository _customerOrdersRepo;
        private IMapper _mapper;

        public GetCustomerOrdersDTOsWithOptionsQuery(ICustomerOrdersRepository customerOrdersRepo, IMapper mapper)
        {
            _customerOrdersRepo = customerOrdersRepo;
            _mapper = mapper;
        }

        public dynamic Execute(DataSourceLoadOptions options)
        {
            IQueryable<CustomerOrders> entities = _customerOrdersRepo.GetAll();

            if (entities != null)
            {
                options.Sort = options.Sort == null ? new SortingInfo[] { new SortingInfo { Selector = "Region", Desc = false } } : options.Sort;
                options.Take = options.Take == 0 ? 100 : options.Take;

                var responseData = DataSourceLoader.Load(entities, options);

                return responseData;
            }

            return entities;
        }       
    }
}
