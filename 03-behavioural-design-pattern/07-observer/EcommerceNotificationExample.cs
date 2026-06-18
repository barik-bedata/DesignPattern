using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.Ecommerce
{
    // ==========================================
    // 1. Observer Interface (যারা নোটিফিকেশন পাবে)
    // ==========================================
    // Subject শুধু এই ইন্টারফেসটিকে চেনে।
    public interface IObserver
    {
        void Update(string message);
    }

    // ==========================================
    // 2. Subject Interface (যে নোটিফিকেশন পাঠাবে)
    // ==========================================
    public interface ISubject
    {
        void Attach(IObserver observer); // সাবস্ক্রাইব করা
        void Detach(IObserver observer); // আনসাবস্ক্রাইব করা
        void NotifyObservers(string message);
    }

    // ==========================================
    // 3. Concrete Subject (প্রোডাক্ট স্টকে আসলে নোটিফাই করবে)
    // ==========================================
    public class Product : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private string _name;
        private bool _inStock;

        public Product(string name)
        {
            _name = name;
            _inStock = false;
        }

        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void NotifyObservers(string message)
        {
            Console.WriteLine($"\n[Product System] Notifying {_observers.Count} observers for '{_name}'...");
            foreach (var observer in _observers)
            {
                observer.Update(message); // DIP এর কারণে লুজ কাপলিং
            }
        }

        public void SetInStock(bool inStock)
        {
            _inStock = inStock;
            if (_inStock)
            {
                NotifyObservers($"Good News! {_name} is now Back in Stock!");
            }
        }
    }

    // ==========================================
    // 4. Concrete Observers (Email, SMS, Mobile)
    // ==========================================
    // নতুন কোনো Notifier (যেমন WhatsApp) অ্যাড করতে হলে শুধু নতুন ক্লাস বানাতে হবে, Product ক্লাসে কোনো হাত দিতে হবে না! (OCP)
    
    public class EmailNotifier : IObserver
    {
        private string _emailAddress;
        public EmailNotifier(string email) => _emailAddress = email;
        public void Update(string message) => Console.WriteLine($"[Email to {_emailAddress}] {message}");
    }

    public class SmsNotifier : IObserver
    {
        private string _phoneNumber;
        public SmsNotifier(string phone) => _phoneNumber = phone;
        public void Update(string message) => Console.WriteLine($"[SMS to {_phoneNumber}] {message}");
    }

    public class MobileAppNotifier : IObserver
    {
        private string _username;
        public MobileAppNotifier(string user) => _username = user;
        public void Update(string message) => Console.WriteLine($"[Push Notification to {_username}'s Phone] {message}");
    }

    // ==========================================
    // 5. Client
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Observer Pattern (E-Commerce Notifications) ===\n");

            // ১. প্রোডাক্ট তৈরি হলো (Subject)
            Product iphone = new Product("iPhone 16 Pro Max");

            // ২. বিভিন্ন ইউজার বিভিন্ন মাধ্যমে সাবস্ক্রাইব করলো (Observers)
            IObserver emailUser = new EmailNotifier("bedata@example.com");
            IObserver smsUser = new SmsNotifier("+8801700000000");
            IObserver appUser = new MobileAppNotifier("bedata_app");

            iphone.Attach(emailUser);
            iphone.Attach(smsUser);
            iphone.Attach(appUser);

            // ৩. প্রোডাক্ট স্টকে আসলো, সাথে সাথে সবাইকে অটোমেটিক নোটিফিকেশন চলে যাবে!
            iphone.SetInStock(true);
        }
    }
}
