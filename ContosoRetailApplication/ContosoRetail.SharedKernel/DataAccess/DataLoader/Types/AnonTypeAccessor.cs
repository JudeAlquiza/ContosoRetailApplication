using System;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader.Types
{
    public class AnonTypeAccessor : IAccessor<AnonType>
    {
        public object Read(AnonType container, string selector)
        {
            if (selector.StartsWith(AnonType.ITEM_PREFIX))
                return container[int.Parse(selector.Substring(1))];
            throw new ArgumentException();
        }

    }
}
