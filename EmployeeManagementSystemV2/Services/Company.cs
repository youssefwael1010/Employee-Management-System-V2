using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using  EmployeeManagementSystem.Models;
using  EmployeeManagementSystem.Common;
using EmployeeManagementSystem.Delegates;
using EmployeeManagementSystem.Events;

namespace EmployeeManagementSystem.Services
{
    public class Company
    {
        private readonly List<Employee> employees  = new();

        private readonly Dictionary<int, Department> departments  = new();

        private readonly Queue<Employee> onboardingQueue  = new();

        private readonly Stack<string> actionHistory  = new();

        private readonly HashSet<string> companySkills = new();

        public event EventHandler<EmployeeEventArgs>? EmployeeOnBoarded;

        public event EventHandler<EmployeeEventArgs>? EmployeePromoted

        // helper methods
        public Employee? FindEmployeeById(int id)
        {
            foreach (Employee employee in employees)
            {
                if (employee.Id == id)
                    return employee;
            }
            return null;
        }
        public Employee? FindEmployeeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (Employee employee in employees)
            {
                if (employee.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return employee;
            }

            return null;
        }
        private Department? FindDepartmentById(int id)
        {
            if (departments.TryGetValue(id, out Department? department))
                return department;

            return null;
        }

        //
        //

        public Result<Employee> AddEmployeeToOnboarding(Employee employee)
        {
            if (employee is null)
                return new Result<Employee>
                {
                    Success = false,
                    Message = "Employee not found.",
                    Data = null
                };

            if (string.IsNullOrWhiteSpace(employee.Name))
                return new Result<Employee>
                {
                    Success = false,
                    Message = "Employee name is required.",
                    Data = null
                };
            //throw new Exception("Employee name is required.");

            if (employee.Salary < 0)
                return new Result<Employee>
                {
                    Success = false,
                    Message = "Salary cannot be negative.",
                    Data = null
                };
            //throw new Exception("Salary cannot be negative.");

            if (!departments.ContainsKey(employee.DepartmentId))
                return new Result<Employee>
                {
                    Success = false,
                    Message = $"Department Id {employee.DepartmentId} does not exist.",
                    Data = null
                };
            //throw new InvalidOperationException($"Department Id {employee.DepartmentId} does not exist.");

            if (FindEmployeeById(employee.Id) is not null)
                return new Result<Employee>
                {
                    Success = false,
                    Message = $"Employee Id {employee.Id} already exists.",
                    Data = null
                };
            //throw new InvalidOperationException($"Employee Id {employee.Id} already exists.");


            onboardingQueue.Enqueue(employee);
            actionHistory.Push($"Employee added to onboarding: {employee.Name}");

            return new Result<Employee>
            {
                Success = true,
                Message = "Employee added Successfully",
                Data = employee
            };

        }
        public Result<Department> AddDepartment(Department department)
        {
            if (department is null)
                //throw new ArgumentNullException(nameof(department));
                return new Result<Department>
                {
                    Success = false,
                    Message = $"{nameof(department)} is required."
                };

            if (string.IsNullOrWhiteSpace(department.Name))
                //throw new Exception("Department name is required.");
                return new Result<Department>
                {
                    Success = false,
                    Message = "Department name is required.",
                    Data = null
                };

            if (departments.ContainsKey(department.Id))
                //throw new InvalidOperationException($"Department Id {department.Id} already exists.");
                return new Result<Department>
                {
                    Success = false,
                    Message = $"Department Id {department.Id} already exists.",
                    Data = null
                };

            departments.Add(department.Id, department);

            actionHistory.Push($"Added department: {department.Name}");

            return new Result<Department>
            {
                Success = true,
                Message = "Department added Successfully.",
                Data = department
            };

        }

        public Result<Employee> ProcessNextOnboarding()
        {
            if (onboardingQueue.Count == 0)
                //throw new InvalidOperationException("No employees waiting for onboarding.");
                return new Result<Employee>
                {
                    Success = false,
                    Message = "No employees waiting for onboarding.",
                    Data = null
                };
            Employee employee = onboardingQueue.Dequeue();



            employees.Add(employee);

            if (EmployeeOnBoarded is not null)
            {
                EmployeeOnBoarded(this, new EmployeeEventArgs(employee));
            }


            actionHistory.Push($"Employee activated: {employee.Name}");
            return new Result<Employee>
            {
                Success = true,
                Message = "Employee OnBoarded Successfully.",
                Data = employee
            };
        }
        public void AddSkillToEmployee(int employeeId, string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                throw new ArgumentException("Skill name is required.");

            Employee? employee = FindEmployeeById(employeeId);

            if (employee is null)
                throw new InvalidOperationException($"Employee with Id {employeeId} was not found.");

            string normalizedSkill = skill.Trim();

            if (!employee.Skills.Contains(normalizedSkill))
                employee.Skills.Add(normalizedSkill);

            companySkills.Add(normalizedSkill);
            actionHistory.Push($"Added skill {normalizedSkill} to {employee.Name}");

        }
        public HashSet<string> GetCompanySkills()
        {
            return companySkills;
        }

        public List<Employee> FilterEmployees(EmployeeFilter filter)
        {

            List<Employee> result = new();
            foreach (var employee in employees)
            {
                if (filter(employee))
                {
                    result.Add(employee);
                }
            }
            return result;

        }

        public Result<Manager> PromoteToManager(int employeeId)
        {

            Employee? employee = FindEmployeeById(employeeId);

            if (employee is null)
                return new Result<Manager>
                {
                    Success = false,
                    Message = "Employee not found.",
                    Data = null
                };

            var manager = new Manager
            {
                Id = employeeId,
                Name = employee.Name,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                Skills = employee.Skills
            };

            int index = employees.IndexOf(employee);

            employees[index] = manager;

            actionHistory.Push($"Employee promoted: {manager.Name}");

            if (EmployeePromoted is not null)
                EmployeePromoted(this, new EmployeeEventArgs(manager));


            return new Result<Manager>
            {
                Success = true,
                Message = "Employee Promoted Successfully",
                Data = manager
            };
        }

    }

    

}
