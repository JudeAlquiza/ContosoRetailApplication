using System.Linq;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Helpers
{
    public static class Compat
    {
        internal static bool IsEntityFramework(IQueryProvider provider)
        {
            var type = provider.GetType().FullName;

            return type == "System.Data.Entity.Internal.Linq.DbQueryProvider"
                || type == "Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider"
                || type == "Microsoft.Data.Entity.Query.Internal.EntityQueryProvider";
        }

    }
}
