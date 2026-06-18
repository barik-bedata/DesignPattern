using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Visitor.Supermarket
{
    // ==========================================
    // 1. Visitor Interface
    // ==========================================
    // এটি ডিফাইন করে যে ভিজিটর কোন কোন প্রোডাক্ট ভিজিট করতে পারবে।
    public interface IShoppingVisitor
    {
        void Visit(Laptop laptop);
        void Visit(Fruit fruit);
        void Visit(Book book);
    }

    // ==========================================
    // 3. Element Interface
    // ==========================================
    // প্রোডাক্টগুলোর ইন্টারফেস। শুধু Accept মেথড থাকবে।
    public interface IItem
    {
        void Accept(IShoppingVisitor visitor);
    }

    // ==========================================
    // 4. Concrete Elements
    // ==========================================
    // SOLID: প্রতিটি এলিমেন্ট তার নিজস্ব ডেটা (Price, Weight) নিজেই ধারণ করে। 
    // ট্যাক্স বা ডিসকাউন্ট লজিক এখানে নেই!
    public class Laptop : IItem
    {
        public double Price { get; }

        public Laptop(double price)
        {
            Price = price;
        }

        public void Accept(IShoppingVisitor visitor)
        {
            visitor.Visit(this); // Double Dispatch
        }
    }

    public class Fruit : IItem
    {
        public double PricePerKg { get; }
        public double Weight { get; }

        public Fruit(double pricePerKg, double weight)
        {
            PricePerKg = pricePerKg;
            Weight = weight;
        }

        public void Accept(IShoppingVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Book : IItem
    {
        public double Price { get; }

        public Book(double price)
        {
            Price = price;
        }

        public void Accept(IShoppingVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // ==========================================
    // 2. Concrete Visitors
    // ==========================================
    // ট্যাক্স ক্যালকুলেট করার স্পেসিফিক ভিজিটর
    public class TaxVisitor : IShoppingVisitor
    {
        public double TotalTax { get; private set; } = 0;

        public void Reset()
        {
            TotalTax = 0;
        }

        public void Visit(Laptop laptop)
        {
            // ল্যাপটপে ১৫% ট্যাক্স
            double tax = laptop.Price * 0.15;
            Console.WriteLine($"[Tax] Laptop Tax: ${tax:F2}");
            TotalTax += tax;
        }

        public void Visit(Fruit fruit)
        {
            // ফলে কোনো ট্যাক্স নেই
            Console.WriteLine("[Tax] Fruit Tax: $0.00 (Tax Free)");
        }

        public void Visit(Book book)
        {
            // বইয়ে ৫% ট্যাক্স
            double tax = book.Price * 0.05;
            Console.WriteLine($"[Tax] Book Tax: ${tax:F2}");
            TotalTax += tax;
        }
    }

    // ডিসকাউন্ট ক্যালকুলেট করার স্পেসিফিক ভিজিটর
    public class DiscountVisitor : IShoppingVisitor
    {
        public double TotalDiscount { get; private set; } = 0;

        public void Reset()
        {
            TotalDiscount = 0;
        }

        public void Visit(Laptop laptop)
        {
            // ল্যাপটপে ফ্ল্যাট $50 ডিসকাউন্ট
            Console.WriteLine("[Discount] Laptop Discount: $50.00");
            TotalDiscount += 50;
        }

        public void Visit(Fruit fruit)
        {
            // ফলের মোট দামের ওপর ১০% ডিসকাউন্ট
            double discount = (fruit.PricePerKg * fruit.Weight) * 0.10;
            Console.WriteLine($"[Discount] Fruit Discount: ${discount:F2}");
            TotalDiscount += discount;
        }

        public void Visit(Book book)
        {
            // বইয়ে ২০% ডিসকাউন্ট
            double discount = book.Price * 0.20;
            Console.WriteLine($"[Discount] Book Discount: ${discount:F2}");
            TotalDiscount += discount;
        }
    }

    // ==========================================
    // 5. Object Structure / Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Visitor Pattern (Supermarket Checkout) ===\n");

            // শপিং কার্ট (Object Structure)
            List<IItem> cart = new List<IItem>
            {
                new Laptop(1000),
                new Fruit(10, 2.5),
                new Book(20)
            };

            TaxVisitor taxCalculator = new TaxVisitor();
            DiscountVisitor discountCalculator = new DiscountVisitor();

            Console.WriteLine("--- Calculating Tax ---");
            foreach (var item in cart)
            {
                item.Accept(taxCalculator); // ভিজিটর কার্টে ঢুকে সব আইটেমে ট্যাক্স হিসাব করছে
            }
            Console.WriteLine($"Total Tax: ${taxCalculator.TotalTax:F2}\n");

            Console.WriteLine("--- Calculating Discount ---");
            foreach (var item in cart)
            {
                item.Accept(discountCalculator); // ভিজিটর কার্টে ঢুকে সব আইটেমে ডিসকাউন্ট দিচ্ছে
            }
            Console.WriteLine($"Total Discount: ${discountCalculator.TotalDiscount:F2}");
        }
    }
}
