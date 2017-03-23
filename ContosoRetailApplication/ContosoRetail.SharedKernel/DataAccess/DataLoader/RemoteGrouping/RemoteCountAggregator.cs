using ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation;
using ContosoRetail.SharedKernel.DataAccess.DataLoader.Types;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.RemoteGrouping
{    
    public class RemoteCountAggregator<T> : Aggregator<T>
    {
        int _count = 0;

        public RemoteCountAggregator(IAccessor<T> accessor)
            : base(accessor)
        {
        }

        public override void Step(T dataitem, string selector)
        {
            var group = dataitem as AnonType;
            _count += (int)group[RemoteGroupTypeMarkup.CountIndex];
        }

        public override object Finish()
        {
            return _count;
        }
    }
}
