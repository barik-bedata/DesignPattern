# Composite Design Pattern (কম্পোজিট প্যাটার্ন)

হ্যাঁ, আপনি যে সংজ্ঞাগুলো দিয়েছেন সেগুলো ঠিক **Composite Pattern** এরই কম্পোনেন্ট! এই প্যাটার্নটি মূলত একটি "গাছ" (Tree) এর মতো স্ট্রাকচার তৈরি করতে ব্যবহৃত হয়। 

চলুন, টেকনিক্যাল কথাবার্তা বাদ দিয়ে আগে একটা গল্প শুনি।

## গল্পের মাধ্যমে Composite Pattern

ধরুন, আপনি একটি আইটি কোম্পানিতে চাকরি করেন। 
কোম্পানির গঠনটা কেমন? 
- সবার উপরে আছেন **CEO**।
- CEO এর অধীনে কয়েকজন **Manager** আছেন।
- প্রত্যেক Manager এর অধীনে আবার কয়েকজন **Developer** বা **Designer** আছেন।

এখন, কোম্পানির HR সিস্টেম যদি জানতে চায়— **"কোম্পানির মোট বেতনের খরচ কত?"**
HR কি প্রত্যেকটা ডেভেলপারের কাছে গিয়ে আলাদা করে বেতন জিজ্ঞেস করবে? না! 
HR সরাসরি **CEO** কে বলবে: *"আপনার আন্ডারে সবার মোট বেতন কত?"*
CEO তখন তার আন্ডারে থাকা **Manager** দের জিজ্ঞেস করবে।
Managers তখন তাদের আন্ডারে থাকা **Developer** দের জিজ্ঞেস করবে। 
সবশেষে সবার বেতনের যোগফল HR এর কাছে চলে আসবে। 

আবার HR চাইলে সরাসরি একজন **Developer** কেও বলতে পারে: *"তোমার বেতন কত?"* 
ডেভেলপার শুধু নিজের বেতনটাই বলে দেবে, কারণ তার আন্ডারে কেউ নেই। 

**মজার ব্যাপার হলো:** HR এর কাছে CEO, Manager বা Developer—সবাই আসলে একজন **"Employee" (কর্মচারী)**। HR সবাইকে একই নিয়মে ট্রিট করে। 

---

## এবার গল্পের সাথে কম্পোনেন্টগুলো মিলিয়ে নিই!

1. **Component (কমন ইন্টারফেস):**
   গল্পের **"Employee"** হলো Component। এটি একটি কমন ইন্টারফেস বা বেস ক্লাস। এর ভেতরে `GetSalary()` বা `ShowDetails()` এর মতো মেথড থাকে, যা সবার জন্যই কমন।
   
2. **Leaf (যার কোনো চাইল্ড নেই):**
   গল্পের **"Developer"** বা **"Designer"** হলো Leaf! এরা হলো Tree-এর একদম শেষ প্রান্তের পাতা। এদের অধীনে কেউ কাজ করে না। এদের `GetSalary()` কল করলে এরা শুধু নিজেদের বেতনটাই রিটার্ন করে।

3. **Composite (যার ভেতরে চাইল্ড আছে):**
   গল্পের **"CEO"** বা **"Manager"** হলো Composite! এরাও Employee, কিন্তু এদের অধীনে আরও Employee (Leaf বা অন্য Composite) থাকতে পারে। এদের `GetSalary()` কল করলে এরা নিজের বেতনের পাশাপাশি নিজের আন্ডারে থাকা সবার বেতন যোগ করে রিটার্ন করে। 
   এদের কাছে `AddEmployee()` বা `RemoveEmployee()` এর মতো এক্সট্রা ক্ষমতা (Methods) থাকে।

4. **Client (যিনি এই সিস্টেম ব্যবহার করছেন):**
   গল্পের **"HR সিস্টেম"** হলো Client। সে জানে না কার আন্ডারে কে আছে। সে শুধু জানে সবাই Employee। সে নিশ্চিন্তে যেকোনো Employee-এর `GetSalary()` কল করতে পারে।

---

## C# কোড দিয়ে উদাহরণ

```csharp
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
```

## মূল কথা (Summary)
Composite Pattern এর আসল ম্যাজিক হলো: **"Single Object (Leaf) এবং Group of Objects (Composite) কে একইভাবে (Uniformly) ট্রিট করা।"** 
ক্লায়েন্টের মাথা ঘামানোর দরকার নেই যে সে কোনো একজন মানুষের বেতন জানতে চাচ্ছে নাকি পুরো টিমের! সে জাস্ট `GetSalary()` কল করবে।
