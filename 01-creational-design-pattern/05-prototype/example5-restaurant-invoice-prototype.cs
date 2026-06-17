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
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.Invoice.Solution
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface IInvoicePrototype
    {
        IInvoicePrototype Clone();
        void SetBillingDetails(string customerName, decimal amount);
        void Print();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class RestaurantInvoice : IInvoicePrototype
    {
        public string RestaurantLogo { get; private set; }
        public string Address { get; private set; }
        public string VatRegNumber { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; }

        public RestaurantInvoice()
        {
            Console.WriteLine("\n>> [Solution] রেস্টুরেন্ট খোলার পর একবারই লোগো ও ডেটা লোড হচ্ছে...");
            Thread.Sleep(500);

            RestaurantLogo = "[কাচ্চি ভাই - Kacchi Bhai Logo]";
            Address = "Mirpur-10, Dhaka";
            VatRegNumber = "VAT-9988776655";
        }

        public void SetBillingDetails(string customerName, decimal amount)
        {
            CustomerName = customerName;
            TotalAmount = amount;
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

    // ==============================================================
    // ৩. Client (BillingSystem)
    // ==============================================================
    public class BillingSystem
    {
        private IInvoicePrototype _masterInvoice;

        public BillingSystem(IInvoicePrototype masterInvoice)
        {
            _masterInvoice = masterInvoice;
        }

        public IInvoicePrototype GenerateBill(string customerName, decimal amount)
        {
            var clone = _masterInvoice.Clone();
            clone.SetBillingDetails(customerName, amount);
            return clone;
        }
    }

    // ==============================================================
    // ৪. Main Class (SolutionRunner)
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Prototype Pattern (Using 4 Components) ===");
            
            // মাস্টার ইনভয়েস লোড করা
            var masterInvoice = new RestaurantInvoice();

            // বিলিং সিস্টেমে (ক্লায়েন্ট) মাস্টার কপি দিয়ে দেওয়া
            var system = new BillingSystem(masterInvoice);

            // শুধু বিলিং ডিটেইলস চেঞ্জ করে ক্লোন করা
            var bill1 = system.GenerateBill("Karim", 1200);
            bill1.Print();

            var bill2 = system.GenerateBill("Rahim", 850);
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
