using System;

namespace BC_ContosoRecordsModule.Core.Entities
{
    public class ProductForecast
    {
        public int CalendarMonth { get; set; }
        public DateTime? ReportDate { get; set; }
        public string ProductCategoryName { get; set; }
        public int? SalesQuantity { get; set; }
        public decimal? SalesAmount { get; set; }
    }
}
