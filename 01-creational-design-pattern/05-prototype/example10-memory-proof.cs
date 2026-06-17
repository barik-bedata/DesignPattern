using System;
using System.Collections.Generic;

namespace PrototypePattern.MemoryProof
{
    // ==============================================================
    // ১. Prototype Interface (প্রোটোটাইপ ইন্টারফেস)
    // ==============================================================
    // এই ইন্টারফেস বলে দিচ্ছে যে যেকোনো স্মার্টফোন ক্লোন করা যাবে।
    public interface ISmartphone
    {
        ISmartphone Clone();
        void InstallApp(string appName);
        void SetBattery(int percentage);
    }

    // ==============================================================
    // ২. Concrete Prototype (কংক্রিট প্রোটোটাইপ)
    // ==============================================================
    // এটি আসল স্মার্টফোন ক্লাস। এর ভেতরে Primitive এবং Reference ডাটা আছে।
    public class IPhone : ISmartphone
    {
        // Primitive Data (কপি করলে সম্পূর্ণ আলাদা মেমোরি পাবে)
        public int BatteryLife { get; private set; }
        public string ModelName { get; private set; }

        // Reference Data (List) - (Shallow Copy করলে মেমোরি অ্যাড্রেস শেয়ার হবে!)
        public List<string> InstalledApps { get; private set; }

        public IPhone(string modelName)
        {
            ModelName = modelName;
            BatteryLife = 100;
            InstalledApps = new List<string>();
        }

        public void InstallApp(string appName)
        {
            InstalledApps.Add(appName);
        }

        public void SetBattery(int percentage)
        {
            BatteryLife = percentage;
        }

        // Shallow Copy করার লজিক এখানেই আছে
        public ISmartphone Clone()
        {
            return (ISmartphone)this.MemberwiseClone();
        }
    }

    // ==============================================================
    // ৩. Client (ক্লায়েন্ট)
    // ==============================================================
    // ক্লায়েন্ট (যেমন PhoneStore) জানে না ভেতরে কীভাবে মেমোরি কপি হচ্ছে।
    // সে শুধু ইন্টারফেসের Clone() মেথড কল করে নতুন ফোন চায়।
    public class PhoneStore
    {
        private ISmartphone _masterPhone;

        public PhoneStore(ISmartphone masterPhone)
        {
            _masterPhone = masterPhone;
        }

        // ক্লায়েন্ট মাস্টার থেকে নতুন কপি বের করে দেয়
        public ISmartphone GetNewPhone()
        {
            return _masterPhone.Clone();
        }
    }

    // ==============================================================
    // ৪. Main Class (মেইন ক্লাস)
    // ==============================================================
    // এখান থেকে প্রোগ্রাম রান হচ্ছে এবং মেমোরির প্রমাণ দেখা হচ্ছে।
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Memory Proof: Shallow Copy (Using 4 Components) ===\n");

            // ধাপ ১: কংক্রিট মাস্টার অবজেক্ট তৈরি করলাম
            IPhone masterIphone = new IPhone("iPhone 15");
            masterIphone.InstallApp("WhatsApp");

            // ধাপ ২: ক্লায়েন্টের (PhoneStore) কাছে মাস্টার ফোনটি দিয়ে দিলাম
            PhoneStore store = new PhoneStore(masterIphone);

            // ধাপ ৩: ক্লায়েন্টকে বললাম "আরেকটি নতুন ফোন কপি করে দাও"
            IPhone clonedPhone = (IPhone)store.GetNewPhone();

            // -------------------------------------------------------------
            // প্রমাণ ১: মেমোরিতে কি নতুন অবজেক্ট তৈরি হয়েছে?
            // -------------------------------------------------------------
            bool isSamePhoneInMemory = Object.ReferenceEquals(masterIphone, clonedPhone);
            Console.WriteLine($"[Proof 1] Are masterIphone and clonedPhone exactly the same memory object? -> {isSamePhoneInMemory}");
            // Output: False. অর্থাৎ, মেমোরিতে ১০০% নতুন একটি অবজেক্ট তৈরি হয়েছে!

            Console.WriteLine("\n--- Modifying the Cloned Phone ---");
            
            // ক্লোন করা ফোনের ব্যাটারি কমিয়ে দিলাম (Primitive Data)
            clonedPhone.SetBattery(50); 
            
            // ক্লোন করা ফোনে নতুন অ্যাপ ইন্সটল করলাম (Reference Data)
            clonedPhone.InstallApp("Facebook"); 

            // -------------------------------------------------------------
            // প্রমাণ ২: Primitive Type (Battery) এর কী অবস্থা?
            // -------------------------------------------------------------
            Console.WriteLine($"\n[Proof 2 - Primitive Data (Battery)]");
            Console.WriteLine($"Master Phone Battery: {masterIphone.BatteryLife}%"); // Output: 100
            Console.WriteLine($"Cloned Phone Battery: {clonedPhone.BatteryLife}%"); // Output: 50
            Console.WriteLine("-> Conclusion: Primitive Data সম্পূর্ণ আলাদা! একটির পরিবর্তনে অন্যটির ক্ষতি হয়নি।");

            // -------------------------------------------------------------
            // প্রমাণ ৩: Reference Type (List) এর কী অবস্থা?
            // -------------------------------------------------------------
            Console.WriteLine($"\n[Proof 3 - Reference Data (List)]");
            Console.WriteLine($"Master Phone Apps: {string.Join(", ", masterIphone.InstalledApps)}"); // Output: WhatsApp, Facebook
            Console.WriteLine($"Cloned Phone Apps: {string.Join(", ", clonedPhone.InstalledApps)}"); // Output: WhatsApp, Facebook
            Console.WriteLine("-> DANGER! List এর মেমোরি শেয়ার হচ্ছে (Shallow Copy)! আমরা শুধু Cloned ফোনে Facebook ইন্সটল করেছিলাম, কিন্তু Master ফোনেও সেটা চলে এসেছে!");
            
            Console.WriteLine("\n=== END OF PROOF ===");
        }
    }
}
