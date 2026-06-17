using System;
using System.Collections.Generic;

namespace PrototypePattern.DeepVsShallow
{
    // ==============================================================
    // ১. Prototype Interface (প্রোটোটাইপ ইন্টারফেস)
    // ==============================================================
    // এই ইন্টারফেসে বলে দেওয়া হচ্ছে যে ফোনকে দুইভাবেই ক্লোন করা যাবে।
    public interface ISmartphone
    {
        ISmartphone ShallowClone();
        ISmartphone DeepClone();
        void SetOwner(string ownerName);
        void InstallApp(string appName);
        void PrintStatus();
    }

    // ==============================================================
    // ২. Concrete Prototype (কংক্রিট প্রোটোটাইপ)
    // ==============================================================
    // এটি আসল স্মার্টফোন ক্লাস। এর ভেতরে Primitive এবং Reference ডাটা আছে।
    public class IPhone : ISmartphone
    {
        // Primitive Data (আলাদা মেমোরি পাবে)
        public string OwnerName { get; private set; }
        
        // Reference Data (List - মেমোরি শেয়ার হতে পারে)
        public List<string> InstalledApps { get; private set; }

        public IPhone(string ownerName)
        {
            OwnerName = ownerName;
            InstalledApps = new List<string>();
        }

        public void SetOwner(string ownerName)
        {
            OwnerName = ownerName;
        }

        public void InstallApp(string appName)
        {
            InstalledApps.Add(appName);
        }

        // --------------------------------------------------
        // Shallow Copy (অগভীর কপি - যেখানে বাগ তৈরি হয়)
        // --------------------------------------------------
        public ISmartphone ShallowClone()
        {
            // MemberwiseClone শুধু মেইন অবজেক্ট বানায়, কিন্তু লিস্টের মেমোরি শেয়ার করে দেয়।
            return (ISmartphone)this.MemberwiseClone();
        }

        // --------------------------------------------------
        // Deep Copy (গভীর কপি - যেখানে বাগ ফিক্স করা হয়েছে)
        // --------------------------------------------------
        public ISmartphone DeepClone()
        {
            // ১. প্রথমে মেইন অবজেক্ট কপি করলাম (Shallow Copy এর মতো)
            IPhone copy = (IPhone)this.MemberwiseClone();
            
            // ২. বাগ ফিক্স: লিস্টের মেমোরি শেয়ারিং ভেঙে দিলাম!
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
    // ৩. Client (ক্লায়েন্ট)
    // ==============================================================
    // ক্লায়েন্ট (যেমন PhoneStore) জানে না ভেতরে কীভাবে মেমোরি কপি হচ্ছে।
    // সে শুধু ইন্টারফেসের Clone মেথডগুলো কল করে নতুন ফোন সেল করে।
    public class PhoneStore
    {
        private ISmartphone _masterPhone;

        public PhoneStore(ISmartphone masterPhone)
        {
            _masterPhone = masterPhone;
        }

        // ক্লায়েন্ট বাগযুক্ত (Shallow) কপি সেল করছে
        public ISmartphone SellPhoneWithBug()
        {
            return _masterPhone.ShallowClone();
        }

        // ক্লায়েন্ট ফিক্সড (Deep) কপি সেল করছে
        public ISmartphone SellPhoneSafely()
        {
            return _masterPhone.DeepClone();
        }
    }

    // ==============================================================
    // ৪. Main Class (মেইন ক্লাস)
    // ==============================================================
    // এখান থেকে প্রোগ্রাম রান হচ্ছে এবং ক্লায়েন্টের মাধ্যমে ফোন বিক্রি করা হচ্ছে।
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Deep Copy vs Shallow Copy Demo (Using 4 Components) ===\n");

            // ধাপ ১: মাস্টার আইফোন তৈরি করলাম
            IPhone masterIphone = new IPhone("Master");
            masterIphone.InstallApp("WhatsApp");

            // ধাপ ২: ক্লায়েন্টের (PhoneStore) কাছে মাস্টার ফোনটি দিয়ে দিলাম
            PhoneStore store = new PhoneStore(masterIphone);


            // -------------------------------------------------------------
            // টেস্ট ১: SHALLOW COPY (The Bug)
            // -------------------------------------------------------------
            Console.WriteLine("--- 1. Testing SHALLOW COPY (The Bug) ---");
            
            // রহিম মাস্টার ফোনের একটি শ্যালো কপি কিনলো
            ISmartphone rahimPhone = store.SellPhoneWithBug();
            rahimPhone.SetOwner("Rahim");

            // করিম আরেকটি শ্যালো কপি কিনলো
            ISmartphone karimPhone = store.SellPhoneWithBug();
            karimPhone.SetOwner("Karim");

            // করিম তার ফোনে Facebook ইন্সটল করলো
            karimPhone.InstallApp("Facebook");

            // রেজাল্ট: করিমের ইন্সটল করা অ্যাপ রহিমের ফোনেও চলে এসেছে! (BUG)
            rahimPhone.PrintStatus(); // Output: WhatsApp, Facebook ❌
            karimPhone.PrintStatus(); // Output: WhatsApp, Facebook


            // -------------------------------------------------------------
            // টেস্ট ২: DEEP COPY (The Fix)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- 2. Testing DEEP COPY (The Fix) ---");
            
            // হাসান একটি নতুন মাস্টার ফোনের ডিপ কপি কিনলো
            ISmartphone hasanPhone = store.SellPhoneSafely();
            hasanPhone.SetOwner("Hasan");

            // রফিক আরেকটি ডিপ কপি কিনলো
            ISmartphone rafiqPhone = store.SellPhoneSafely();
            rafiqPhone.SetOwner("Rafiq");

            // রফিক তার ফোনে Instagram ইন্সটল করলো
            rafiqPhone.InstallApp("Instagram");

            // রেজাল্ট: রফিকের ইন্সটল করা অ্যাপ শুধু রফিকের ফোনেই আছে! (SAFE)
            hasanPhone.PrintStatus(); // Output: WhatsApp ✅
            rafiqPhone.PrintStatus(); // Output: WhatsApp, Instagram
            
            Console.WriteLine("\n======================================");
        }
    }
}
