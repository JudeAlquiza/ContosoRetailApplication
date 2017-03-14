namespace BC_ContosoRecordsModule.Core.Entities
{
    public class OnlineSalesOrder
    {
        public string OrderNumber { get; set; }
        public int CustomerKey { get; set; }
        public string Region { get; set; }
        public string IncomeGroup { get; set; }
    }
}
