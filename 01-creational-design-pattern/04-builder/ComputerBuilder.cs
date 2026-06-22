using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Telescoping Constructor Anti-pattern)
// ==============================================================
// ভুল পদ্ধতি: একটি অবজেক্ট তৈরি করতে কন্সট্রাক্টরের ভেতরে অনেকগুলো প্যারামিটার পাঠানো।
// সমস্যা: 
// ১. প্যারামিটারগুলোর সিরিয়াল মনে রাখা অসম্ভব।
// ২. পরপর কয়েকটি `bool` বা `int` থাকলে ভুল ভ্যালু পাস হওয়ার সম্ভাবনা অনেক বেশি।
// ৩. ঐচ্ছিক (Optional) প্যারামিটার থাকলে অনেকগুলো null বা false পাস করতে হয়, যা কোডকে কুৎসিত করে তোলে।

namespace BuilderPattern.Computer.Violation
{
    // ১. প্রোডাক্ট ক্লাস (The God Object)
    public class Computer
    {
        public string Processor { get; }
        public int RamInGB { get; }
        public int StorageInGB { get; }
        public string GraphicsCard { get; }
        public bool HasLiquidCooling { get; }

        // Telescoping Constructor - এটি একটি মারাত্মক ব্যাড প্র্যাকটিস
        public Computer(string processor, int ram, int storage, string gpu, bool cooling)
        {
            Processor = processor;
            RamInGB = ram;
            StorageInGB = storage;
            GraphicsCard = gpu;
            HasLiquidCooling = cooling;
        }

        public void Display()
        {
            Console.WriteLine($"[Violation PC] CPU: {Processor} | RAM: {RamInGB}GB | Storage: {StorageInGB}GB | GPU: {GraphicsCard ?? "None"} | Liquid Cooler: {(HasLiquidCooling ? "Yes" : "No")}");
        }
    }

    // এই ক্লাসটি রান করে ভায়োলেশনের সমস্যাটি দেখাবে
    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: বিশাল কন্সট্রাক্টর দিয়ে PC বিল্ড ===");
            
            // সমস্যা: কোন int টা RAM আর কোনটা Storage? কোন bool টা কিসের? বোঝা অসম্ভব!
            var gamingPc = new Computer("Intel i9", 32, 2048, "RTX 4090", true);
            gamingPc.Display();

            // সমস্যা: শুধু অফিস পিসি বানাতে গেলেও অযথা null এবং false পাঠাতে হচ্ছে!
            var officePc = new Computer("Intel i3", 8, 256, null, false);
            officePc.Display();
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Using Builder Pattern - Fluent API)
// ==============================================================
// সমাধান: অবজেক্ট তৈরি করার প্রক্রিয়াটিকে ধাপে ধাপে (step-by-step) ভাগ করা। 

namespace BuilderPattern.Computer.Solution
{
    // 🧱 বিল্ডার প্যাটার্নের ৫টি মূল কম্পোনেন্ট:

    // ১. Product: যে জটিল অবজেক্টটি তৈরি করা হচ্ছে
    // ==============================================================
    public class Computer
    {
        public string Processor { get; set; }
        public int RamInGB { get; set; }
        public int StorageInGB { get; set; }
        public string GraphicsCard { get; set; }
        public bool HasLiquidCooling { get; set; }

        public void Display()
        {
            Console.WriteLine($"[✅ Builder PC] CPU: {Processor} | RAM: {RamInGB}GB | Storage: {StorageInGB}GB | GPU: {GraphicsCard ?? "None"} | Liquid Cooler: {(HasLiquidCooling ? "Yes" : "No")}");
        }
    }

    // ২. Builder Interface: অবজেক্ট তৈরির বিভিন্ন ধাপের নিয়ম বেঁধে দেয়
    // ==============================================================
    public interface IComputerBuilder
    {
        IComputerBuilder SetProcessor(string processor);
        IComputerBuilder SetRAM(int gb);
        IComputerBuilder SetStorage(int gb);
        IComputerBuilder SetGraphicsCard(string gpu);
        IComputerBuilder AddLiquidCooling();
        Computer Build(); // শেষে ফাইনাল প্রোডাক্টটি রিটার্ন করবে
    }

    // ৩. Concrete Builder: Builder Interface ইমপ্লিমেন্ট করে পিসি জোড়া লাগায়
    // ==============================================================
    public class CustomComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        // Fluent API এর জন্য `this` রিটার্ন করা হচ্ছে, যাতে Method Chaining করা যায়
        public IComputerBuilder SetProcessor(string processor)
        {
            _computer.Processor = processor;
            return this;
        }

        public IComputerBuilder SetRAM(int gb)
        {
            _computer.RamInGB = gb;
            return this;
        }

        public IComputerBuilder SetStorage(int gb)
        {
            _computer.StorageInGB = gb;
            return this;
        }

        public IComputerBuilder SetGraphicsCard(string gpu)
        {
            _computer.GraphicsCard = gpu;
            return this;
        }

        public IComputerBuilder AddLiquidCooling()
        {
            _computer.HasLiquidCooling = true;
            return this;
        }

        public Computer Build()
        {
            Computer builtComputer = _computer;
            _computer = new Computer(); // রিসেট
            return builtComputer;
        }
    }

    // ৪. Director: নির্দিষ্ট সিকোয়েন্স বা ধাপে অবজেক্ট বিল্ড করার কাজটি পরিচালনা করে
    // ==============================================================
    public class ComputerDirector
    {
        // ডিরেক্টর শুধু ইনস্ট্রাকশন দেয়, আসল কাজ বিল্ডারই করে
        public Computer BuildHighEndGamingPC(IComputerBuilder builder)
        {
            return builder
                    .SetProcessor("AMD Ryzen 9")
                    .SetRAM(64)
                    .SetStorage(2048)
                    .SetGraphicsCard("RTX 4090")
                    .AddLiquidCooling()
                    .Build();
        }
    }

    // ৫. Builder Client: Concrete Builder তৈরি করে প্রয়োজন হলে Director-কে দেয় এবং Build করে
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Builder Pattern দিয়ে PC বিল্ড ===");

            // পদ্ধতি ১: Custom Build (ডিরেক্টর ছাড়া সরাসরি বিল্ডার ক্লায়েন্ট ব্যবহার করে)
            Console.WriteLine("-- Custom Office PC --");
            IComputerBuilder builder = new CustomComputerBuilder();
            var officePc = builder
                            .SetProcessor("Intel i5")
                            .SetRAM(16)
                            .SetStorage(512)
                            .Build(); // Graphics Card বা Cooling দরকার নেই, তাই ডাকলাম না!
            officePc.Display();

            // পদ্ধতি ২: Director এর মাধ্যমে ফিক্সড কনফিগারেশনের পিসি বানানো
            Console.WriteLine("\n-- Directed Gaming PC --");
            var director = new ComputerDirector();
            var gamingPc = director.BuildHighEndGamingPC(new CustomComputerBuilder());
            gamingPc.Display();
        }
    }
}


// ══════════════════════════════════════════
// 🚀 Application Client / Main Program (সমগ্র প্রোগ্রামের আল্টিমেট রানার)
// ══════════════════════════════════════════
class Program
{
    static void Main()
    {
        // ভায়োলেশন রান
        BuilderPattern.Computer.Violation.ViolationRunner.Run();
        
        // সলিউশন রান
        BuilderPattern.Computer.Solution.SolutionRunner.Run();
    }
}
