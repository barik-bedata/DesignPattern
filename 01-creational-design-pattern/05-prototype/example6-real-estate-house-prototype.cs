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
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.RealEstate.Solution
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface IHousePrototype
    {
        IHousePrototype Clone();
        void CustomizeHouse(string houseNumber, string color);
        void Display();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class HouseBlueprint : IHousePrototype
    {
        public string FoundationPlan { get; private set; }
        public string PlumbingSystem { get; private set; }
        public string RoofDesign { get; private set; }
        
        public string HouseNumber { get; private set; }
        public string PaintColor { get; private set; }

        public HouseBlueprint()
        {
            Console.WriteLine("\n>> [Solution] বসুন্ধরা প্রোজেক্টের জন্য একবারই মাস্টার ব্লু-প্রিন্ট বানানো হচ্ছে... (Taking Months)");
            Thread.Sleep(500);

            FoundationPlan = "Duplex Foundation 2500 sqft";
            PlumbingSystem = "Modern European Plumbing";
            RoofDesign = "Flat Roof with Garden";
        }

        public void CustomizeHouse(string houseNumber, string color)
        {
            HouseNumber = houseNumber;
            PaintColor = color;
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

    // ==============================================================
    // ৩. Client (ConstructionCompany)
    // ==============================================================
    public class ConstructionCompany
    {
        private IHousePrototype _masterBlueprint;

        public ConstructionCompany(IHousePrototype masterBlueprint)
        {
            _masterBlueprint = masterBlueprint;
        }

        public IHousePrototype BuildHouse(string houseNumber, string color)
        {
            var clone = _masterBlueprint.Clone();
            clone.CustomizeHouse(houseNumber, color);
            return clone;
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
            
            // মাস্টার ব্লু-প্রিন্ট
            var masterBlueprint = new HouseBlueprint();

            // কন্সট্রাকশন কোম্পানির (ক্লায়েন্ট) কাছে মাস্টার ডিজাইন দেওয়া হলো
            var company = new ConstructionCompany(masterBlueprint);

            // কোম্পানি মাস্টার কপি ক্লোন করে কাস্টম বাড়ি বানাচ্ছে
            var house1 = company.BuildHouse("H-101", "White");
            house1.Display();

            var house2 = company.BuildHouse("H-102", "Light Yellow");
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
