namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.RemoteGrouping
{
    public class RemoteGroupTypeMarkup
    {
        int
            _groupCount,
            _totalSummaryCount,
            _groupSummaryCount;

        public RemoteGroupTypeMarkup(int groupCount, int totalSummaryCount, int groupSummaryCount)
        {
            _groupCount = groupCount;
            _totalSummaryCount = totalSummaryCount;
            _groupSummaryCount = groupSummaryCount;
        }

        public static int CountIndex
        {
            get { return 0; }
        }

        public static int KeysStartIndex
        {
            get { return 1; }
        }

        public int TotalSummaryStartIndex
        {
            get { return KeysStartIndex + _groupCount; }
        }

        public int GroupSummaryStartIndex
        {
            get { return TotalSummaryStartIndex + _totalSummaryCount; }
        }

    }

}
