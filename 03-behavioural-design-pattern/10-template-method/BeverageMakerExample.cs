using System;

namespace BehavioralDesignPattern.TemplateMethod.BeverageMaker
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // এটি হলো একটি বেস ক্লাস, যেখানে কাজের মূল স্ট্রাকচার (Skeleton) এবং কিছু ফিক্সড স্টেপ ডিফাইন করা থাকে।
    public abstract class BeverageMaker
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // এটি হলো মূল টেমপ্লেট বা ছাঁচ। এই মেথডটি কাজের নির্দিষ্ট সিকোয়েন্স (আগে পানি ফুটবে, তারপর চা পাতা দিবে...) ফিক্স করে দেয়।
        // C#-এ মেথড ডিফল্টভাবে নন-ভার্চুয়াল (non-virtual) থাকে, ফলে সাব-ক্লাস এই মেথডটি ওভাররাইড করে সিকোয়েন্স ভাঙতে পারবে না (Java-র final এর সমতুল্য)।
        public void MakeBeverage()
        {
            BoilWater();
            Brew();
            PourInCup();
            AddCondiments();
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // এই মেথডগুলোর বডি এখানে ফাঁকা রাখা হয়। সাব-ক্লাসগুলোকে (চা বা কফি মেকার) অবশ্যই নিজেদের মতো করে এই মেথডগুলো ইমপ্লিমেন্ট করতে হবে।
        protected abstract void Brew();
        protected abstract void AddCondiments();

        // এই মেথডগুলোর কাজ সবার জন্যই সেম (চা এবং কফি উভয়ের জন্যই পানি ফোটাতে হবে)। তাই কোড ডুপ্লিকেশন এড়াতে এগুলো এখানেই লেখা হয়েছে।
        private void BoilWater()
        {
            Console.WriteLine("Boiling water");
        }

        private void PourInCup()
        {
            Console.WriteLine("Pouring into cup");
        }
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // এরা হলো সাব-ক্লাস যারা বেস ক্লাসটিকে ইনহেরিট করে এবং শুধুমাত্র নিজেদের জন্য প্রয়োজনীয় abstract মেথডগুলো ইমপ্লিমেন্ট করে (যেমন চা বানানোর পদ্ধতি)।
    public class TeaMaker : BeverageMaker
    {
        protected override void Brew()
        {
            Console.WriteLine("Steeping the tea");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Adding lemon");
        }
    }

    public class CoffeeMaker : BeverageMaker
    {
        protected override void Brew()
        {
            Console.WriteLine("Dripping coffee through filter");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Adding sugar and milk");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    // ক্লায়েন্ট শুধু সাব-ক্লাসের অবজেক্ট তৈরি করে এবং বেস ক্লাসের Template Method (MakeBeverage) কে কল করে দেয়।
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Beverage Maker) ===\n");

            Console.WriteLine("Making tea:");
            BeverageMaker teaMaker = new TeaMaker();
            teaMaker.MakeBeverage();

            Console.WriteLine("\nMaking coffee:");
            BeverageMaker coffeeMaker = new CoffeeMaker();
            coffeeMaker.MakeBeverage();
        }
    }
}
