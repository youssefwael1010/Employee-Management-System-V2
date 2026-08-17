using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Models
{
    public class Manager : Employee
    {
        public List<Employee> TeamMembers { get; set; } = new List<Employee>();

        public override string GetInfo()
        {
            return $"Manager  | ID:{Id} | {Name} | Salary:{Salary} | Department ID:{DepartmentId} | Team Members:{TeamMembers.Count}";
        }

    }
}
