# Adapter Design Pattern (গল্পে গল্পে)

## বাস্তব জীবনের গল্প: দাদু এবং বিদেশি বন্ধু জন

ধরুন, আপনার একজন বয়স্ক **দাদু** আছেন, যিনি শুধুমাত্র **বাংলা** বোঝেন এবং বাংলায় কথা বলেন। 
অন্যদিকে আপনার একজন বিদেশি বন্ধু আছে, তার নাম **জন (John)**। জন শুধুমাত্র **ইংরেজি** বোঝে। 

এখন দাদু এবং জনের মধ্যে কথা হবে কীভাবে? তারা তো একে অপরের ভাষা বোঝে না! (এখানে দাদু এবং জনের *ইন্টারফেস* আলাদা)।

এই সমস্যা সমাধানের জন্য **আপনি** মাঝখানে দাঁড়ালেন। আপনি বাংলা এবং ইংরেজি—দুটোই জানেন। 
- জন যখন ইংরেজিতে কিছু বলে, আপনি সেটা শুনে বাংলায় অনুবাদ করে দাদুকে বলেন। 
- আবার দাদু যখন বাংলায় কিছু বলে, আপনি সেটা ইংরেজিতে অনুবাদ করে জনকে বলেন। 

এখানে **আপনিই হলেন "Adapter" (অ্যাডাপ্টার)**! আপনি দুই ভিন্ন ভাষার (ইন্টারফেসের) মানুষের মধ্যে সেতুবন্ধন তৈরি করে দিলেন।

## সফটওয়্যারে কীভাবে কাজ করে?

বাস্তব জীবনে অনেক সময় আমরা এমন কোড বা সিস্টেম পাই, যেটা আগে থেকেই লেখা আছে (যেমন বিদেশি বন্ধু জন)। ওই কোডটা আমরা পরিবর্তন করতে পারি না বা করা ঠিক নয়। কিন্তু আমাদের বর্তমান সিস্টেম (যেমন দাদু) সেটার সাথে কথা বলতে চায়। তখন ওই দুটোর মাঝখানে আমরা একটা "Adapter" বা "Translator" বসিয়ে দিই, যাতে আমাদের আগের কোড ভাঙতে না হয়, আবার নতুন কোডও ব্যবহার করা যায়। 

### C# কোড উদাহরণ

```csharp
using System;

namespace BengaliAdapterPattern
{
    // ১. Target: আমাদের দাদু শুধু বাংলা বোঝেন
    public interface IBengaliSpeaker
    {
        void SpeakBengali(string kotha);
    }

    // ২. Adaptee: বিদেশি বন্ধু জন, যে শুধু ইংরেজি বোঝে
    public class EnglishSpeaker
    {
        public void SpeakEnglish(string words)
        {
            Console.WriteLine("John says: " + words);
        }
    }

    // ৩. Adapter: আপনি নিজে (Translator), যে ইংরেজিকে বাংলায় কনভার্ট করে দাদুকে বোঝাবে
    public class TranslatorAdapter : IBengaliSpeaker
    {
        private EnglishSpeaker _john;

        // আপনি জনকে সাথে নিয়ে এসেছেন
        public TranslatorAdapter(EnglishSpeaker john)
        {
            _john = john;
        }

        // দাদু আপনার কাছে বাংলায় কথা শুনতে চাইছেন
        public void SpeakBengali(string kotha)
        {
            // আপনি বুঝতে পারলেন দাদু আসলে জনকে কিছু বলতে বা জনের কথা শুনতে চাইছেন
            // তাই আপনি জনের ইংরেজি কথাটাকে দাদুর জন্য বাংলায় রূপান্তর করছেন
            
            if(kotha == "কেমন আছো?")
            {
                // জনকে ইংরেজিতে বললেন
                _john.SpeakEnglish("How are you?");
                Console.WriteLine("Adapter (আপনি): দাদু, জন ইংরেজিতে বলল 'How are you?', যার মানে 'কেমন আছো?'");
            }
            else
            {
                _john.SpeakEnglish("I don't understand.");
                Console.WriteLine("Adapter (আপনি): দাদু, জন আপনার কথা বুঝতে পারেনি!");
            }
        }
    }

    // মেইন প্রোগ্রাম
    class Program
    {
        static void Main(string[] args)
        {
            // বিদেশি বন্ধু জন আসলো
            EnglishSpeaker john = new EnglishSpeaker();

            // আপনি (Adapter) জনকে দাদুর সাথে কথা বলার জন্য তৈরি করলেন
            IBengaliSpeaker apni = new TranslatorAdapter(john);

            Console.WriteLine("--- দাদু এবং জনের কথোপকথন ---\n");

            // দাদু আপনার মাধ্যমে জনকে জিজ্ঞেস করলেন "কেমন আছো?"
            apni.SpeakBengali("কেমন আছো?");
        }
    }
}
```

