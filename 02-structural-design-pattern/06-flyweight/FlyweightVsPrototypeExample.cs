using System;
using System.Collections.Generic;

namespace FlyweightPattern.Comparison
{
    // =======================================================
    // 💡 PROTOTYPE PATTERN (The Photocopy Machine)
    // =======================================================
    // উদ্দেশ্য: ডাটাবেস বা নেটওয়ার্ক কল বাঁচিয়ে দ্রুত অবজেক্ট তৈরি করা (Save CPU)।
    // মেমোরি: প্রতিটি ক্লোন সম্পূর্ণ নতুন মেমোরি জায়গা দখল করে।
    public class PrototypeDocument : ICloneable
    {
        public string Content { get; set; } // এটি পরিবর্তনযোগ্য (Mutable)

        public PrototypeDocument(string content)
        {
            Content = content;
            Console.WriteLine($"[Prototype] Heavy DB Call: Created original document.");
        }

        public object Clone()
        {
            // Shallow Copy: মেমোরিতে নতুন অবজেক্ট তৈরি হচ্ছে।
            Console.WriteLine($"[Prototype] Cloned document from memory (Fast!).");
            return this.MemberwiseClone(); 
        }
    }


    // =======================================================
    // 💡 FLYWEIGHT PATTERN (The Library Book)
    // =======================================================
    // উদ্দেশ্য: একই অবজেক্ট বারবার তৈরি না করে সবার মাঝে শেয়ার করা (Save RAM)।
    // মেমোরি: লাখ লাখ ক্লায়েন্ট চাইলেও মেমোরিতে মাত্র ১টি অবজেক্ট থাকে।
    public class FlyweightBook
    {
        public string Title { get; private set; } // এটি পরিবর্তনযোগ্য নয় (Immutable)

        public FlyweightBook(string title)
        {
            Title = title;
            Console.WriteLine($"[Flyweight] Created '{title}' book in Library (Takes 50MB RAM).");
        }

        public void Read(string studentName)
        {
            // ক্লায়েন্ট নিজের ইউনিক ডেটা (Extrinsic State) মেথডের মাধ্যমে পাস করে
            Console.WriteLine($"[Flyweight] Student '{studentName}' is reading the shared '{Title}' book.");
        }
    }

    public class LibraryFactory
    {
        private Dictionary<string, FlyweightBook> _books = new Dictionary<string, FlyweightBook>();

        public FlyweightBook GetBook(string title)
        {
            if (!_books.ContainsKey(title))
            {
                _books[title] = new FlyweightBook(title);
            }
            return _books[title];
        }
    }


    // =======================================================
    // 🚀 CLIENT CODE (Comparison Test)
    // =======================================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== PROTOTYPE VS FLYWEIGHT ===\n");

            // ---------------------------------------------------
            // Test 1: Prototype (Photocopy)
            // ---------------------------------------------------
            Console.WriteLine("--- Testing Prototype ---");
            PrototypeDocument originalDoc = new PrototypeDocument("Secret Data");
            
            PrototypeDocument clone1 = (PrototypeDocument)originalDoc.Clone();
            PrototypeDocument clone2 = (PrototypeDocument)originalDoc.Clone();

            // প্রমাণ: ক্লোন ১ চেঞ্জ করলে ক্লোন ২ চেঞ্জ হবে না! কারণ তারা আলাদা অবজেক্ট (Shallow Copy হলেও ভ্যালু টাইপের জন্য আলাদা)।
            clone1.Content = "Modified Data by Clone1";
            Console.WriteLine($"Original: {originalDoc.Content}");
            Console.WriteLine($"Clone1: {clone1.Content}"); // Changed!
            Console.WriteLine($"Clone2: {clone2.Content}"); // Not changed!
            
            // Result: মেমোরিতে ৩টি আলাদা Document অবজেক্ট! (Original, Clone1, Clone2)


            Console.WriteLine("\n--- Testing Flyweight ---");
            // ---------------------------------------------------
            // Test 2: Flyweight (Shared Reference)
            // ---------------------------------------------------
            LibraryFactory library = new LibraryFactory();

            FlyweightBook bookForRahim = library.GetBook("Design Patterns");
            FlyweightBook bookForKarim = library.GetBook("Design Patterns");
            FlyweightBook bookForJodu = library.GetBook("Design Patterns");

            bookForRahim.Read("Rahim");
            bookForKarim.Read("Karim");
            bookForJodu.Read("Jodu");

            // প্রমাণ: এখানে বই চেঞ্জ করার কোনো অপশনই নেই (Title is private set / Immutable)।
            // Result: রহিম, করিম, যদু ৩ জন পড়লেও মেমোরিতে Book অবজেক্ট মাত্র ১টি! (Design Patterns)

            // ---------------------------------------------------
            // ⚠️ WHY NO PUBLIC SETTER IN FLYWEIGHT? (Setter রাখা মহাপাপ কেন?)
            // ---------------------------------------------------
            // যদি FlyweightBook ক্লাসে 'public set' থাকতো এবং রহিম কোডে লিখতো: 
            // bookForRahim.Title = "Comic Book";
            // তাহলে যেহেতু সবাই একই মেমোরি অ্যাড্রেস শেয়ার করছে, সাথে সাথেই করিম এবং যদুর বইয়ের 
            // নামও চেঞ্জ হয়ে "Comic Book" হয়ে যেতো! এই ভয়ংকর বাগ (Side-effect) এড়ানোর জন্যই 
            // ফ্লাইওয়েট প্যাটার্নে শেয়ার্ড অবজেক্টকে কঠোরভাবে Immutable (private set) রাখা হয়।
        }
    }
}
