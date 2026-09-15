using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Events
{
  
    public class EmployeeEventArgs : EventArgs
    {
       public Employee Employee { get; set; }

        public EmployeeEventArgs (Employee employee)
        {
            Employee = employee;

        }
    }
}
