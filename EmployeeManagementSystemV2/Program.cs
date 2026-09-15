using System.Collections.Generic;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Delegates;
using EmployeeManagementSystem.Events;


namespace EmployeeManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Company company = new Company();
            company.EmployeeOnBoarded += OnEmployeeOnBoarded;
            company.EmployeePromoted += OnEmployeePromoted; ;

            company.SeedData();

            bool exit = false;

            do
            {

                ShowMenu();

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case 1:

                            Console.Write("Employee Name: ");
                            string empName = Console.ReadLine()!;


                            Console.Write("Salary: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal empSalary))
                            {
                                Console.WriteLine("Invalid number.");
                                break;
                            }

                            Console.Write("Department id: ");

                            if (!int.TryParse(Console.ReadLine(), out int empDeptId))
                            {
                                Console.WriteLine("Invalid number.");
                                break;
                            }

                            var result=company.AddEmployeeToOnboarding(new Employee { Salary= empSalary,Name = empName!, DepartmentId= empDeptId });
                            if (result.Success)
                                Console.WriteLine(result.Message);
                            else
                                Console.WriteLine($"Error: {result.Message}");

                            break;

                        case 2:

                            Console.Write("Name: ");
                            string deptname = Console.ReadLine()!;

                            var depResult=company.AddDepartment(new Department { Name= deptname! });
                            if (depResult.Success)
                                Console.WriteLine(depResult.Message);
                            else
                                Console.WriteLine($"Error: {depResult.Message}");
                            break;

                        case 3:

                            var Allemployees = company.GetEmployees();

                            if (Allemployees.Count == 0)
                            {
                                Console.WriteLine("No employees found.");
                            }
                            else
                            {
                                foreach (Employee emp in Allemployees)
                                    Console.WriteLine(emp.GetInfo());
                            }

                            break;

                        case 4:

                            var boardRes= company.ProcessNextOnboarding();
                            if (boardRes.Success)
                                Console.WriteLine(boardRes.Message);
                            else
                                Console.WriteLine($"Error: {boardRes.Message}");
                            break;

                        case 5:

                            Console.Write("Skill: ");
                            string skill = Console.ReadLine()!;

                            Console.Write("Employee id: ");
                            if (!int.TryParse(Console.ReadLine(), out int EmpID))
                            {
                                Console.WriteLine("Invalid number.");
                                break;
                            }

                            company.AddSkillToEmployee(EmpID, skill);
                            Console.WriteLine("Skill Added Successfully.");

                            break;

                        case 6:
                            Console.WriteLine("Managers");
                            var managers = company.FilterEmployees(e=> e is Manager);

                            if (managers.Count == 0)
                            {
                                Console.WriteLine("No managers found.");
                            }
                            else
                            {
                                foreach (Employee emp in managers)
                                {
                                    Console.WriteLine(emp.GetInfo());
                                }
                            }

                            
                            Console.WriteLine("------------------------------------------------------------");
                            Console.WriteLine("Employees with Salary > 10000");
                            var highSalaryEmployees = company.FilterEmployees(e => e.Salary > 10000m);

                            if (highSalaryEmployees.Count == 0)
                            {
                                Console.WriteLine("No employees found.");
                            }
                            else
                            {
                                foreach (Employee emp in highSalaryEmployees)
                                {
                                    Console.WriteLine(emp.GetInfo());
                                }
                            }

                            break;

                        case 7:

                            Console.Write("Employee id: ");
                            if (!int.TryParse(Console.ReadLine(), out int empID))
                            {
                                Console.WriteLine("Invalid number.");
                                break;
                            }

                            var promoRes=company.PromoteToManager(empID);
                            if (promoRes.Success)
                                Console.WriteLine(promoRes.Message);
                            else
                                Console.WriteLine($"Error: {promoRes.Message}");
                            break;

                        case 8:

                            Console.WriteLine("Company Skills:");

                            var Skills = company.GetCompanySkills();
                            if (Skills.Count == 0)
                            {
                                Console.WriteLine("No skills found.");
                            }
                            else
                            {
                                foreach (var Skill in Skills)
                                    Console.WriteLine(Skill);
                            }
                                break;

                        case 9:

                            Console.Write("Search by (1-ID / 2-Name): ");

                            if (!int.TryParse(Console.ReadLine(), out int option))
                            {
                                Console.WriteLine("Invalid option.");
                                break;
                            }
                            Employee? employee=null;
                            if (option == 1)
                            {
                                Console.Write("Employee ID: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    Console.WriteLine("Invalid ID.");
                                    break;
                                }

                                employee =company.FindEmployeeById(id);
                                Console.WriteLine(employee?.GetInfo());
                            }
                            else if (option == 2)
                            {
                                Console.Write("Employee Name: ");

                                string name = Console.ReadLine()!;

                                employee=company.FindEmployeeByName(name);
                                Console.WriteLine(employee?.GetInfo());
                            }
                            else
                            {
                                Console.WriteLine("Invalid option.");
                            }
                            break;

                        case 10:

                            var report= company.GetDepartmentReport();

                            Console.WriteLine("Department Report:");

                            foreach (var item in report)
                            {
                                Console.WriteLine($"{item.Key}: {item.Value} employees");
                            }

                            break;

                        case 11:

                            Console.WriteLine("Action History:");
                            var actions = company.GetActionHistory();
                            foreach (var action in actions)
                                Console.WriteLine(action);
                            break;

                        case 12:

                            Console.Write("Employee ID: ");

                            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                            {
                                Console.WriteLine("Invalid Employee ID.");
                                break;
                            }

                            Console.Write("Manager ID: ");

                            if (!int.TryParse(Console.ReadLine(), out int managerId))
                            {
                                Console.WriteLine("Invalid Manager ID.");
                                break;
                            }

                            var assignResult =
                                company.AssignEmployeeToManager(employeeId, managerId);

                            Console.WriteLine(assignResult.Message);

                            break;

                        case 13:

                            Console.Write("Manager ID: ");

                            if (!int.TryParse(Console.ReadLine(), out int mngId))
                            {
                                Console.WriteLine("Invalid Manager ID.");
                                break;
                            }

                            var team = company.GetManagerTeam(mngId);

                            if (team.Count == 0)
                            {
                                Console.WriteLine("No team members found.");
                                break;
                            }

                            Console.WriteLine("Team Members:");

                            foreach (Employee emp in team)
                            {
                                Console.WriteLine(emp.GetInfo());
                            }

                            break;

                        case 14:

                            Console.Write("Department id: ");
                            if (!int.TryParse(Console.ReadLine(), out int deptId))
                            {
                                Console.WriteLine("Invalid number.");
                                break;
                            }

                            var empsDepartment = company.GetEmployeesByDepartment(deptId);
                            foreach (var employee1 in empsDepartment)
                                Console.WriteLine(employee1.GetInfo());
                            break;

                        case 15:

                            Console.Write("Average Salary = ");
                            var avgSalary = company.CalculateAverageSalary();
                           
                             Console.WriteLine(avgSalary);
                            break;

                        case 0:

                            exit = true;

                            break;

                        default:

                            Console.WriteLine("Invalid choice.");

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }

            }
            while (!exit);

            Console.WriteLine("Bye Bye");

        }
        private static void OnEmployeePromoted(object? sender, EmployeeEventArgs e)
        {
            Console.WriteLine($"Employee with Id {e.Employee.Id} has been Promoted by Board.");
        }
        private static void OnEmployeeOnBoarded(object? sender, EmployeeEventArgs e)
        {
            Console.WriteLine($"Employee with Id {e.Employee.Id} has been OnBoarded and ready to work.");
        }

        private static void ShowMenu()
        {

            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine("      Employee Management System");
            Console.WriteLine("====================================");
            Console.WriteLine("1.  Add Employee"); 
            Console.WriteLine("2.  Add Department"); 
            Console.WriteLine("3.  Show Employees"); 
            Console.WriteLine("4.  Process Onboarding");
            Console.WriteLine("5.  Add Skill");
            Console.WriteLine("6.  Filter Employees");
            Console.WriteLine("7.  Promote Employee");
            Console.WriteLine("8.  Company Skills");
            Console.WriteLine("9.  Search Employee"); 
            Console.WriteLine("10. Department Report");
            Console.WriteLine("11. Action History");
            Console.WriteLine("12. Assign Employee To Manager");
            Console.WriteLine("13. Show Manager Team");
            Console.WriteLine("14. Show Department Employees");
            Console.WriteLine("15. Average Salary");
            Console.WriteLine("0.  Exit");
            Console.WriteLine();
            Console.Write("Choose: ");
        }

}


}
