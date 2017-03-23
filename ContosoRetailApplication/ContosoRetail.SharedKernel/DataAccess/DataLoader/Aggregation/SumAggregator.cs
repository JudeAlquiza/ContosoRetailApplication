using System;
using System.Globalization;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation
{
    public class SumAggregator<T> : ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>
    {
        decimal? _sum;

        public SumAggregator(IAccessor<T> accessor)
            : base(accessor)
        {
        }

        public override void Step(T container, string selector)
        {
            var value = Accessor.Read(container, selector);

            if (value != null)
            {
                if (!_sum.HasValue)
                    _sum = 0;

                try
                {
                    _sum += Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }
            }
        }

        public override object Finish()
        {
            return _sum;
        }

    }
}
