using System;

namespace FacadePattern.CombinedWithAbstractFactory
{
    // ==========================================
    // PART 1: ABSTRACT FACTORY PATTERN
    // (অবজেক্ট তৈরি করার জটিলতা লুকানোর জন্য)
    // ==========================================

    // 1. Abstract Products
    public interface ISmartBulb { void TurnOn(string color); }
    public interface ISmartSpeaker { void PlayMusic(string song); }

    // 2. Concrete Products (Apple Ecosystem)
    public class AppleBulb : ISmartBulb 
    { 
        public void TurnOn(string color) => Console.WriteLine($"[Apple HomeKit] Siri turned on the bulb in {color} color."); 
    }
    public class AppleSpeaker : ISmartSpeaker 
    { 
        public void PlayMusic(string song) => Console.WriteLine($"[Apple HomePod] Siri is playing '{song}' on Apple Music."); 
    }

    // 3. Concrete Products (Google Ecosystem)
    public class GoogleBulb : ISmartBulb 
    { 
        public void TurnOn(string color) => Console.WriteLine($"[Google Home] Assistant turned on the bulb in {color} color."); 
    }
    public class GoogleSpeaker : ISmartSpeaker 
    { 
        public void PlayMusic(string song) => Console.WriteLine($"[Google Nest] Assistant is playing '{song}' on YouTube Music."); 
    }

    // 4. Abstract Factory
    public interface ISmartHomeFactory
    {
        ISmartBulb CreateBulb();
        ISmartSpeaker CreateSpeaker();
    }

    // 5. Concrete Factories
    public class AppleHomeFactory : ISmartHomeFactory
    {
        public ISmartBulb CreateBulb() => new AppleBulb();
        public ISmartSpeaker CreateSpeaker() => new AppleSpeaker();
    }

    public class GoogleHomeFactory : ISmartHomeFactory
    {
        public ISmartBulb CreateBulb() => new GoogleBulb();
        public ISmartSpeaker CreateSpeaker() => new GoogleSpeaker();
    }


    // ==========================================
    // PART 2: FACADE PATTERN
    // (অবজেক্ট ব্যবহার করার বা এক্সিকিউট করার জটিলতা লুকানোর জন্য)
    // ==========================================

    public class SmartHomeEcosystemFacade
    {
        // Facade ক্লায়েন্টকে ৫টি আলাদা ক্লাসের কল থেকে বাঁচিয়ে দিচ্ছে!
        public void ActivatePartyMode(string ecosystemName, string songName)
        {
            Console.WriteLine($"\n=== [Facade] Initializing Party Mode for {ecosystemName} Ecosystem ===");

            // Step 1: Facade নিজে ডিসিশন নিচ্ছে কোন Factory ইউজ করতে হবে
            ISmartHomeFactory factory = ecosystemName.ToLower() == "apple" 
                                        ? new AppleHomeFactory() 
                                        : new GoogleHomeFactory();

            // Step 2: Factory কে দিয়ে অবজেক্ট তৈরি করিয়ে নিচ্ছে (Abstract Factory in action)
            ISmartBulb bulb = factory.CreateBulb();
            ISmartSpeaker speaker = factory.CreateSpeaker();

            // Step 3: অবজেক্টগুলোকে একসাথে এক্সিকিউট করছে (Facade in action)
            bulb.TurnOn("Neon Purple");
            speaker.PlayMusic(songName);

            Console.WriteLine("=== [Facade] Party Mode is LIVE! ===\n");
        }
    }


    // ==========================================
    // PART 3: CLIENT CODE
    // ==========================================

    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Masterclass: Facade + Abstract Factory ===\n");

            // ক্লায়েন্ট (ইউজার) এর কাছে কোনো 'new' কিওয়ার্ড নেই, কোনো ফ্যাক্টরি নেই, কোনো লাইট বা স্পিকারের ইন্টারফেসও নেই!
            // সে শুধু Facade কে বলছে: "আমার অ্যাপল সিস্টেমে পার্টি মোড অন করো!"
            SmartHomeEcosystemFacade smartRemote = new SmartHomeEcosystemFacade();

            // ক্লায়েন্ট ১: Apple ইউজার
            smartRemote.ActivatePartyMode("Apple", "Coldplay - A Sky Full of Stars");

            // ক্লায়েন্ট ২: Google ইউজার
            smartRemote.ActivatePartyMode("Google", "Linkin Park - Numb");
            
            /* 
             * ম্যাজিকটা খেয়াল করেছেন?
             * ১. Abstract Factory: অ্যাপল নাকি গুগল—সেই অনুযায়ী সঠিক অবজেক্টগুলো (Bulb, Speaker) তৈরি করার দায়িত্ব নিয়েছে।
             * ২. Facade: অবজেক্টগুলো তৈরি করা থেকে শুরু করে তাদের দিয়ে কাজ করানো—পুরো ওয়ার্কফ্লোটা ক্লায়েন্টের থেকে লুকিয়ে ফেলেছে।
             * ক্লায়েন্ট শুধু জাস্ট একটা সিঙ্গেল মেথড কল করেছে! এটাই রিয়েল-ওয়ার্ল্ড এন্টারপ্রাইজ আর্কিটেকচার!
             */
        }
    }
}
