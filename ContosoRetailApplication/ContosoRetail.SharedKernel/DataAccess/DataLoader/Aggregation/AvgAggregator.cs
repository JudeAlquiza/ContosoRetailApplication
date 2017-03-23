namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation
{
    
    public class AvgAggregator<T> : ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T>
    {
        ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T> _counter;
        ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.SumAggregator<T> _summator;

        public AvgAggregator(IAccessor<T> accessor, ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.Aggregator<T> counter)
            : base(accessor)
        {
            _counter = counter;
            _summator = new ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation.SumAggregator<T>(accessor);
        }

        public override void Step(T container, string selector)
        {
            _counter.Step(container, selector);
            _summator.Step(container, selector);
        }

        public override object Finish()
        {
            var count = _counter.Finish();
            var sum = _summator.Finish();

            if (Equals(count, 0))
                return null;

            return (decimal)sum / (int)count;
        }
    }
}
