using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (প্রতিটি বিলের জন্য লোগো ও হেডার লোড করা)
// ==============================================================

namespace PrototypePattern.Invoice.Violation
{
    public class RestaurantInvoice
    {
        public string RestaurantLogo { get; set; }
        public string Address { get; set; }
        public string VatRegNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        public RestaurantInvoice(string customer, decimal amount)
        {
            Console.WriteLine(">> [Violation] ডেটাবেস থেকে লোগো, ঠিকানা ও VAT নাম্বার লোড হচ্ছে... (API Call)");
            Thread.Sleep(500);

            RestaurantLogo = "[কাচ্চি ভাই - Kacchi Bhai Logo]";
            Address = "Mirpur-10, Dhaka";
            VatRegNumber = "VAT-9988776655";

            CustomerName = customer;
            TotalAmount = amount;
        }

        public void Print()
        {
            Console.WriteLine($"\n{RestaurantLogo}");
            Console.WriteLine($"{Address} | {VatRegNumber}");
            Console.WriteLine($"Bill To: {CustomerName} | Amount: {TotalAmount} TK");
            Console.WriteLine("------------------------------------------");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: প্রতিটি ইনভয়েসে স্ট্যাটিক ডেটা রিলোড করা ===");
            var bill1 = new RestaurantInvoice("Karim", 1200);
            bill1.Print();

            var bill2 = new RestaurantInvoice("Rahim", 850);
            bill2.Print();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (মাস্টার ইনভয়েস ক্লোন করা)
// ==============================================================

namespace PrototypePattern.Invoice.Solution
{
    public interface IInvoicePrototype
    {
        IInvoicePrototype Clone();
    }

    public class RestaurantInvoice : IInvoicePrototype
    {
        public string RestaurantLogo { get; set; }
        public string Address { get; set; }
        public string VatRegNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        public RestaurantInvoice()
        {
            Console.WriteLine("\n>> [Solution] রেস্টুরেন্ট খোলার পর একবারই লোগো ও ডেটা লোড হচ্ছে...");
            Thread.Sleep(500);

            RestaurantLogo = "[কাচ্চি ভাই - Kacchi Bhai Logo]";
            Address = "Mirpur-10, Dhaka";
            VatRegNumber = "VAT-9988776655";
        }

        public IInvoicePrototype Clone()
        {
            return (IInvoicePrototype)this.MemberwiseClone();
        }

        public void Print()
        {
            Console.WriteLine($"\n{RestaurantLogo}");
            Console.WriteLine($"{Address} | {VatRegNumber}");
            Console.WriteLine($"Bill To: {CustomerName} | Amount: {TotalAmount} TK");
            Console.WriteLine("------------------------------------------");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: মাস্টার ইনভয়েস ক্লোন করে শুধু কাস্টমার ডেটা বসানো ===");
            
            var masterInvoice = new RestaurantInvoice();

            var bill1 = (RestaurantInvoice)masterInvoice.Clone();
            bill1.CustomerName = "Karim";
            bill1.TotalAmount = 1200;
            bill1.Print();

            var bill2 = (RestaurantInvoice)masterInvoice.Clone();
            bill2.CustomerName = "Rahim";
            bill2.TotalAmount = 850;
            bill2.Print();
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.Invoice.Violation.ViolationRunner.Run();
        PrototypePattern.Invoice.Solution.SolutionRunner.Run();
    }
}
