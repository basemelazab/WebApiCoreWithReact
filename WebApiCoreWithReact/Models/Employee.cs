using System;

namespace WebApiCoreWithReact.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName  { get; set; }
        public string Department { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
