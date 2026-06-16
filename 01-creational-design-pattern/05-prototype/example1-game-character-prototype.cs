using System;

// ==============================================================
// 📦 Shared Models
// ==============================================================
namespace PrototypePattern.Shared
{
    // এটি একটি Reference Type প্রপার্টি বোঝানোর জন্য।
    public class Weapon
    {
        public string Name { get; set; }
        public Weapon(string name) { Name = name; }
    }
}

// ==============================================================
// ❌ VIOLATION: The Bad Way (ম্যানুয়ালি কপি করা)
// ==============================================================
// সমস্যা: একটি অবজেক্টের হুবহু কপি বানাতে চাইলে আমাদের প্রতিটি প্রপার্টি
// ম্যানুয়ালি ধরে ধরে কপি করতে হয়। অবজেক্টে যদি ৫০টি প্রপার্টি থাকে, 
// তবে ৫০ লাইন কোড লিখতে হবে শুধু কপি করার জন্যই!

namespace PrototypePattern.Violation
{
    using Shared;

    public class Enemy
    {
        public string Type { get; set; }
        public int Health { get; set; }
        public Weapon EnemyWeapon { get; set; }

        public void Display()
        {
            Console.WriteLine($"[Enemy] Type: {Type}, Health: {Health}, Weapon: {EnemyWeapon.Name}");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: ম্যানুয়াল কপি ===");
            
            // অরিজিনাল এনিমি
            var bossEnemy = new Enemy { Type = "Orc Boss", Health = 1000, EnemyWeapon = new Weapon("Axe") };
            
            // এখন আমি চাই বস এনিমির হুবহু একটা ক্লোন বানাতে, শুধু হেলথ একটু কম হবে।
            // সমস্যা: আমাকে ম্যানুয়ালি সব প্রপার্টি কপি করতে হচ্ছে! (Tedious & Error-prone)
            var clonedEnemy = new Enemy();
            clonedEnemy.Type = bossEnemy.Type; 
            clonedEnemy.Health = 500; // শুধু এটা চেঞ্জ করলাম
            clonedEnemy.EnemyWeapon = new Weapon(bossEnemy.EnemyWeapon.Name); // রেফারেন্স টাইপ কপি করা আরও ঝামেলার!

            bossEnemy.Display();
            clonedEnemy.Display();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (Using Prototype Pattern)
// ==============================================================
// সমাধান: অবজেক্ট নিজেই নিজেকে কপি (Clone) করার ক্ষমতা রাখবে। 
// C# এ ICloneable ইন্টারফেস ব্যবহার করা যায়, অথবা নিজস্ব Clone মেথড বানানো যায়।

namespace PrototypePattern.Solution
{
    using Shared;

    // ১. Prototype Interface (অথবা শুধু একটি Clone মেথড রাখলেও হয়)
    public interface IEnemyPrototype
    {
        IEnemyPrototype ShallowCopy();
        IEnemyPrototype DeepCopy();
    }

    // ২. Concrete Prototype
    public class Enemy : IEnemyPrototype
    {
        public string Type { get; set; }
        public int Health { get; set; }
        public Weapon EnemyWeapon { get; set; }

        public void Display()
        {
            Console.WriteLine($"[Enemy] Type: {Type}, Health: {Health}, Weapon: {EnemyWeapon?.Name}");
        }

        // --------------------------------------------------------
        // Shallow Copy (ভাসা ভাসা কপি)
        // ভ্যালু টাইপ (int, string) কপি হবে, কিন্তু রেফারেন্স টাইপ (Weapon) 
        // কপি হবে না, মেমোরি অ্যাড্রেস শেয়ার করবে!
        // --------------------------------------------------------
        public IEnemyPrototype ShallowCopy()
        {
            // MemberwiseClone() C# এর বিল্ট-ইন মেথড যা শ্যালো কপি করে।
            return (IEnemyPrototype)this.MemberwiseClone(); 
        }

        // --------------------------------------------------------
        // Deep Copy (গভীর কপি)
        // ভ্যালু এবং রেফারেন্স টাইপ— সবকিছু একদম নতুন করে মেমোরিতে কপি হবে।
        // --------------------------------------------------------
        public IEnemyPrototype DeepCopy()
        {
            var clone = (Enemy)this.MemberwiseClone();
            // রেফারেন্স টাইপকে ম্যানুয়ালি নতুন করে ইনস্ট্যান্স বানিয়ে কপি করতে হবে
            clone.EnemyWeapon = new Weapon(this.EnemyWeapon.Name);
            return clone;
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Prototype Pattern (Shallow vs Deep Copy) ===");

            var originalEnemy = new Enemy { Type = "Goblin", Health = 100, EnemyWeapon = new Weapon("Dagger") };
            Console.WriteLine("\n--- Original ---");
            originalEnemy.Display();

            // ⚠️ Shallow Copy Test
            var shallowClone = (Enemy)originalEnemy.ShallowCopy();
            shallowClone.Health = 50; 
            shallowClone.EnemyWeapon.Name = "Wooden Stick"; // ❌ শ্যালো কপিতে উইপন চেঞ্জ করলে অরিজিনালটারও চেঞ্জ হয়ে যাবে!

            Console.WriteLine("\n--- After Shallow Clone changed Weapon to 'Wooden Stick' ---");
            Console.WriteLine("Original:");
            originalEnemy.Display(); // অরিজিনাল উইপনও "Wooden Stick" হয়ে গেছে! (কারন মেমোরি রেফারেন্স সেম)
            Console.WriteLine("Shallow Clone:");
            shallowClone.Display();

            // 🔄 অরিজিনাল উইপন ঠিক করে নিচ্ছি
            originalEnemy.EnemyWeapon.Name = "Dagger";

            // ✅ Deep Copy Test
            var deepClone = (Enemy)originalEnemy.DeepCopy();
            deepClone.Health = 80;
            deepClone.EnemyWeapon.Name = "Iron Sword"; // ✅ ডিপ কপিতে উইপন চেঞ্জ করলে অরিজিনালটার কোনো সমস্যা হবে না!

            Console.WriteLine("\n--- After Deep Clone changed Weapon to 'Iron Sword' ---");
            Console.WriteLine("Original:");
            originalEnemy.Display(); // অরিজিনাল উইপন "Dagger" ই আছে! (কারন সম্পূর্ণ নতুন মেমোরি)
            Console.WriteLine("Deep Clone:");
            deepClone.Display();
        }
    }
}

// ══════════════════════════════════════════
// 🚀 Main Entry Point
// ══════════════════════════════════════════
class Program
{
    static void Main()
    {
        PrototypePattern.Violation.ViolationRunner.Run();
        PrototypePattern.Solution.SolutionRunner.Run();
    }
}
