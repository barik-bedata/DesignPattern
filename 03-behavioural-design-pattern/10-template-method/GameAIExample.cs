using System;

namespace BehavioralDesignPattern.TemplateMethod.GameAI
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // গেমের AI কীভাবে কাজ করবে, তার একটি কমন ব্লু-প্রিন্ট।
    public abstract class GameAI
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // এই মেথডটি AI-এর কাজের নির্দিষ্ট সিকোয়েন্স (আগে রিসোর্স কালেক্ট, তারপর বিল্ডিং, তারপর ইউনিট, শেষে অ্যাটাক) ফিক্স করে দেয়।
        public void TakeTurn()
        {
            CollectResources();
            BuildStructures();
            BuildUnits();
            Attack();
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // ভিন্ন ভিন্ন জাতির (যেমন: Orcs, Elves) বিল্ডিং এবং ইউনিট আলাদা হবে, তাই এগুলো abstract।
        protected abstract void BuildStructures();
        protected abstract void BuildUnits();

        // Common methods (রিসোর্স কালেক্ট করা এবং অ্যাটাক করার লজিক সব জাতির জন্যই একই)
        private void CollectResources()
        {
            Console.WriteLine("[Common] Collecting gold and wood from the map.");
        }

        private void Attack()
        {
            Console.WriteLine("[Common] Sending all trained units to attack the enemy.\n");
        }
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // Orcs জাতির স্পেসিফিক লজিক
    public class OrcsAI : GameAI
    {
        protected override void BuildStructures()
        {
            Console.WriteLine("[Orcs] Building Orc Barracks and Forges.");
        }

        protected override void BuildUnits()
        {
            Console.WriteLine("[Orcs] Training Orc Warriors and Goblin Archers.");
        }
    }

    // Elves জাতির স্পেসিফিক লজিক
    public class ElvesAI : GameAI
    {
        protected override void BuildStructures()
        {
            Console.WriteLine("[Elves] Building Archery Ranges and Magic Trees.");
        }

        protected override void BuildUnits()
        {
            Console.WriteLine("[Elves] Training Elf Archers and Mages.");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Game AI) ===\n");

            GameAI orcs = new OrcsAI();
            orcs.TakeTurn();

            GameAI elves = new ElvesAI();
            elves.TakeTurn();
        }
    }
}
