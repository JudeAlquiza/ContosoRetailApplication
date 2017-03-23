using System.Collections;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    public class Group
    {
        public object Key { get; set; }
        public IList Items { get; set; }
        public int? Count { get; set; }
        public object[] Summary { get; set; }
    }
}
