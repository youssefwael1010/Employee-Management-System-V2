using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using  EmployeeManagementSystem.Models;
using  EmployeeManagementSystem.Common;

namespace EmployeeManagementSystem.Services
{
    public class Company
    {
        private readonly List<Employee> employees  = new();

        private readonly Dictionary<int, Department> departments  = new();

        private readonly Queue<Employee> onboardingQueue  = new();

        private readonly Stack<string> actionHistory  = new();

        private readonly HashSet<string> companySkills = new();

    }

    

}
