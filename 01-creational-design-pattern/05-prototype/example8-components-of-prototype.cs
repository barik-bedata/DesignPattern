using System;
using System.Threading;

namespace PrototypePattern.Components
{
    // ==============================================================
    // ১. Prototype Interface (প্রোটোটাইপ ইন্টারফেস)
    // ==============================================================
    // এটি সেই ইন্টারফেস বা কন্ট্রাক্ট, যা বলে দেয় যে যেকোনো শেপ (Shape) 
    // ক্লাসে একটি Clone() মেথড থাকতে হবে।
    public interface IShape
    {
        IShape Clone();
        void Draw();
    }

    // ==============================================================
    // ২. Concrete Prototype (কংক্রিট প্রোটোটাইপ)
    // ==============================================================
    // এটি হলো আসল ক্লাস (যেমন Circle) যা IShape ইন্টারফেসকে ইমপ্লিমেন্ট করে 
    // এবং নিজের একটি কপি তৈরি করার লজিক (MemberwiseClone) লিখে রাখে।
    public class Circle : IShape
    {
        public string Color { get; set; }
        public int Radius { get; set; }

        public Circle(string color, int radius)
        {
            Console.WriteLine($"   -> [Expensive Operation] Creating a new {color} Circle of radius {radius}...");
            Thread.Sleep(1000); // সিমুলেট করা হচ্ছে যে এটি বানাতে অনেক সময় লাগে
            
            Color = color;
            Radius = radius;
        }

        // यहीं (এখানেই) আসল কপি করার কাজটা হচ্ছে
        public IShape Clone()
        {
            Console.WriteLine($"   -> [Cloning] Quickly making a copy of the {Color} Circle...");
            return (IShape)this.MemberwiseClone();
        }

        public void Draw()
        {
            Console.WriteLine($"[Draw] I am a {Color} Circle with radius {Radius}");
        }
    }

    // ==============================================================
    // ৩. Client (ক্লায়েন্ট)
    // ==============================================================
    // ক্লায়েন্ট হলো সেই ক্লাস, যে প্রোটোটাইপ ইন্টারফেস (IShape) ব্যবহার করে 
    // নতুন অবজেক্টের কপি চায়। সে জানে না ভেতরে Circle কিভাবে কপি হচ্ছে।
    // সে শুধু জানে "আমাকে একটি কপি দাও"।
    public class ShapeClient
    {
        private IShape _shapePrototype;

        // ক্লায়েন্টকে শুরুতে একটি মাস্টার কপি বা ডেমো ধরিয়ে দেওয়া হয়
        public ShapeClient(IShape shapePrototype)
        {
            _shapePrototype = shapePrototype;
        }

        // ক্লায়েন্ট সেই মাস্টার কপি থেকে নতুন কপি তৈরি করে দেয়
        public IShape CreateShape()
        {
            return _shapePrototype.Clone();
        }
    }

    // ==============================================================
    // ৪. Main Class (মেইন ক্লাস)
    // ==============================================================
    // এখান থেকে প্রোগ্রাম রান হয়। এখানে আমরা ক্লায়েন্টকে ব্যবহার করে 
    // মাস্টার কপি থেকে অনেকগুলো কপি বানাবো।
    public class BasicShapeRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== Prototype Pattern Components Demo (Basic) ===\n");

            // ধাপ ১: আমরা একটি 'মাস্টার' সার্কেল বানালাম (এটি বানাতে ১ সেকেন্ড সময় লাগবে)
            Circle masterCircle = new Circle("Red", 10);

            // ধাপ ২: ক্লায়েন্টকে মাস্টার সার্কেলটি দিয়ে দিলাম
            ShapeClient client = new ShapeClient(masterCircle);

            Console.WriteLine("\n--- Now cloning from Client ---");

            // ধাপ ৩: ক্লায়েন্টের কাছ থেকে কপি চাচ্ছি (কোনো সময় লাগবে না, মিলি-সেকেন্ডে হবে)
            Circle clonedCircle1 = (Circle)client.CreateShape();
            clonedCircle1.Draw();

