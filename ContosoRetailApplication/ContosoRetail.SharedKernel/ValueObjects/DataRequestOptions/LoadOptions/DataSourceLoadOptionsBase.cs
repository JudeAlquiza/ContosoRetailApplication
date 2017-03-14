using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Grouping;
using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Sorting;
using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Summary;
using System.Collections;

namespace ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.LoadOptions
{
    public class DataSourceLoadOptionsBase
    {
        public bool RequireTotalCount;
        public bool RequireGroupCount;
        public bool IsCountQuery;
        public int Skip;
        public int Take;
        public SortingInfo[] Sort;
        public GroupingInfo[] Group;
        public IList Filter;
        public SummaryInfo[] TotalSummary;
        public SummaryInfo[] GroupSummary;

        public bool? RemoteGrouping;
        public string[] PrimaryKey;
        public string DefaultSort;
    }
}
