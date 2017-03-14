using ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Sorting;

namespace ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Grouping
{
    public class GroupingInfo : SortingInfo
    {
        public string GroupInterval;
        public bool? IsExpanded;

        public bool GetIsExpanded()
        {
            if (!IsExpanded.HasValue)
                return true;

            return IsExpanded.Value;
        }
    }
}