            // আরেকটা কপি নিয়ে তার কালার চেঞ্জ করে দিলাম
            Circle clonedCircle2 = (Circle)client.CreateShape();
            clonedCircle2.Color = "Blue"; 
            clonedCircle2.Draw();

            // প্রমাণ যে মাস্টার সার্কেল অক্ষত আছে
            Console.WriteLine("\n--- Checking Master ---");
            masterCircle.Draw();
        }
    }
}

namespace PrototypePattern.Components.RealWorld
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    // এটি কন্ট্রাক্ট যা গ্যারান্টি দেয় যে যেকোনো আইডি কার্ড কপি করা যাবে।
    public interface IIdCard
    {
        IIdCard Clone();
        void PrintCard();
    }

    // ==============================================================
    // ২. Concrete Prototype (আসল ক্লাস)
    // ==============================================================
    // এটি আসল আইডি কার্ড, যা বানাতে শুরুতে একটু সময় লাগে।
    public class SchoolIdCard : IIdCard
    {
        public string SchoolName { get; set; }
        public string PrincipalSignature { get; set; }
        public string StudentName { get; set; }

        public SchoolIdCard()
        {
            Console.WriteLine("\n   -> [Expensive] Loading School Logo and Principal Signature from Database...");
            Thread.Sleep(1000); 
            
            SchoolName = "Dhaka Zilla School";
            PrincipalSignature = "Signed_By_Headmaster";
        }

        // এখানেই কপি করার ম্যাজিকটা আছে
        public IIdCard Clone()
        {
            return (IIdCard)this.MemberwiseClone();
        }

        public void PrintCard()
        {
            Console.WriteLine($"[ID CARD] {StudentName} | {SchoolName} | {PrincipalSignature}");
        }
    }

    // ==============================================================
    // ৩. Client (ID Card Printer)
    // ==============================================================
    // ক্লায়েন্ট (প্রিন্টার) শুধু জানে তার কাছে একটি মাস্টার কপি আছে। 
    // সে মাস্টার কপিকে কল করে শুধু নতুন কার্ড প্রিন্ট করে দেয়। 
    public class IdCardPrinter
    {
        private IIdCard _masterCard;

        // প্রিন্টার মেশিনে মাস্টার কপি সেট করা হলো
        public IdCardPrinter(IIdCard masterCard)
        {
            _masterCard = masterCard;
        }

        // ক্লায়েন্ট নতুন ছাত্রের নাম নিয়ে মাস্টার কপি ক্লোন করে রিটার্ন করছে
        public IIdCard PrintNewCard(string studentName)
        {
            SchoolIdCard newCard = (SchoolIdCard)_masterCard.Clone();
            newCard.StudentName = studentName;
            return newCard;
        }
    }

    public class RealWorldRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== Prototype Pattern Components Demo (Real World: School ID Card) ===\n");

            // ধাপ ১: মাস্টার আইডি কার্ড বানালাম (সময় লাগবে)
            SchoolIdCard masterCard = new SchoolIdCard();

            // ধাপ ২: প্রিন্টার (ক্লায়েন্ট) এর ভেতরে মাস্টার কার্ডটি সেট করে দিলাম
            IdCardPrinter printer = new IdCardPrinter(masterCard);

            Console.WriteLine("\n--- Printing ID Cards ---");

            // ধাপ ৩: প্রিন্টার (ক্লায়েন্ট) কে দিয়ে নতুন নতুন কার্ড বানাচ্ছি (কোনো সময় লাগবে না)
            IIdCard student1 = printer.PrintNewCard("Rahim");
            student1.PrintCard();

            IIdCard student2 = printer.PrintNewCard("Karim");
            student2.PrintCard();
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
        PrototypePattern.Components.BasicShapeRunner.Run();
        PrototypePattern.Components.RealWorld.RealWorldRunner.Run();
    }
}
