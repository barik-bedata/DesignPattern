using System;
using System.Collections.Generic;

namespace PrototypePattern.DeepVsShallow
{
    // ==============================================================
    // Smartphone Class
    // ==============================================================
    public class Smartphone
    {
        // Primitive Data (আলাদা মেমোরি পাবে)
        public string OwnerName { get; set; }
        
        // Reference Data (লিস্ট - মেমোরি শেয়ার হতে পারে)
        public List<string> InstalledApps { get; set; }

        public Smartphone(string ownerName)
        {
            OwnerName = ownerName;
            InstalledApps = new List<string>();
        }

        // ==========================================
        // ১. Shallow Copy (অগভীর কপি - যেখানে বাগ আছে)
        // ==========================================
        public Smartphone ShallowClone()
        {
            // MemberwiseClone শুধু মেইন অবজেক্ট বানায়, কিন্তু লিস্টের মেমোরি শেয়ার করে দেয়।
            return (Smartphone)this.MemberwiseClone();
        }

        // ==========================================
        // ২. Deep Copy (গভীর কপি - বাগ ফিক্সড)
        // ==========================================
        public Smartphone DeepClone()
        {
            // ১. প্রথমে মেইন অবজেক্ট কপি করলাম (Shallow Copy এর মতো)
            Smartphone copy = (Smartphone)this.MemberwiseClone();
            
            // ২. এবার বাগ ফিক্স: লিস্টের মেমোরি শেয়ারিং ভেঙে দিলাম!
            // নতুন ফোনের জন্য সম্পূর্ণ নতুন একটি লিস্ট (new List) তৈরি করলাম 
            // এবং পুরোনো ফোনের লিস্ট থেকে ডেটাগুলো লুপ করে নতুনটিতে বসিয়ে দিলাম।
            copy.InstalledApps = new List<string>(this.InstalledApps);
            
            return copy;
        }

        public void PrintStatus()
        {
            Console.WriteLine($"📱 {OwnerName}'s Phone Apps: {string.Join(", ", InstalledApps)}");
        }
    }

    // ==============================================================
    // Main Program
    // ==============================================================
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Deep Copy vs Shallow Copy Demo ===\n");

            // -------------------------------------------------------------
            // টেস্ট ১: SHALLOW COPY (The Bug)
            // -------------------------------------------------------------
            Console.WriteLine("--- 1. Testing SHALLOW COPY (The Bug) ---");
            Smartphone rahimPhone = new Smartphone("Rahim");
            rahimPhone.InstalledApps.Add("WhatsApp");

            // করিম শ্যালো কপি করে ফোন নিলো
            Smartphone karimPhone = rahimPhone.ShallowClone();
            karimPhone.OwnerName = "Karim";

            // করিম তার ফোনে Facebook ইন্সটল করলো
            karimPhone.InstalledApps.Add("Facebook");

            // রেজাল্ট: করিমের ইন্সটল করা অ্যাপ রহিমের ফোনেও চলে এসেছে! (BUG)
            rahimPhone.PrintStatus(); // Output: WhatsApp, Facebook ❌
            karimPhone.PrintStatus(); // Output: WhatsApp, Facebook


            // -------------------------------------------------------------
            // টেস্ট ২: DEEP COPY (The Fix)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 2. Testing DEEP COPY (The Fix) ---");
            Smartphone hasanPhone = new Smartphone("Hasan");
            hasanPhone.InstalledApps.Add("WhatsApp");

            // রফিক ডিপ কপি করে ফোন নিলো
            Smartphone rafiqPhone = hasanPhone.DeepClone();
            rafiqPhone.OwnerName = "Rafiq";

            // রফিক তার ফোনে Instagram ইন্সটল করলো
            rafiqPhone.InstalledApps.Add("Instagram");

            // রেজাল্ট: রফিকের ইন্সটল করা অ্যাপ শুধু রফিকের ফোনেই আছে! (SAFE)
            hasanPhone.PrintStatus(); // Output: WhatsApp ✅
            rafiqPhone.PrintStatus(); // Output: WhatsApp, Instagram
            
            Console.WriteLine("\n======================================");
        }
    }
}
