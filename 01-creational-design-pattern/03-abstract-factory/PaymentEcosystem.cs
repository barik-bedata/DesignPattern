using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Without Abstract Factory)
// ==============================================================
// ভুল পদ্ধতি: Client নিজেই সব অবজেক্ট হার্ডকোড করে তৈরি করছে।
// সমস্যা: 
// ১. Inconsistent Family: ভুল করে বিকাশের পেমেন্টের সাথে নগদের রিসিট জেনারেট হয়ে যেতে পারে।
// ২. Open/Closed Principle ভাঙা: নতুন ব্র্যান্ড (যেমন- Rocket) যোগ করতে হলে মেইন ক্লাসের if-else বিশাল হয়ে যাবে।

namespace AbstractFactoryPattern.Violation
{
    // ১. প্রোডাক্টের কন্ট্রাক্ট
    public interface IPayment 
    { 
        void Process(decimal amount); 
    }

    public interface IReceipt 
    { 
        void Generate(decimal amount); 
    }

    public interface INotification 
    { 
        void Send(string message); 
    }

    // ২. আসল প্রোডাক্ট ক্লাসগুলো (ভায়োলেশন বোঝানোর জন্য)
    
    // ----- Nagad Family -----
    public class NagadPayment : IPayment 
    { 
        public void Process(decimal amount)
        {
            Console.WriteLine($"[ন নগদ] {amount} টাকা কাটা হলো");
        }
    }

    public class NagadReceipt : IReceipt 
    { 
        public void Generate(decimal amount) 
        {
            Console.WriteLine($"[নগদ রিসিট] লেনদেন সম্পন্ন: {amount} টাকা"); 
        }
    }

    public class NagadNotification : INotification 
    { 
        public void Send(string message) 
        {
            Console.WriteLine($"[নগদ SMS] {message}"); 
        }
    }

    // ----- bKash Family -----
    public class bKashPayment : IPayment 
    { 
        public void Process(decimal amount) 
        {
            Console.WriteLine($"[বিকাশ] {amount} টাকা কাটা হলো"); 
        }
    }

    public class bKashReceipt : IReceipt 
    { 
        public void Generate(decimal amount) 
        {
            Console.WriteLine($"[বিকাশ রিসিট] লেনদেন সম্পন্ন: {amount} টাকা"); 
        }
    }

    public class bKashNotification : INotification 
    { 
        public void Send(string message) 
        {
            Console.WriteLine($"[বিকাশ Push] {message}"); 
        }
    }

    // ৩. Client Class (The God Class / The Problem)
    public class BadCheckoutService
    {
        public void Checkout(string brand, decimal amount)
        {
            if (brand == "Nagad")
            {
                var payment = new NagadPayment();
                var receipt = new NagadReceipt();
                var notification = new NagadNotification();
                
                payment.Process(amount);
                receipt.Generate(amount);
                notification.Send($"{amount} টাকা লেনদেন হয়েছে।");
            }
            else if (brand == "bKash")
            {
                var payment = new bKashPayment();
                
                // ❌ মারাত্মক ভুল: কপি-পেস্ট করতে গিয়ে বিকাশের পেমেন্টে নগদের রিসিট চলে আসতে পারে!
                var receipt = new NagadReceipt(); 
                var notification = new bKashNotification();
                
                payment.Process(amount);
                receipt.Generate(amount);
                notification.Send($"{amount} টাকা লেনদেন হয়েছে।");
            }
        }
    }

    // এই ক্লাসটি রান করে ভায়োলেশনের ভুলটি দেখাবে
    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: বিকাশ দিয়ে পেমেন্ট করার চেষ্টা ===");
            var service = new BadCheckoutService();
            // এখানে বিকাশের পেমেন্ট হবে ঠিকই, কিন্তু রিসিট জেনারেট হবে নগদের! (Inconsistent Family)
            service.Checkout("bKash", 1000); 
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Using Abstract Factory)
// ==============================================================
// সমাধান: ফ্যাক্টরি পুরো "ফ্যামিলি" বানানোর গ্যারান্টি দেয়, তাই মিক্স-আপ হওয়ার কোনো সুযোগ নেই।

namespace AbstractFactoryPattern.Solution
{
    // 🧱 অ্যাবস্ট্রাক্ট ফ্যাক্টরির ৫টি মূল কম্পোনেন্ট:

    // ১. Abstract Product: প্রতিটা "ধরনের" জিনিসের contract (যেমন- IPayment, IReceipt, INotification)
    public interface IPayment 
    { 
        void Process(decimal amount); 
    }

    public interface IReceipt 
    { 
        void Generate(decimal amount); 
    }

    public interface INotification 
    { 
        void Send(string message); 
    }

