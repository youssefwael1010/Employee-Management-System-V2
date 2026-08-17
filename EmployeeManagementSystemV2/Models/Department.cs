using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


        public override string ToString()
        {
            return $"Department: {Name} - ID: {Id}";
        }

    }
}
