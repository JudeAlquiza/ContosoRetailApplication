using System.Collections;

namespace ContosoRetail.SharedKernel.ValueObjects.DataRequestOptions.Grouping
{
    public class Group
    {
        public object key { get; set; }
        public IList items { get; set; }
        public int count { get; set; }
        public object[] summary { get; set; }
        public bool isContinuationOnNextPage { get; set; }
    }
}
