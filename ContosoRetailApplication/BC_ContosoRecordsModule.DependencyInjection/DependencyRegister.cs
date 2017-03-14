using BC_ContosoRecordsModule.Application.Queries;
using BC_ContosoRecordsModule.Application.Queries.Interfaces;
using BC_ContosoRecordsModule.DataAccess.Repositories;
using BC_ContosoRecordsModule.DataAccess.Repositories.Interfaces;
using BC_ContosoRecordsModule.DataModel.Model;
using Microsoft.Extensions.DependencyInjection;

namespace BC_ContosoRecordsModule.DependencyInjection
{
    public static class DependencyRegister
    {
        public static void AddContosoRecordsModule(this IServiceCollection services)
        {
            services.AddTransient<ICustomerOrdersRepository, CustomerOrdersRepository>();
            services.AddTransient<IGetCustomerOrdersDTOsWithOptionsQuery, GetCustomerOrdersDTOsWithOptionsQuery>();
            services.AddTransient<ContosoRecordsModuleContext>();
        }
    }
}
