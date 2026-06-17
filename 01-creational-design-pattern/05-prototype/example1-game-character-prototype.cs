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
            
            // সমস্যা: আমাকে ম্যানুয়ালি সব প্রপার্টি কপি করতে হচ্ছে! (Tedious & Error-prone)
            var clonedEnemy = new Enemy();
            clonedEnemy.Type = bossEnemy.Type; 
            clonedEnemy.Health = 500; 
            clonedEnemy.EnemyWeapon = new Weapon(bossEnemy.EnemyWeapon.Name); 

            bossEnemy.Display();
            clonedEnemy.Display();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.Solution
{
    using Shared;

    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface IEnemyPrototype
    {
        IEnemyPrototype ShallowCopy();
        IEnemyPrototype DeepCopy();
        void Display();
        
        // Helper methods for the client to modify stats
        void SetHealth(int health);
        void SetWeaponName(string name);
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class Enemy : IEnemyPrototype
    {
        public string Type { get; set; }
        public int Health { get; set; }
        public Weapon EnemyWeapon { get; set; }

        public void Display()
        {
            Console.WriteLine($"[Enemy] Type: {Type}, Health: {Health}, Weapon: {EnemyWeapon?.Name}");
        }

        public void SetHealth(int health) => Health = health;
        public void SetWeaponName(string name) { if (EnemyWeapon != null) EnemyWeapon.Name = name; }

        // --------------------------------------------------------
        // Shallow Copy (ভাসা ভাসা কপি - মেমোরি শেয়ার হবে)
        // --------------------------------------------------------
        public IEnemyPrototype ShallowCopy()
        {
            return (IEnemyPrototype)this.MemberwiseClone(); 
        }

        // --------------------------------------------------------
        // Deep Copy (গভীর কপি - নতুন লিস্ট/অবজেক্ট তৈরি হবে)
        // --------------------------------------------------------
        public IEnemyPrototype DeepCopy()
        {
            var clone = (Enemy)this.MemberwiseClone();
            clone.EnemyWeapon = new Weapon(this.EnemyWeapon.Name);
            return clone;
        }
    }

    // ==============================================================
    // ৩. Client (EnemySpawner)
    // ==============================================================
    public class EnemySpawner
    {
        private IEnemyPrototype _masterEnemy;

        public EnemySpawner(IEnemyPrototype masterEnemy)
        {
            _masterEnemy = masterEnemy;
        }

        public IEnemyPrototype SpawnShallowClone()
        {
            return _masterEnemy.ShallowCopy();
        }

        public IEnemyPrototype SpawnDeepClone()
        {
            return _masterEnemy.DeepCopy();
        }
    }

    // ==============================================================
    // ৪. Main Class (SolutionRunner)
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Prototype Pattern (Using 4 Components) ===");

            // ১. মাস্টার এনিমি তৈরি করা
            var masterEnemy = new Enemy { Type = "Goblin", Health = 100, EnemyWeapon = new Weapon("Dagger") };
            Console.WriteLine("\n--- Original Master ---");
            masterEnemy.Display();

            // ২. ক্লায়েন্টকে মাস্টার কপি দিয়ে দেওয়া
            var spawner = new EnemySpawner(masterEnemy);

            // ⚠️ Shallow Copy Test
            var shallowClone = spawner.SpawnShallowClone();
            shallowClone.SetHealth(50); 
            shallowClone.SetWeaponName("Wooden Stick"); // ❌ শ্যালো কপিতে মাস্টারও পালটে যাবে!

            Console.WriteLine("\n--- After Shallow Clone changed Weapon to 'Wooden Stick' ---");
            Console.WriteLine("Original Master:");
            masterEnemy.Display(); 
            Console.WriteLine("Shallow Clone:");
            shallowClone.Display();

            // 🔄 অরিজিনাল উইপন ঠিক করে নিচ্ছি
            masterEnemy.SetWeaponName("Dagger");

            // ✅ Deep Copy Test
            var deepClone = spawner.SpawnDeepClone();
            deepClone.SetHealth(80);
            deepClone.SetWeaponName("Iron Sword"); // ✅ ডিপ কপিতে মাস্টারের কোনো সমস্যা হবে না!

            Console.WriteLine("\n--- After Deep Clone changed Weapon to 'Iron Sword' ---");
            Console.WriteLine("Original Master:");
            masterEnemy.Display(); 
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
