using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Reference Copy Bug)
// ==============================================================
namespace PrototypePattern.BiyeCard.Violation
{
    // ভেন্যু ক্লাস (Reference Type)
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

            // ধরে নিচ্ছি - বর্ডার ডিজাইন বানাতে অনেক সময়/effort লাগে
            Console.WriteLine($"   -> {groom}-{bride}'র জন্য নতুন বর্ডার ডিজাইন বানানো হচ্ছে...");
            Thread.Sleep(1000); // expensive কাজ simulate করছি
            BorderDesign = "Gold-Floral-Mandala-v1";
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN ===");
            // প্রতিটা পরিবারের জন্য আবার পুরো design কাজ থেকে শুরু - একই জিনিস বারবার বানানো হচ্ছে
            var card1 = new BiyeCard("Arnab", "Priya", "20 Dec 2026", new Venue("Rajbari Hall", "Dhanmondi"));
            var card2 = new BiyeCard("Rahim", "Sumi", "5 Jan 2027", new Venue("Rajbari Hall", "Dhanmondi"));

            // আরেকটা খুবই common ভুল - মনে করা "=" দিয়ে copy হয়ে যাবে
            // ❌ লাইন ব্যাখ্যা: এখানে card1 এর মেমোরি অ্যাড্রেসটা card3 এর ভেতরে ঢুকে গেলো। 
            // কোনো নতুন অবজেক্ট তৈরি হয়নি! card3 এবং card1 একই জিনিসকে পয়েন্ট করছে।
            var card3 = card1; 
            
            // ❌ লাইন ব্যাখ্যা: যেহেতু card3 আর card1 একই জিনিস, তাই card3 এর নাম বদলালে
            // card1 এর নামও বদলে যাবে। এটি একটি মারাত্মক বাগ!
            card3.GroomName = "Tanvir"; 
            
            // আউটপুট: "Tanvir" — card1-ও বদলে গেলো! 
            Console.WriteLine($"Card1 Groom Name: {card1.GroomName} (Unexpected Bug!)"); 
        }
    }
}

// ==============================================================
// ✅ SOLUTION: Prototype Pattern (ICloneable & Deep Copy)
// ==============================================================
namespace PrototypePattern.BiyeCard.Solution
{
    // ১. Venue ক্লাসকে ICloneable করা হলো যাতে সেও নিজের কপি বানাতে পারে
    public class Venue : ICloneable
    {
        public string HallName, Address;
        public Venue(string hall, string address) { HallName = hall; Address = address; }
        
        // ✅ লাইন ব্যাখ্যা: Venue এর নিজস্ব Clone মেথড। এটি একটি সম্পূর্ণ নতুন 
        // Venue অবজেক্ট (independent কপি) তৈরি করে মেমোরিতে নতুন জায়গা নিচ্ছে।
        public object Clone() => new Venue(HallName, Address); 
    }

    // ২. BiyeCard ক্লাসকেও ICloneable করা হলো
    public class BiyeCard : ICloneable
    {
        public string GroomName, BrideName, Date, BorderDesign;
        public Venue Venue; // Nested Object (Reference Type)

        public BiyeCard(string groom, string bride, string date, Venue venue, string border)
        { 
            GroomName = groom; BrideName = bride; Date = date; Venue = venue; BorderDesign = border; 
        }

        public object Clone()
        {
            // 🌟 Deep clone ম্যাজিক:
            // ✅ লাইন ব্যাখ্যা: এখানে আমরা Venue.Clone() কল করে Venue ক্লাসের একটি নতুন কপি বানালাম।
            // এতে করে পুরনো Venue এর সাথে নতুন Venue এর reference শেয়ার হবে না!
            Venue clonedVenue = (Venue)Venue.Clone();
            
            // ✅ লাইন ব্যাখ্যা: নতুন কপি করা Venue টি ব্যবহার করে নতুন একটি BiyeCard তৈরি করে রিটার্ন করছি।
            // এটাই হলো Deep Copy.
            return new BiyeCard(GroomName, BrideName, Date, clonedVenue, BorderDesign);
        }

        public void Display()
        {
            Console.WriteLine($"[Card] {GroomName} & {BrideName} | Date: {Date} | Venue: {Venue.HallName}, {Venue.Address}");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN ===");

            // মাস্টার টেমপ্লেট - শুধু একবার "expensive" design কাজ হলো
            var masterTemplate = new BiyeCard("___", "___", "___",
                                   new Venue("Rajbari Hall", "Dhanmondi"), "Gold-Floral-Mandala-v1");

            // বাকি সব কার্ড - শুধু clone করে নাম/তারিখ বদলে দিচ্ছি, design আবার বানাচ্ছি না
            // ✅ লাইন ব্যাখ্যা: masterTemplate.Clone() কল করার ফলে আমাদের Deep Copy মেথড ফায়ার হলো।
            // এবং সম্পূর্ণ নতুন মেমোরি অ্যাড্রেস সহ একটি independent কার্ড তৈরি হলো।
            var card1 = (BiyeCard)masterTemplate.Clone();
            card1.GroomName = "Arnab"; 
            card1.BrideName = "Priya"; 
            card1.Date = "20 Dec 2026";
            card1.Display();

            var card2 = (BiyeCard)masterTemplate.Clone();
            card2.GroomName = "Rahim"; 
            card2.BrideName = "Sumi";
            
            // 🌟 Deep Copy এর চাক্ষুষ প্রমান: 
            // ✅ লাইন ব্যাখ্যা: card2-র venue আলাদা। এটা চেঞ্জ করলেও masterTemplate বা card1 এর কোনো ক্ষতি হবে না।
            // কারণ Deep Copy এর ফলে সবার Venue অবজেক্ট আলাদা আলাদা মেমোরিতে আছে।
            card2.Venue.Address = "Mirpur, Dhaka"; 
            card2.Display();

            // প্রমাণ করে দিচ্ছি যে মাস্টার কপিটি সুরক্ষিত আছে
            Console.WriteLine($"\n[Check Master] Master Venue Address: {masterTemplate.Venue.Address} (Unchanged!)");
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
        PrototypePattern.BiyeCard.Violation.ViolationRunner.Run();
        PrototypePattern.BiyeCard.Solution.SolutionRunner.Run();
    }
}
