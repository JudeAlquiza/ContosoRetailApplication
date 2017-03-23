namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Aggregation
{
    public abstract class Aggregator<T>
    {
        protected readonly IAccessor<T> Accessor;

        public Aggregator(IAccessor<T> accessor)
        {
            Accessor = accessor;
        }

        public abstract void Step(T container, string selector);
        public abstract object Finish();
    }
}