## আরও একটি ক্লাসিক উদাহরণ (C# - Printer Example)

আপনার দেওয়া প্রিন্টারের কোডটিকে একটু মডিফাই করে C# এ কনভার্ট করে সাজানো হলো। এখানে আমরা `LegacyPrinter` কে সরাসরি অ্যাডাপ্টারের ভেতর তৈরি না করে, কনস্ট্রাক্টরের মাধ্যমে পাস করেছি (যাকে Dependency Injection বলে)। এতে কোডটি আরও স্ট্যান্ডার্ড হয়েছে এবং একটি `ModernPrinter` ও যোগ করা হয়েছে পার্থক্য বোঝানোর জন্য।

```csharp
using System;

namespace PrinterAdapterExample
{
    // ১. Target Interface (আমাদের বর্তমান সিস্টেম এই ইন্টারফেস ব্যবহার করে)
    public interface IPrinter
    {
        void Print(string text);
    }

    // ২. বর্তমান সিস্টেমের সাথে মানানসই একটি মডার্ন প্রিন্টার
    public class ModernPrinter : IPrinter
    {
        public void Print(string text)
        {
            Console.WriteLine("Modern Printer is printing: " + text);
        }
    }

    // ৩. Adaptee (পুরনো প্রিন্টার, যার মেথড আলাদা এবং আমরা এটি চেঞ্জ করতে পারবো না)
    public class LegacyPrinter
    {
        public void PrintDocument(string text)
        {
            Console.WriteLine("Legacy Printer is slowly printing: " + text);
        }
    }

    // ৪. Adapter (মাঝখানের সেতু, যা পুরনো প্রিন্টারকে নতুন সিস্টেমে মানানসই করবে)
    public class PrinterAdapter : IPrinter
    {
        private LegacyPrinter _legacyPrinter;

        // কনস্ট্রাক্টরের মাধ্যমে পুরনো প্রিন্টারটি নেওয়া হচ্ছে
        public PrinterAdapter(LegacyPrinter legacyPrinter)
        {
            _legacyPrinter = legacyPrinter;
        }

        public void Print(string text)
        {
            // পুরনো প্রিন্টারের PrintDocument মেথডকে কল করে কাজ চালিয়ে নেওয়া হচ্ছে
            _legacyPrinter.PrintDocument(text);
        }
    }

    // Client Code (মেইন প্রোগ্রাম)
    class Program
    {
        // ক্লায়েন্ট শুধু IPrinter ইন্টারফেস চেনে, ভেতরে কোন প্রিন্টার কাজ করছে তা সে জানে না
        public static void ClientCode(IPrinter printer, string text)
        {
            printer.Print(text);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("--- Using Modern Printer ---");
            IPrinter modernPrinter = new ModernPrinter();
            ClientCode(modernPrinter, "Hello Modern World!");

            Console.WriteLine("\n--- Using Legacy Printer via Adapter ---");
            // পুরনো প্রিন্টারকে সরাসরি ClientCode এ পাঠানো যাবে না, তাই Adapter ব্যবহার করছি
            LegacyPrinter oldPrinter = new LegacyPrinter();
            IPrinter adapter = new PrinterAdapter(oldPrinter);
            ClientCode(adapter, "Hello from the past!");
        }
    }
}
```

## মূল কথা (Summary)
অ্যাডাপ্টার ডিজাইন প্যাটার্ন দুটি বেমানান (incompatible) ইন্টারফেসকে একসাথে কাজ করার সুযোগ দেয়। এটি বিদ্যমান কোনো কোডকে পরিবর্তন না করেই নতুন বা পুরনো সিস্টেমের সাথে যুক্ত করার জন্য একটি দারুণ উপায়।

