using System;
using System.Collections.Generic;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation
{
    public class MinAggregator<T> : ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>
    {
        object _min = null;

        public MinAggregator(IAccessor<T> accessor)
            : base(accessor)
        {
        }

        public override void Step(T container, string selector)
        {
            var value = Accessor.Read(container, selector);

            if (value is IComparable)
            {
                if (_min == null || Comparer<object>.Default.Compare(value, _min) < 0)
                    _min = value;
            }
        }

        public override object Finish()
        {
            return _min;
        }

    }
}
