using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Without Factory Method)
// ==============================================================
// ভুল পদ্ধতি: Client নিজেই অবজেক্ট তৈরি করার দায়িত্ব নিচ্ছে।
// 
// এর ফলে প্রধানত ৪টি বড় সমস্যা (Violations) তৈরি হয়:
// ১. Open/Closed Principle (OCP) ভাঙা: নতুন পেমেন্ট (যেমন Rocket) যোগ করতে হলে এই ক্লাসের if-else মডিফাই করতে হবে।
// ২. Tight Coupling: Client ক্লাসটি সরাসরি NagadPayment এবং BkashPayment এর উপর নির্ভরশীল।
// ৩. Spaghetti Code: অনেক পেমেন্ট মেথড আসলে এই কনস্ট্রাক্টরের if-else বিশাল বড় ও জটিল হয়ে যাবে।
// ৪. Hard to Test: এই ক্লাসের Unit Testing করার সময় Mock অবজেক্ট পাঠানো প্রায় অসম্ভব।

namespace FactoryMethodViolation
{
    // ১. Common Abstract Class
    public abstract class Payment
    {
        public abstract void ProcessPayment(decimal amount);
    }

    // ২. Concrete Classes
    public class NagadPayment : Payment
    {
        public override void ProcessPayment(decimal amount) => Console.WriteLine($"❌ [Violation] নগদে {amount} টাকা পাঠানো হলো");
    }

    public class BkashPayment : Payment
    {
        public override void ProcessPayment(decimal amount) => Console.WriteLine($"❌ [Violation] বিকাশে {amount} টাকা পাঠানো হলো");
    }

    // ৩. Client Class (The God Class / The Problem)
    public class PaymentClient
    {
        private Payment _payment;

        public PaymentClient(int type)
        {
            // ❌ Violation: Client ক্লাস নিজেই হার্ডকোড করে অবজেক্ট তৈরি করছে।
            // "new" কি-ওয়ার্ডের ব্যবহার এবং if-else এর ছড়াছড়ি। 
            // Rocket যোগ করতে হলে এই কনস্ট্রাক্টর আবার খুলতে হবে!
            if (type == 1) {
                _payment = new NagadPayment();
            }
            else if (type == 2) {
                _payment = new BkashPayment();
            }
            else {
                _payment = null;
            }
        }

        public Payment GetPayment() 
        { 
            return _payment; 
        }
    }

    // ৪. Driver Code (Client ব্যবহারকারী)
    public class ViolationProgram
    {
        public static void Run()
        {
            PaymentClient client = new PaymentClient(1);
            Payment payment = client.GetPayment();
            
            if (payment != null) {
                payment.ProcessPayment(500);
            }
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Using Factory Method)
// ==============================================================
// 🧱 ফ্যাক্টরি মেথডের ৪টি মূল কম্পোনেন্ট:

// ১. Product: সব payment-ই এটা মানবে (Common Interface)
public interface IPayment
{
    void ProcessPayment(decimal amount);
    void Refund(decimal amount);
}

// ২. Concrete Products: প্রতিটা gateway নিজের কাজ নিজে করে (আসল প্রোডাক্ট ক্লাসগুলো)
public class NagadPayment : IPayment
{
    public void ProcessPayment(decimal amount) =>
        Console.WriteLine($"✅ নগদ: {amount} টাকা পাঠানো হলো (01XXXXXXXXX)");

    public void Refund(decimal amount) =>
        Console.WriteLine($"↩️ নগদ: {amount} টাকা ফেরত দেওয়া হলো");
}

public class bKashPayment : IPayment
{
    public void ProcessPayment(decimal amount) =>
        Console.WriteLine($"✅ বিকাশ: {amount} টাকা পাঠানো হলো (+88017XXXXXXXX)");

    public void Refund(decimal amount) =>
        Console.WriteLine($"↩️ বিকাশ: {amount} টাকা ফেরত দেওয়া হলো");
}

// (Concrete Product-এর অংশ) Rocket যোগ করতে শুধু নতুন ক্লাস — আর কিছু ছুঁতে হবে না!
public class RocketPayment : IPayment
{
    public void ProcessPayment(decimal amount) =>
        Console.WriteLine($"✅ রকেট: {amount} টাকা পাঠানো হলো (Dutch-Bangla)");

    public void Refund(decimal amount) =>
        Console.WriteLine($"↩️ রকেট: {amount} টাকা ফেরত দেওয়া হলো");
}

// ৩. Creator: এটাই Factory Method Pattern-এর হৃদয় (Abstract Creator class containing Factory Method)
public abstract class PaymentProcessor
{
    // এটাই Factory Method — subclass ঠিক করবে কোনটা বানাবে
    protected abstract IPayment CreatePayment();

    // এই নিয়মটা সবার জন্য একই — পরিবর্তন হয় না
    public void ExecutePayment(decimal amount)
    {
        Console.WriteLine("--- পেমেন্ট শুরু হচ্ছে ---");
        IPayment payment = CreatePayment(); // Factory Method call
        payment.ProcessPayment(amount);
        Console.WriteLine("--- পেমেন্ট সম্পন্ন ---\n");
    }
}

// ৪. Concrete Creators: নির্দিষ্ট প্রোডাক্ট তৈরি করার আসল ফ্যাক্টরি ক্লাসগুলো
public class NagadProcessor : PaymentProcessor
{
    protected override IPayment CreatePayment() => new NagadPayment();
}

public class bKashProcessor : PaymentProcessor
{
    protected override IPayment CreatePayment() => new bKashPayment();
}

public class RocketProcessor : PaymentProcessor
{
    protected override IPayment CreatePayment() => new RocketPayment();
}

// 💻 Application Client: মেইন মেথড (পুরো অ্যাপ্লিকেশনের আল্টিমেট ক্লায়েন্ট কোড বা Driver Code)
class Program
{
    static void Main()
    {
        PaymentProcessor processor;

        processor = new NagadProcessor();
        processor.ExecutePayment(500);

        processor = new bKashProcessor();
        processor.ExecutePayment(1200);

        processor = new RocketProcessor(); // Rocket যোগ? শুধু এটুকু!
        processor.ExecutePayment(300);
    }
}