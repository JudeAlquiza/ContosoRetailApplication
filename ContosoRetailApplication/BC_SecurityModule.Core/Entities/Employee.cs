using System;
using System.Collections.Generic;

namespace BC_SecurityModule.Core.Entities
{
    public class Employee
    {
        public Employee()
        {
            DimEmployee1 = new HashSet<Employee>();
        }

        public int EmployeeKey { get; set; }
        public int? ParentEmployeeKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string MaritalStatus { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public bool? SalariedFlag { get; set; }
        public string Gender { get; set; }
        public byte? PayFrequency { get; set; }
        public decimal? BaseRate { get; set; }
        public short? VacationHours { get; set; }
        public bool CurrentFlag { get; set; }
        public bool SalesPersonFlag { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int? ETLLoadID { get; set; }
        public DateTime? LoadDate { get; set; }  
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<BC_SecurityModule.Core.Entities.Employee> DimEmployee1 { get; set; }
        public virtual BC_SecurityModule.Core.Entities.Employee DimEmployee2 { get; set; }
    }
}
