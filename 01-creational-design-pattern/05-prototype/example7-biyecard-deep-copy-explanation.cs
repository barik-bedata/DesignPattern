using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Reference Copy Bug)
// ==============================================================
namespace PrototypePattern.BiyeCard.Violation
{
    public class Venue
    {
        public string HallName, Address;
        public Venue(string hall, string address) { HallName = hall; Address = address; }
    }

    public class BiyeCard
    {
        public string GroomName, BrideName, Date, BorderDesign;
        public Venue Venue;

        public BiyeCard(string groom, string bride, string date, Venue venue)
        {
            GroomName = groom; BrideName = bride; Date = date; Venue = venue;

            Console.WriteLine($"   -> {groom}-{bride}'র জন্য নতুন বর্ডার ডিজাইন বানানো হচ্ছে...");
            Thread.Sleep(1000); 
            BorderDesign = "Gold-Floral-Mandala-v1";
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN ===");
            var card1 = new BiyeCard("Arnab", "Priya", "20 Dec 2026", new Venue("Rajbari Hall", "Dhanmondi"));
            
            // ❌ লাইন ব্যাখ্যা: এখানে card1 এর মেমোরি অ্যাড্রেসটা card3 এর ভেতরে ঢুকে গেলো। 
            var card3 = card1; 
            
            // ❌ লাইন ব্যাখ্যা: যেহেতু card3 আর card1 একই জিনিস, তাই card3 এর নাম বদলালে
            // card1 এর নামও বদলে যাবে। এটি একটি মারাত্মক বাগ!
            card3.GroomName = "Tanvir"; 
            
            Console.WriteLine($"Card1 Groom Name: {card1.GroomName} (Unexpected Bug!)"); 
        }
    }
}

// ==============================================================
// ✅ SOLUTION: Prototype Pattern (Using 4 Components & Deep Copy)
// ==============================================================
namespace PrototypePattern.BiyeCard.Solution
{
    public class Venue : ICloneable
    {
        public string HallName, Address;
        public Venue(string hall, string address) { HallName = hall; Address = address; }
        
        public object Clone() => new Venue(HallName, Address); 
    }

    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface IBiyeCard
    {
        IBiyeCard Clone();
        void Customize(string groom, string bride, string date);
        void SetVenueAddress(string address);
        string GetVenueAddress();
        void Display();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class BiyeCard : IBiyeCard
    {
        public string GroomName { get; private set; }
        public string BrideName { get; private set; }
        public string Date { get; private set; }
        public string BorderDesign { get; private set; }
        public Venue Venue { get; private set; }

        public BiyeCard(string groom, string bride, string date, Venue venue, string border)
        { 
            GroomName = groom; BrideName = bride; Date = date; Venue = venue; BorderDesign = border; 
        }

        public void Customize(string groom, string bride, string date)
        {
            GroomName = groom;
            BrideName = bride;
            Date = date;
        }

        public void SetVenueAddress(string address) => Venue.Address = address;
        public string GetVenueAddress() => Venue.Address;

        // 🌟 Deep clone ম্যাজিক:
        public IBiyeCard Clone()
        {
            Venue clonedVenue = (Venue)Venue.Clone();
            return new BiyeCard(GroomName, BrideName, Date, clonedVenue, BorderDesign);
        }

        public void Display()
        {
            Console.WriteLine($"[Card] {GroomName} & {BrideName} | Date: {Date} | Venue: {Venue.HallName}, {Venue.Address}");
        }
    }

    // ==============================================================
    // ৩. Client (CardDesignStudio)
    // ==============================================================
    public class CardDesignStudio
    {
        private IBiyeCard _masterTemplate;

        public CardDesignStudio(IBiyeCard masterTemplate)
        {
            _masterTemplate = masterTemplate;
        }

        public IBiyeCard PrintCustomCard(string groom, string bride, string date)
        {
            var clone = _masterTemplate.Clone();
            clone.Customize(groom, bride, date);
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

            // ১. মাস্টার টেমপ্লেট
            var masterTemplate = new BiyeCard("___", "___", "___",
                                   new Venue("Rajbari Hall", "Dhanmondi"), "Gold-Floral-Mandala-v1");

            // ২. স্টুডিও (ক্লায়েন্ট) এর কাছে মাস্টার কপি দিলাম
            var studio = new CardDesignStudio(masterTemplate);

            // ৩. ক্লায়েন্টকে দিয়ে প্রথম কার্ড ক্লোন করালাম
            var card1 = studio.PrintCustomCard("Arnab", "Priya", "20 Dec 2026");
            card1.Display();

            // ৪. দ্বিতীয় কার্ড ক্লোন করালাম
            var card2 = studio.PrintCustomCard("Rahim", "Sumi", "5 Jan 2027");
            
            // 🌟 Deep Copy এর চাক্ষুষ প্রমান: 
            card2.SetVenueAddress("Mirpur, Dhaka"); 
            card2.Display();

            // প্রমাণ করে দিচ্ছি যে মাস্টার কপিটি সুরক্ষিত আছে
            Console.WriteLine($"\n[Check Master] Master Venue Address: {masterTemplate.GetVenueAddress()} (Unchanged!)");
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.BiyeCard.Violation.ViolationRunner.Run();
        PrototypePattern.BiyeCard.Solution.SolutionRunner.Run();
    }
}
