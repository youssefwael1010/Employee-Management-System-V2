# 👨‍💼 Employee Management System

A C# Console Application that demonstrates **Object-Oriented Programming (OOP)** and the practical use of **.NET Collections, Generics, Delegates, and Events** through an employee management system.

The project simulates a simple company where employees, managers, departments, onboarding, employee skills, filtering, promotion, and employee lifecycle events are managed using real C# features and collections.

---

## ✨ Features

* 👨‍💼 Add Employees to the Onboarding Queue
* 🏢 Add Departments
* 📋 Show Active Employees
* 🚀 Process Employee Onboarding using FIFO Queue
* 🧠 Register Employee Skills using HashSet
* 🔍 Search Employee by ID or Name
* 🎯 Filter Employees using a Custom Delegate and Lambda Expressions
* 👔 Promote Employees to Managers
* 📢 Employee Onboarded & Promoted Events
* 🏢 Show Employees by Department
* 📊 Calculate Average Salary
* 📈 Generate Department Report
* 📚 Show Company Skills
* 📝 Show Action History using Stack (LIFO)
* 🧩 Use Generic `Result<T>` for operation results
* ✅ Input Validation
* ⚠️ Error Handling

---

## 🛠️ Technologies Used

* C#
* .NET 8
* Console Application
* Object-Oriented Programming (OOP)
* Generics
* Delegates
* Lambda Expressions
* Events
* .NET Collections

---

## 📦 Collections Used

| Collection                          | Purpose                                            |
| ----------------------------------- | -------------------------------------------------- |
| `List<Employee>`                    | Store active employees                             |
| `Dictionary<int, Department>`       | Store departments and access them by ID            |
| `Queue<Employee>`                   | Store employees waiting for onboarding using FIFO  |
| `Stack<string>`                     | Store action history with newest actions first     |
| `HashSet<string>`                   | Store unique company skills without duplicates     |
| `HashSet<string>` inside `Employee` | Store employee skills and prevent duplicate skills |

---

## 🧩 Generic Result

The project uses a custom generic `Result<T>` class to provide a unified result for operations.

```csharp
public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
```

This allows operations such as adding employees, adding departments, onboarding, and promotion to return:

* Whether the operation succeeded
* A descriptive message
* The resulting object when applicable

---

## 🎯 Delegate-Based Filtering

The project defines a custom `EmployeeFilter` delegate:

```csharp
public delegate bool EmployeeFilter(Employee employee);
```

It is used by `FilterEmployees` together with Lambda Expressions.

### Example

```csharp
company.FilterEmployees(e => e is Manager);
```

Another example:

```csharp
company.FilterEmployees(e => e.Salary > 10000m);
```

This allows different employee filtering conditions without creating a separate method for every condition.

---

## 📢 Events

The application uses **Events** to represent employee lifecycle events.

### EmployeeOnboarded

Raised when an employee is successfully processed from the onboarding queue.

### EmployeePromoted

Raised when an employee is successfully promoted to `Manager`.

The events are subscribed to in `Program.cs`, keeping the notification logic separate from the `Company` business logic.

---

## 📂 Project Structure

```text
EmployeeManagementSystem
│
├── Models
│   ├── Employee.cs
│   ├── Manager.cs
│   └── Department.cs
│
├── Common
│   └── Result.cs
│
├── Delegates
│   └── EmployeeFilter.cs
│
├── Events
│   └── EmployeeEventArgs.cs
│
├── Services
│   └── Company.cs
│
├── Program.cs
│
└── README.md
```

---

## 🖥️ Application Menu

```text
====================================
      Employee Management System
====================================

1.  Add Employee
2.  Add Department
3.  Show Employees
4.  Process Onboarding
5.  Add Skill
6.  Filter Employees
7.  Promote Employee
8.  Company Skills
9.  Search Employee
10. Department Report
11. Action History
12. Show Department Employees
13. Average Salary
0.  Exit
```

---

## 🔄 Workflow

```text
Create Department
        │
        ▼
Add Employee
        │
        ▼
Employee enters Onboarding Queue
        │
        ▼
Process Onboarding (FIFO)
        │
        ▼
Employee becomes Active
        │
        ├──────────────► EmployeeOnboarded Event
        │
        ▼
Register Skills
        │
        ▼
Search / Filter Employees
        │
        ├──────────────► Promote Employee
        │                         │
        │                         ▼
        │                  EmployeePromoted Event
        │
        ▼
Generate Reports
        │
        ▼
View Action History
```

---

## 🛡️ Validation & Error Handling

The application validates user input to prevent invalid operations and unexpected crashes.

Examples include:

* Empty employee names
* Invalid salary values
* Invalid department IDs
* Invalid employee IDs
* Employee not found
* Department not found
* Duplicate employee skills
* Invalid menu choices
* Empty onboarding queue
* Invalid promotion requests

Expected operation results are handled using the generic `Result<T>` class.

---

## ▶️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/YourUsername/Employee-Management-System.git
```

### 2. Open the Project

Open the solution using **Visual Studio 2022**.

### 3. Build

Build the solution using:

```text
Build → Build Solution
```

### 4. Run

Run the application using:

```text
Ctrl + F5
```

or:

```text
F5
```

The application will start with the console menu.

---

## 👨‍💻 Author

**Youssef Wael**
