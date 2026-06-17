using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (বারবার স্ক্র্যাচ থেকে বানানো)
// ==============================================================
namespace PrototypePattern.WeddingCard.Violation
{
    public class WeddingCard
    {
        public string GroomAndBride { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public string GuestName { get; set; }

        public WeddingCard(string groomAndBride, string venue, string date, string guestName)
        {
            Console.WriteLine(">> [Violation] ডিজাইনার নতুন করে কার্ড ডিজাইন করছে... (Expensive Operation)");
            Thread.Sleep(500); 

            GroomAndBride = groomAndBride;
            Venue = venue;
            Date = date;
            GuestName = guestName;
        }

        public void PrintCard()
        {
            Console.WriteLine($"[দাওয়াত] প্রতি: {GuestName} | {GroomAndBride} এর বিয়ে | স্থান: {Venue}\n");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: প্রতিটি মেহমানের জন্য নতুন কার্ড ডিজাইন ===");
            var card1 = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর", "খালেক চাচা");
            card1.PrintCard();

            var card2 = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর", "বন্ধু শফিক");
            card2.PrintCard();
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.WeddingCard.Solution
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface ICloneableCard
    {
        ICloneableCard Clone();
        void SetGuestName(string guestName);
        void PrintCard();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class WeddingCard : ICloneableCard
    {
        public string GroomAndBride { get; private set; }
        public string Venue { get; private set; }
        public string Date { get; private set; }
        public string GuestName { get; private set; }

        public WeddingCard(string groomAndBride, string venue, string date)
        {
            Console.WriteLine(">> [Solution] ডিজাইনার একবারই মাস্টার কপি বানাচ্ছে... (Expensive Operation)");
            Thread.Sleep(500);

            GroomAndBride = groomAndBride;
            Venue = venue;
            Date = date;
        }

        public void SetGuestName(string guestName)
        {
            GuestName = guestName;
        }

        public ICloneableCard Clone()
        {
            return (ICloneableCard)this.MemberwiseClone(); 
        }

        public void PrintCard()
        {
            Console.WriteLine($"[দাওয়াত] প্রতি: {GuestName} | {GroomAndBride} এর বিয়ে | স্থান: {Venue}\n");
        }
    }

    // ==============================================================
    // ৩. Client (PrintingPress)
    // ==============================================================
    public class CardPrintingPress
    {
        private ICloneableCard _masterTemplate;

        public CardPrintingPress(ICloneableCard masterTemplate)
        {
            _masterTemplate = masterTemplate;
        }

        public ICloneableCard PrintForGuest(string guestName)
        {
            // ক্লায়েন্ট মাস্টারকে ক্লোন করছে
            var clone = _masterTemplate.Clone();
            clone.SetGuestName(guestName);
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
            Console.WriteLine("=== ✅ SOLUTION RUN: Prototype Pattern (Using 4 Components) ===");
            
            // ১. একবারই মাস্টার কপি বানালাম 
            var masterCopy = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর");

            // ২. প্রিন্টিং প্রেস (ক্লায়েন্ট) এর কাছে মাস্টার কপি দিয়ে দিলাম
            var press = new CardPrintingPress(masterCopy);

            // ৩. ক্লায়েন্টকে দিয়ে শুধু নাম বসিয়ে ফটোকপি করাচ্ছি
            var card1 = press.PrintForGuest("খালেক চাচা");
            card1.PrintCard();

            var card2 = press.PrintForGuest("বন্ধু শফিক");
            card2.PrintCard();
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.WeddingCard.Violation.ViolationRunner.Run();
        PrototypePattern.WeddingCard.Solution.SolutionRunner.Run();
    }
}
