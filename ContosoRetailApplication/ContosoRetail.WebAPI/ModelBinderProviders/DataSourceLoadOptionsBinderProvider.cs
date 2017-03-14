using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.LoadOptions;
using ContosoRetail.WebAPI.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContosoRetail.WebAPI.ModelBinderProviders
{
    public class DataSourceLoadOptionsBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DataSourceLoadOptions))
                return new DataSourceLoadOptionsBinder();

            return null;
        }
    }
}
