using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.LoadOptions;

namespace BC_ContosoRecordsModule.Application.Queries.Interfaces
{
    public interface IGetCustomerOrdersDTOsWithOptionsQuery
    {
        dynamic Execute(DataSourceLoadOptionsBase options);
    }
}
