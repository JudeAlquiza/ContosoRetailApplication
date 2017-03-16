using BC_ContosoRecordsModule.Application.LoadOptions;

namespace BC_ContosoRecordsModule.Application.Queries.Interfaces
{
    public interface IGetCustomerOrdersDTOsWithOptionsQuery
    {
        dynamic Execute(DataSourceLoadOptions options);
    }
}
