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
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.NIDCard.Solution
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface INIDPrototype
    {
        INIDPrototype Clone();
        void SetCitizenName(string name);
        void Display();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class NIDCard : INIDPrototype
    {
        public string GovtSeal { get; private set; }
        public string HologramDesign { get; private set; }
        public string CitizenName { get; private set; }

        public NIDCard()
        {
            Console.WriteLine("\n>> [Solution] ব্লাংক NID কার্ডের মাস্টার ডিজাইন একবার লোড হচ্ছে...");
            Thread.Sleep(500);

            GovtSeal = "Govt. of Bangladesh Seal";
            HologramDesign = "High Security Hologram 3D";
        }

        public void SetCitizenName(string name)
        {
            CitizenName = name;
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

    // ==============================================================
    // ৩. Client (ElectionCommissionServer)
    // ==============================================================
    public class ElectionCommissionServer
    {
        private INIDPrototype _blankTemplate;

        public ElectionCommissionServer(INIDPrototype blankTemplate)
        {
            _blankTemplate = blankTemplate;
        }

        public INIDPrototype IssueNID(string name)
        {
            var clone = _blankTemplate.Clone();
            clone.SetCitizenName(name);
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
            
            // মাস্টার ব্লাংক কার্ড
            var blankMasterNID = new NIDCard();

            // সার্ভার (ক্লায়েন্ট) এর কাছে ব্লাংক টেমপ্লেট দিয়ে দিলাম
            var server = new ElectionCommissionServer(blankMasterNID);

            // সার্ভার শুধু ক্লোন করে নাম বসিয়ে দিচ্ছে
            var p1 = server.IssueNID("Bedata");
            p1.Display();

            var p2 = server.IssueNID("Barik");
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