    // ২. Concrete Product: আসল প্রোডাক্ট ক্লাসগুলো (Nagad পরিবার)
    public class NagadPayment : IPayment 
    { 
        public void Process(decimal amount) 
        {
            Console.WriteLine($"[নগদ] {amount} টাকা কাটা হলো (01XXXXXXXXX)"); 
        }
    }

    public class NagadReceipt : IReceipt 
    { 
        public void Generate(decimal amount) 
        {
            Console.WriteLine($"[নগদ রিসিট] লেনদেন সম্পন্ন: {amount} টাকা — TXN#NGD001"); 
        }
    }

    public class NagadNotification : INotification 
    { 
        public void Send(string message) 
        {
            Console.WriteLine($"[নগদ SMS] আপনার মোবাইলে পাঠানো হলো: {message}"); 
        }
    }

    // (Concrete Product-এর অংশ) আসল প্রোডাক্ট ক্লাসগুলো (bKash পরিবার)
    public class bKashPayment : IPayment 
    { 
        public void Process(decimal amount) 
        {
            Console.WriteLine($"[বিকাশ] {amount} টাকা কাটা হলো (+88017XXXXXXXX)"); 
        }
    }

    public class bKashReceipt : IReceipt 
    { 
        public void Generate(decimal amount) 
        {
            Console.WriteLine($"[বিকাশ রিসিট] লেনদেন সম্পন্ন: {amount} টাকা — TXN#BKS001"); 
        }
    }

    public class bKashNotification : INotification 
    { 
        public void Send(string message) 
        {
            Console.WriteLine($"[বিকাশ Push] অ্যাপে নোটিফিকেশন: {message}"); 
        }
    }

    // ৩. Abstract Factory: "পুরো পরিবার বানানোর চুক্তি" (Factory Interface containing creation methods)
    public interface IPaymentFactory
    {
        IPayment CreatePayment();
        IReceipt CreateReceipt();
        INotification CreateNotification();
    }

    // ৪. Concrete Factory: প্রতিটা নিজের পরিবার বানায় (আসল ফ্যাক্টরি ক্লাসগুলো)
    public class NagadFactory : IPaymentFactory
    {
        public IPayment CreatePayment() 
        {
            return new NagadPayment();
        }

        public IReceipt CreateReceipt() 
        {
            return new NagadReceipt();
        }

        public INotification CreateNotification() 
        {
            return new NagadNotification();
        }
    }

    public class bKashFactory : IPaymentFactory
    {
        public IPayment CreatePayment() 
        {
            return new bKashPayment();
        }

        public IReceipt CreateReceipt() 
        {
            return new bKashReceipt();
        }

        public INotification CreateNotification() 
        {
            return new bKashNotification();
        }
    }

    // ৫. Client: factory কে চেনে, কিন্তু কোন brand সেটা জানে না এবং জানতে চায় না (প্যাটার্নের ভাষায় Client)
    // Abstract Factory-এর Client
    public class CheckoutService
    {
        private readonly IPayment _payment;
        private readonly IReceipt _receipt;
        private readonly INotification _notification;

        // Factory inject করো — ব্যস, পুরো পরিবার ঠিক হয়ে গেল
        public CheckoutService(IPaymentFactory factory)
        {
            _payment = factory.CreatePayment();
            _receipt = factory.CreateReceipt();
            _notification = factory.CreateNotification();
        }

        public void Checkout(decimal amount)
        {
            _payment.Process(amount);
            _receipt.Generate(amount);
            _notification.Send($"{amount} টাকার লেনদেন সফল হয়েছে।");
        }
    }

    // 💻 Application Client: সলিউশন রান করার ক্লাস (পুরো অ্যাপ্লিকেশনের আল্টিমেট ক্লায়েন্ট)
    // Puro program er client
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: নগদ দিয়ে Checkout ===");
            var nagadService = new CheckoutService(new NagadFactory());
            nagadService.Checkout(750);

            Console.WriteLine("\n=== ✅ SOLUTION RUN: বিকাশ দিয়ে Checkout ===");
            var bkashService = new CheckoutService(new bKashFactory());
            bkashService.Checkout(1200);
        }
    }
}


// ══════════════════════════════════════════
// 🚀 Main Entry Point (এখান থেকেই প্রোগ্রাম রান হবে)
// ══════════════════════════════════════════
class Program
{
    static void Main()
    {
        // প্রথমে ভায়োলেশন রান করে দেখবো ভুলটা কী হয়
        AbstractFactoryPattern.Violation.ViolationRunner.Run();
        
        // এরপর সলিউশন রান করে দেখবো কীভাবে পারফেক্ট কাজ করে
        AbstractFactoryPattern.Solution.SolutionRunner.Run();
    }
}
