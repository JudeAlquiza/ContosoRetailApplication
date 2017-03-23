using System.Collections;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    public class DataSourceLoadResult
    {
        public IEnumerable data;

        public int totalCount = -1;

        public int groupCount = -1;

        public object[] summary;

        internal bool IsDataOnly()
        {
            return totalCount == -1 && summary == null && groupCount == -1;
        }
    }

}
