using System;
using System.Collections.Generic;

namespace CompositePatternExample
{
    // ১. Component (সবার জন্য কমন)
    public interface IEmployee
    {
        void ShowDetails();
        int GetSalary();
    }

    // ২. Composite Interface (শুধু যাদের আন্ডারে লোক আছে তাদের জন্য - ISP Followed)
    public interface IManager : IEmployee
    {
        void AddEmployee(IEmployee employee);
    }

    // ৩. Leaf (যার আন্ডারে কেউ নেই)
    public class Developer : IEmployee
    {
        private string _name;
        private int _salary;

        public Developer(string name, int salary)
        {
            _name = name;
            _salary = salary;
        }

        public void ShowDetails() => Console.WriteLine($"Developer: {_name}");
        public int GetSalary() => _salary;
    }

    // ৪. Composite (যার আন্ডারে অন্য Employee আছে)
    public class Manager : IManager
    {
        private string _name;
        private int _salary;
        
        // Composite এর সবচেয়ে বড় বৈশিষ্ট্য: সে নিজের ভেতরে Component কে লিস্ট আকারে ধারণ করে (Has-a / Composition)
        private List<IEmployee> _subordinates = new List<IEmployee>();

        public Manager(string name, int salary)
        {
            _name = name;
            _salary = salary;
        }

        public void AddEmployee(IEmployee employee)
        {
            _subordinates.Add(employee);
        }

        public void ShowDetails()
        {
            Console.WriteLine($"\nManager: {_name}");
            Console.WriteLine("Subordinates:");
            foreach (var emp in _subordinates)
            {
                emp.ShowDetails();
            }
        }

        public int GetSalary()
        {
            int totalSalary = _salary; // নিজের বেতন
            foreach (var emp in _subordinates)
            {
                totalSalary += emp.GetSalary(); // আন্ডারে থাকা সবার বেতন যোগ হচ্ছে
            }
            return totalSalary;
        }
    }

    // ৫. Client
    class Program
    {
        static void Main()
        {
            // Leaf তৈরি করছি (IEmployee ব্যবহার করে)
            IEmployee dev1 = new Developer("Abdul Barik", 50000);
            IEmployee dev2 = new Developer("Rahim", 40000);

            // Composite তৈরি করছি (IManager ব্যবহার করে, Concrete Manager নয়!)
            IManager techManager = new Manager("Karim (Tech Lead)", 80000);
            
            // Composite এর ভেতরে Leaf ঢুকিয়ে দিচ্ছি
            techManager.AddEmployee(dev1);
            techManager.AddEmployee(dev2);

            IManager ceo = new Manager("Boss (CEO)", 150000);
            // Composite এর ভেতরে আরেকটা Composite ঢোকাচ্ছি!
            ceo.AddEmployee(techManager); 

            // Client (HR) শুধু মেথড কল করছে, সে জানে না ভেতরে কত বড় Tree আছে!
            Console.WriteLine("=== Company Details ===");
            ceo.ShowDetails();

            Console.WriteLine($"\nTotal Company Salary Expense: {ceo.GetSalary()} BDT");
        }
    }
}
