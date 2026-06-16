using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (বারবার স্ক্র্যাচ থেকে বানানো)
// ==============================================================
// সমস্যা: যদি আমরা প্রোটোটাইপ ব্যবহার না করি, তবে প্রতিটি মেহমানের জন্য 
// কার্ড বানানোর সময় 'মাস্টার ডেটা' (যেমন পাত্র-পাত্রীর নাম, ভেন্যু, ডিজাইন) 
// বারবার সেট করতে হবে। এতে অনেক সময় এবং মেমোরি নষ্ট হয়।

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
            // ধরি, এই ডেটাগুলো সেট করা অনেক এক্সপেনসিভ কাজ!
            Console.WriteLine(">> [Violation] ডিজাইনার নতুন করে কার্ড ডিজাইন করছে... (Expensive Operation)");
            Thread.Sleep(500); // সিমুলেট করছি যে অনেক সময় লাগছে

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
            
            // সমস্যা: প্রত্যেকবার নতুন অবজেক্ট বানালে প্রত্যেকবার ডিজাইনারকে নতুন করে কাজ করতে হচ্ছে! (সময় নষ্ট)
            var card1 = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর", "খালেক চাচা");
            card1.PrintCard();

            var card2 = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর", "বন্ধু শফিক");
            card2.PrintCard();
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (মাস্টার কপি ফটোকপি করা)
// ==============================================================
// সমাধান: আমরা একটি 'মাস্টার কপি' বানাবো। এরপর জাস্ট সেটাকে ক্লোন (ফটোকপি) করে
// শুধু মেহমানের নাম চেঞ্জ করে দেবো!

namespace PrototypePattern.WeddingCard.Solution
{
    public interface ICloneableCard
    {
        ICloneableCard Clone();
    }

    public class WeddingCard : ICloneableCard
    {
        public string GroomAndBride { get; set; }
        public string Venue { get; set; }
        public string Date { get; set; }
        public string GuestName { get; set; }

        public WeddingCard(string groomAndBride, string venue, string date)
        {
            Console.WriteLine(">> [Solution] ডিজাইনার একবারই মাস্টার কপি বানাচ্ছে... (Expensive Operation)");
            Thread.Sleep(500);

            GroomAndBride = groomAndBride;
            Venue = venue;
            Date = date;
        }

        public ICloneableCard Clone()
        {
            // জাস্ট ফটোকপি মেশিন দিয়ে ফটোকপি করে দিলাম!
            return (ICloneableCard)this.MemberwiseClone(); 
        }

        public void PrintCard()
        {
            Console.WriteLine($"[দাওয়াত] প্রতি: {GuestName} | {GroomAndBride} এর বিয়ে | স্থান: {Venue}\n");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ✅ SOLUTION RUN: একবার ডিজাইন করে বারবার ফটোকপি (Clone) ===");
            
            // ১. একবারই মাস্টার কপি বানালাম (সময় বাঁচলো)
            var masterCopy = new WeddingCard("রহিম ও কারিমা", "সেনাকুঞ্জ", "১২ই ডিসেম্বর");

            // ২. এবার জাস্ট ফটোকপি করে নাম বসাচ্ছি (No extra time needed!)
            var card1 = (WeddingCard)masterCopy.Clone();
            card1.GuestName = "খালেক চাচা";
            card1.PrintCard();

            var card2 = (WeddingCard)masterCopy.Clone();
            card2.GuestName = "বন্ধু শফিক";
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
