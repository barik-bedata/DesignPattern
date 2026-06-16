using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (বারবার NID এর ব্যাকগ্রাউন্ড লোড করা)
// ==============================================================

namespace PrototypePattern.NIDCard.Violation
{
    public class NIDCard
    {
        public string GovtSeal { get; set; }
        public string HologramDesign { get; set; }
        public string CitizenName { get; set; }

        public NIDCard(string name)
        {
            Console.WriteLine(">> [Violation] ডেটাবেস থেকে NID এর হলোগ্রাম এবং সিল লোড হচ্ছে... (API Call)");
            Thread.Sleep(500); 

            GovtSeal = "Govt. of Bangladesh Seal";
            HologramDesign = "High Security Hologram 3D";
            CitizenName = name;
        }

        public void Display()
        {
            Console.WriteLine($"[NID] Name: {CitizenName} | Seal: {GovtSeal} | Hologram: {HologramDesign}");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: প্রতিটি NID প্রিন্টে ভারী ডিজাইন লোড ===");
            var p1 = new NIDCard("Bedata");
            p1.Display();

            var p2 = new NIDCard("Barik");
            p2.Display();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (ব্লাংক NID কে ফটোকপি করা)
// ==============================================================

namespace PrototypePattern.NIDCard.Solution
{
    public interface INIDPrototype
    {
        INIDPrototype Clone();
    }

    public class NIDCard : INIDPrototype
    {
        public string GovtSeal { get; set; }
        public string HologramDesign { get; set; }
        public string CitizenName { get; set; }

        public NIDCard()
        {
            Console.WriteLine("\n>> [Solution] ব্লাংক NID কার্ডের মাস্টার ডিজাইন একবার লোড হচ্ছে...");
            Thread.Sleep(500);

            GovtSeal = "Govt. of Bangladesh Seal";
            HologramDesign = "High Security Hologram 3D";
        }

        public INIDPrototype Clone()
        {
            return (INIDPrototype)this.MemberwiseClone();
        }

        public void Display()
        {
            Console.WriteLine($"[NID] Name: {CitizenName} | Seal: {GovtSeal} | Hologram: {HologramDesign}");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: ব্ল্যাংক মাস্টার কপিকে ক্লোন করে শুধু নাম বসানো ===");
            
            var blankMasterNID = new NIDCard();

            var p1 = (NIDCard)blankMasterNID.Clone();
            p1.CitizenName = "Bedata";
            p1.Display();

            var p2 = (NIDCard)blankMasterNID.Clone();
            p2.CitizenName = "Barik";
            p2.Display();
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.NIDCard.Violation.ViolationRunner.Run();
        PrototypePattern.NIDCard.Solution.SolutionRunner.Run();
    }
}
