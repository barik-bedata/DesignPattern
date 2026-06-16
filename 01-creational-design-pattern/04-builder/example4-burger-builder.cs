using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Telescoping Constructor Anti-pattern)
// ==============================================================
// সমস্যা: একটি অবজেক্ট তৈরি করতে অনেকগুলো কন্সট্রাক্টর ওভারলোড তৈরি করা।
// একে Telescoping Constructor বলা হয়। এর ফলে কোন প্যারামিটার কীসের, 
// তা মনে রাখা অসম্ভব হয়ে যায় এবং কোড পড়া খুব কঠিন হয়ে পড়ে।

namespace BuilderPattern.BurgerBuilder.Violation
{
    public class Burger
    {
        public string Bun { get; }
        public string Patty { get; }
        public bool Cheese { get; }
        public bool Lettuce { get; }
        public bool Tomato { get; }
        public bool Onion { get; }
        public string Sauce { get; }
        public bool Bacon { get; }
        public string Size { get; }

        public Burger(string bun, string patty)
            : this(bun, patty, false, false, false, false, "Ketchup", false, "Medium") { }

        public Burger(string bun, string patty, bool cheese)
            : this(bun, patty, cheese, false, false, false, "Ketchup", false, "Medium") { }

        // সবচেয়ে বাজে অংশ - সব option একসাথে একটা constructor এ
        public Burger(string bun, string patty, bool cheese, bool lettuce,
                      bool tomato, bool onion, string sauce, bool bacon, string size)
        {
            Bun = bun; Patty = patty; Cheese = cheese; Lettuce = lettuce;
            Tomato = tomato; Onion = onion; Sauce = sauce; Bacon = bacon; Size = size;
        }

        public void Display()
        {
            Console.WriteLine($"[Violation Burger] {Size} {Patty} on {Bun} | Cheese: {Cheese}, Lettuce: {Lettuce}, Tomato: {Tomato}, Onion: {Onion}, Bacon: {Bacon}, Sauce: {Sauce}");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: Telescoping Constructor দিয়ে Burger ===");
            
            // Usage - বোঝার উপায় নাই কোনটা cheese, কোনটা bacon! 
            // true, false, true দেখে কিছুই বোঝা যাচ্ছে না।
            var burger = new Burger("Sesame", "Beef", true, false, true, false, "Mayo", true, "Large");
            burger.Display();
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Inner Builder Class Pattern)
// ==============================================================
// সমাধান: ক্লাসের ভেতরেই একটি Builder ক্লাস (Inner Class) তৈরি করা এবং
// অরিজিনাল ক্লাসের কন্সট্রাক্টর private করে দেওয়া। এতে করে Builder ছাড়া 
// অন্য কোনোভাবেই অবজেক্ট তৈরি করা সম্ভব হয় না।

namespace BuilderPattern.BurgerBuilder.Solution
{
    public class Burger
    {
        public string Bun { get; private set; }
        public string Patty { get; private set; }
        public bool Cheese { get; private set; }
        public bool Bacon { get; private set; }
        public string Sauce { get; private set; }
        public string Size { get; private set; }

        // কন্সট্রাক্টর প্রাইভেট! অর্থাৎ শুধু Builder-ই এই Burger বানাতে পারবে।
        private Burger() { } 

        public void Display()
        {
            Console.WriteLine($"[✅ Builder Burger] {Size} {Patty} on {Bun} | Cheese: {Cheese}, Bacon: {Bacon}, Sauce: {Sauce}");
        }

        // Inner Builder Class
        public class Builder
        {
            private readonly Burger _burger = new Burger();

            public Builder SetBun(string bun) { _burger.Bun = bun; return this; }
            public Builder SetPatty(string patty) { _burger.Patty = patty; return this; }
            public Builder AddCheese() { _burger.Cheese = true; return this; }
            public Builder AddBacon() { _burger.Bacon = true; return this; }
            public Builder SetSauce(string sauce) { _burger.Sauce = sauce; return this; }
            public Builder SetSize(string size) { _burger.Size = size; return this; }

            public Burger Build()
            {
                // ভ্যালিডেশন
                if (string.IsNullOrEmpty(_burger.Bun) || string.IsNullOrEmpty(_burger.Patty))
                    throw new InvalidOperationException("Bun and Patty are mandatory!");
                    
                return _burger;
            }
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Inner Builder Class দিয়ে Burger ===");
            
            // Usage - readable, self-explanatory
            Burger burger = new Burger.Builder()
                .SetBun("Sesame")
                .SetPatty("Beef")
                .AddCheese()
                .AddBacon()
                .SetSauce("Mayo")
                .SetSize("Large")
                .Build();

            burger.Display();
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
        BuilderPattern.BurgerBuilder.Violation.ViolationRunner.Run();
        BuilderPattern.BurgerBuilder.Solution.SolutionRunner.Run();
    }
}
