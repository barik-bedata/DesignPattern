using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Visitor.Organization
{
    // ==========================================
    // 1. Visitor Interface
    // ==========================================
    public interface IEmployeeVisitor
    {
        void Visit(RegularEmployee employee);
        void Visit(Manager manager);
        void Visit(Director director);
    }

    // ==========================================
    // 3. Element Interface
    // ==========================================
    public interface IEmployee
    {
        void Accept(IEmployeeVisitor visitor);
    }

    // ==========================================
    // 4. Concrete Elements
    // ==========================================
    // এমপ্লয়ি ক্লাসগুলোতে শুধু তাদের নিজেদের প্রপার্টি আছে। 
    // বোনাস বা অ্যালাউন্স হিসাব করার কোনো মেথড এখানে নেই।
    public class RegularEmployee : IEmployee
    {
        public string Name { get; }
        public double MonthlySalary { get; }

        public RegularEmployee(string name, double salary)
        {
            Name = name;
            MonthlySalary = salary;
        }

        public void Accept(IEmployeeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Manager : IEmployee
    {
        public string Name { get; }
        public double MonthlySalary { get; }
        public int TeamSize { get; }

        public Manager(string name, double salary, int teamSize)
        {
            Name = name;
            MonthlySalary = salary;
            TeamSize = teamSize;
        }

        public void Accept(IEmployeeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Director : IEmployee
    {
        public string Name { get; }
        public double MonthlySalary { get; }

        public Director(string name, double salary)
        {
            Name = name;
            MonthlySalary = salary;
        }

        public void Accept(IEmployeeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // ==========================================
    // 2. Concrete Visitors
    // ==========================================
    // বোনাস হিসাব করার ভিজিটর
    public class BonusCalculatorVisitor : IEmployeeVisitor
    {
        public void Visit(RegularEmployee employee)
        {
            // 1 month salary as bonus
            double bonus = employee.MonthlySalary;
            Console.WriteLine($"[Bonus] {employee.Name} (Employee) receives ${bonus:F2}");
        }

        public void Visit(Manager manager)
        {
            // 1.5 month salary + $100 per team member
            double bonus = (manager.MonthlySalary * 1.5) + (manager.TeamSize * 100);
            Console.WriteLine($"[Bonus] {manager.Name} (Manager) receives ${bonus:F2}");
        }

        public void Visit(Director director)
        {
            // 3 months salary as bonus
            double bonus = director.MonthlySalary * 3;
            Console.WriteLine($"[Bonus] {director.Name} (Director) receives ${bonus:F2}");
        }
    }

    // মেডিকেল সুবিধা নির্ধারণ করার ভিজিটর
    public class MedicalAllowanceVisitor : IEmployeeVisitor
    {
        public void Visit(RegularEmployee employee)
        {
            Console.WriteLine($"[Medical] {employee.Name} gets Basic Health Coverage.");
        }

        public void Visit(Manager manager)
        {
            Console.WriteLine($"[Medical] {manager.Name} gets Family Health Coverage.");
        }

        public void Visit(Director director)
        {
            Console.WriteLine($"[Medical] {director.Name} gets Premium Global Health Coverage.");
        }
    }

    // ==========================================
    // 5. Object Structure / Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Visitor Pattern (Organization System) ===\n");

            List<IEmployee> company = new List<IEmployee>
            {
                new RegularEmployee("John Doe", 3000),
                new Manager("Alice Smith", 6000, 5),
                new Director("Bob Boss", 15000)
            };

            BonusCalculatorVisitor bonusVisitor = new BonusCalculatorVisitor();
            MedicalAllowanceVisitor medicalVisitor = new MedicalAllowanceVisitor();

            Console.WriteLine("--- Distributing Annual Bonus ---");
            foreach (var emp in company)
            {
                emp.Accept(bonusVisitor);
            }

            Console.WriteLine("\n--- Assigning Medical Allowance ---");
            foreach (var emp in company)
            {
                emp.Accept(medicalVisitor);
            }
        }
    }
}
