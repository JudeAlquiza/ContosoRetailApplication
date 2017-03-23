namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    public class GroupingInfo : SortingInfo
    {
        public string GroupInterval { get; set; }
        public bool? IsExpanded { get; set; }

        public bool GetIsExpanded()
        {
            if (!IsExpanded.HasValue)
                return true;

            return IsExpanded.Value;
        }
    }

}
