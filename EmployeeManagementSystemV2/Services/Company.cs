using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using  EmployeeManagementSystem.Delegates;
using  EmployeeManagementSystem.Models;
using  EmployeeManagementSystem.Events;
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
        
        public event EventHandler<EmployeeEventArgs>? EmployeeOnBoarded;

        public event EventHandler<EmployeeEventArgs>? EmployeePromoted;

        // helper methods
        public Employee? FindEmployeeById(int id)
        {
            foreach (Employee employee in employees) 
            {
                if(employee.Id == id)
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
        private bool EmployeeIdExists(int id)
                {
                    foreach (Employee emp in employees)
                    {
                        if (emp.Id == id) return true;
                    }

                    foreach (Employee emp in onboardingQueue)
                    {
                        if (emp.Id == id) return true;
                    }

                    return false;
                }
       
        
        public Result<Employee> AddEmployeeToOnboarding(Employee employee)
        {
            if (employee is null)
                return Result<Employee>.Failure("Employee not found.");

            if (string.IsNullOrWhiteSpace(employee.Name))
                return Result<Employee>.Failure("Employee name is required.");

            if (employee.Salary <= 0)
                return Result<Employee>.Failure("Salary must be greater than zero.");
   
            if (!departments.ContainsKey(employee.DepartmentId))
                return Result<Employee>.Failure($"Department Id {employee.DepartmentId} does not exist.");
     
            if (EmployeeIdExists(employee.Id))
                return Result<Employee>.Failure($"Employee Id {employee.Id} already exists.");

            int maxId = 0;
            foreach (var emp in employees)
            {
                if (emp.Id > maxId) maxId = emp.Id;
            }
            foreach (var emp in onboardingQueue)
            {
                if (emp.Id > maxId) maxId = emp.Id;
            }

            employee.Id = maxId + 1;

            onboardingQueue.Enqueue(employee);
            actionHistory.Push($"Employee added to onboarding: {employee.Name}");

            return Result<Employee>.SuccessResult(employee, "Employee added Successfully");
   

        }
        public Result<Department> AddDepartment(Department department)
        {
            if (department is null)
               
                return Result<Department>.Failure($"{nameof(department)} is required.");

            if (string.IsNullOrWhiteSpace(department.Name))
                
                return Result<Department>.Failure("Department name is required.");

            if (departments.ContainsKey(department.Id))
             
                return  Result<Department>.Failure($"Department Id {department.Id} already exists.");

            departments.Add(department.Id, department);

            actionHistory.Push($"Added department: {department.Name}");

            return Result<Department>.SuccessResult(department, "Department added Successfully.");
          

        }
        public Result<Employee> ProcessNextOnboarding() 
        {
            if (onboardingQueue.Count == 0)
                return Result<Employee>.Failure("No employees waiting for onboarding.");
             
            Employee employee = onboardingQueue.Dequeue();



            employees.Add(employee);

            if (EmployeeOnBoarded is not null)
            {
                EmployeeOnBoarded(this , new EmployeeEventArgs(employee));
            }
            

            actionHistory.Push( $"Employee activated: {employee.Name}");
            return Result<Employee>.SuccessResult(employee, "Employee OnBoarded Successfully.");
            
        }
        public Result<Employee> AssignEmployeeToManager(int employeeId, int managerId)
        {
            Employee? employee = FindEmployeeById(employeeId);

            if (employee is null)
                return Result<Employee>.Failure(
                    $"Employee with Id {employeeId} was not found.");


            Employee? managerEmployee = FindEmployeeById(managerId);

            if (managerEmployee is null)
            {
                return Result<Employee>.Failure(
                    $"Manager with Id {managerId} was not found.");
            }

            if (managerEmployee is not Manager manager)
            {
                return Result<Employee>.Failure(
                    "The selected employee is not a manager.");
            }


            if (employee.Id == manager.Id)
            {
                return Result<Employee>.Failure(
                    "A manager cannot be assigned to himself.");
            }



            foreach (Employee teamMember in manager.TeamMembers)
            {
                if (teamMember.Id== employee.Id)
                {
                    return Result<Employee>.Failure(
                        "Employee is already assigned to this manager.");
                }
            }
            manager.TeamMembers.Add(employee);

            actionHistory.Push(
                 $"Assigned {employee.Name} to manager {manager.Name}");

            return Result<Employee>.SuccessResult(
                employee,
                $"Employee {employee.Name} assigned to manager {manager.Name}.");

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

        public Dictionary<string, int> GetDepartmentReport()  
        {
            if (departments.Count == 0)
                throw new InvalidOperationException("No departments found.");

            Dictionary<string, int> report = new();

            foreach (KeyValuePair<int, Department> pair in departments)
            {
                int employeeCount = 0;
                foreach (Employee employee in employees)
                {
                    if (employee.DepartmentId == pair.Key)
                    {
                        employeeCount++;
                    }
                }
                report.Add(pair.Value.Name, employeeCount);
            }
            return report;
        }
        public Stack<string> GetActionHistory() 
        {
            return actionHistory;
        } 
        public decimal CalculateAverageSalary()
        {
            if (employees.Count == 0)
                return 0;

            decimal totalSalary = 0;

            foreach (var employee in employees)
              totalSalary += employee.Salary;

            return Math.Round(totalSalary / employees.Count,2);
        }
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            Department? department = FindDepartmentById(departmentId);

            if (department is null)
                throw new InvalidOperationException($"Department with Id {departmentId} was not found.");

            List < Employee> empsDepartment = new ();

            foreach (var employee in employees)
            {
                if(employee.DepartmentId == departmentId)
                    empsDepartment.Add(employee);

            }

            return empsDepartment;
        }
        public Result<Manager> PromoteToManager(int employeeId)
        {

            Employee? employee = FindEmployeeById(employeeId);

            if (employee is null)
                return Result<Manager>.Failure("Employee not found.");
              

            var manager = new Manager
            {
                Id = employeeId,
                Name = employee.Name,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                Skills = new HashSet<string>(employee.Skills)
            };

            int index = employees.IndexOf(employee);

            employees[index] = manager;

            actionHistory.Push($"Employee promoted: {manager.Name}");

            if (EmployeePromoted is not null)
                EmployeePromoted(this, new EmployeeEventArgs(manager));


            return Result<Manager>.SuccessResult(manager, "Employee Promoted Successfully");
           
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
        public List<Employee> GetEmployees()
        {
            List<Employee> result = new();

            foreach (Employee employee in employees)
                result.Add(employee);

            return result;
        }

        public List<Employee> GetManagerTeam(int managerId)
        {
            Employee? employee = FindEmployeeById(managerId);

            if (employee is null)
                return new List<Employee>();

            if (employee is not Manager manager)
                return new List<Employee>();

            return manager.TeamMembers;
        }


        public void SeedData()
        {

            AddDepartment(new Department { Id = 1, Name = "IT" });
            AddDepartment(new Department { Id = 2, Name = "HR" });
            AddDepartment(new Department { Id = 3, Name = "Finance" });
            AddDepartment(new Department { Id = 4, Name = "Marketing" });



            AddEmployeeToOnboarding(new Employee { Name = "Ahmed", HireDate = DateTime.Now.AddMonths(-6), DepartmentId = 1, Salary = 12000 });
            AddEmployeeToOnboarding(new Employee { Name = "Ali", HireDate = DateTime.Now.AddMonths(-4), DepartmentId = 2, Salary = 8000 });
            AddEmployeeToOnboarding(new Employee { Name = "Sara", HireDate = DateTime.Now.AddMonths(-5), DepartmentId = 1, Salary = 15000 });
            AddEmployeeToOnboarding(new Employee { Name = "Omar", HireDate = DateTime.Now.AddMonths(-2), DepartmentId = 3, Salary = 9500 });
            AddEmployeeToOnboarding(new Employee { Name = "Mona", HireDate = DateTime.Now.AddMonths(-1), DepartmentId = 4, Salary = 11000 });




            //AddEmployeeToOnboarding(new Employee {Id = 1,Name = "Ahmed",HireDate = DateTime.Now,DepartmentId = 1,Salary = 15000});
            //AddEmployeeToOnboarding(new Employee {Id = 2,Name = "Ali"  ,HireDate = DateTime.Now,DepartmentId = 2,Salary = 7000 });
            //AddEmployeeToOnboarding(new Employee {Id = 3,Name = "Sara" ,HireDate = DateTime.Now,DepartmentId = 1,Salary = 15000});

            ProcessNextOnboarding();
            ProcessNextOnboarding();
            ProcessNextOnboarding();


            AddSkillToEmployee(1, "C#");
            AddSkillToEmployee(1, ".NET");
            AddSkillToEmployee(1, "SQL");

            AddSkillToEmployee(2, "Communication");
            AddSkillToEmployee(2, "Excel");

            AddSkillToEmployee(3, "Angular");
            AddSkillToEmployee(3, "C#");


            PromoteToManager(1);

            AssignEmployeeToManager(2, 1); 
            AssignEmployeeToManager(3, 1);


        }

    }

    

}
