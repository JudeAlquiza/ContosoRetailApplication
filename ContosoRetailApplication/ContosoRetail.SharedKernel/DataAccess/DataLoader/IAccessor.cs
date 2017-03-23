namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    public interface IAccessor<T>
    {
        object Read(T container, string selector);
    }
}
