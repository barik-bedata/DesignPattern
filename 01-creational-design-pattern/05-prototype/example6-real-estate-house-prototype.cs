using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (প্রতিটি বাড়ির জন্য নতুন করে আর্কিটেকচার ডিজাইন করা)
// ==============================================================

namespace PrototypePattern.RealEstate.Violation
{
    public class HouseBlueprint
    {
        public string FoundationPlan { get; set; }
        public string PlumbingSystem { get; set; }
        public string RoofDesign { get; set; }
        
        public string HouseNumber { get; set; }
        public string PaintColor { get; set; }

        public HouseBlueprint(string number, string color)
        {
            Console.WriteLine(">> [Violation] আর্কিটেক্ট নতুন করে বাড়ির ব্লু-প্রিন্ট বানাচ্ছে... (Taking Months)");
            Thread.Sleep(500);

            FoundationPlan = "Duplex Foundation 2500 sqft";
            PlumbingSystem = "Modern European Plumbing";
            RoofDesign = "Flat Roof with Garden";

            HouseNumber = number;
            PaintColor = color;
        }

        public void Display()
        {
            Console.WriteLine($"[House {HouseNumber}] Color: {PaintColor} | Base: {FoundationPlan}");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: প্রতিটি বাড়ির জন্য নতুন করে ডিজাইন করা ===");
            var house1 = new HouseBlueprint("H-101", "White");
            house1.Display();

            var house2 = new HouseBlueprint("H-102", "Light Yellow");
            house2.Display();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (মাস্টার ব্লু-প্রিন্ট ক্লোন করা)
// ==============================================================

namespace PrototypePattern.RealEstate.Solution
{
    public interface IHousePrototype
    {
        IHousePrototype Clone();
    }

    public class HouseBlueprint : IHousePrototype
    {
        public string FoundationPlan { get; set; }
        public string PlumbingSystem { get; set; }
        public string RoofDesign { get; set; }
        
        public string HouseNumber { get; set; }
        public string PaintColor { get; set; }

        public HouseBlueprint()
        {
            Console.WriteLine("\n>> [Solution] বসুন্ধরা প্রোজেক্টের জন্য একবারই মাস্টার ব্লু-প্রিন্ট বানানো হচ্ছে... (Taking Months)");
            Thread.Sleep(500);

            FoundationPlan = "Duplex Foundation 2500 sqft";
            PlumbingSystem = "Modern European Plumbing";
            RoofDesign = "Flat Roof with Garden";
        }

        public IHousePrototype Clone()
        {
            return (IHousePrototype)this.MemberwiseClone();
        }

        public void Display()
        {
            Console.WriteLine($"[House {HouseNumber}] Color: {PaintColor} | Base: {FoundationPlan}");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: মাস্টার ব্লু-প্রিন্ট ক্লোন করে শুধু নাম্বার ও কালার চেঞ্জ করা ===");
            
            var masterBlueprint = new HouseBlueprint();

            var house1 = (HouseBlueprint)masterBlueprint.Clone();
            house1.HouseNumber = "H-101";
            house1.PaintColor = "White";
            house1.Display();

            var house2 = (HouseBlueprint)masterBlueprint.Clone();
            house2.HouseNumber = "H-102";
            house2.PaintColor = "Light Yellow";
            house2.Display();
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.RealEstate.Violation.ViolationRunner.Run();
        PrototypePattern.RealEstate.Solution.SolutionRunner.Run();
    }
}
