using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }= -1;
        public decimal Salary { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public HashSet<string> Skills { get; set; } = new ();



        public virtual string GetInfo()
        {
            return  $"Employee | ID:{Id} | {Name} | " +
               $"Salary:{Salary} | Department ID:{DepartmentId}";
        }

    }
}


