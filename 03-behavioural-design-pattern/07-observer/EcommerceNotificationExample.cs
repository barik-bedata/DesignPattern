using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.Ecommerce
{
    // ==========================================
    // 1. Observer Interface
    // ==========================================
    public interface IObserver
    {
        void Update(string productName);
    }

    // ==========================================
    // 2. Subject Interface (For Observers)
    // ==========================================
    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
    }

    // ==========================================
    // 2.1 Manager Interface (For Admin/Client)
    // ==========================================
    public interface IProductManager : ISubject
    {
        void Restock();
    }

    // ==========================================
    // 3. Concrete Subject
    // ==========================================
    public class Product : IProductManager
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        
        public string ProductName { get; }

        public Product(string productName)
        {
            ProductName = productName;
        }

        public void AddObserver(IObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);

        private void NotifyObservers()
        {
            Console.WriteLine($"\n[System] Triggering {_observers.Count} observers for '{ProductName}'...");
            foreach (var observer in _observers)
            {
                observer.Update(ProductName);
            }
        }

        public void Restock()
        {
            // প্রোডাক্ট স্টকে এড করার অন্যান্য বিজনেস লজিক এখানে থাকতে পারে...
            NotifyObservers();
        }
    }

    // ==========================================
    // 4. Concrete Observers (Notification Services)
    // ==========================================
    
    public class EmailNotificationService : IObserver
    {
        private readonly string _customerEmail;
        
        public EmailNotificationService(string customerEmail)
        {
            _customerEmail = customerEmail;
        }

        public void Update(string productName)
        {
            Console.WriteLine($"[Email Alert to {_customerEmail}] Good news! {productName} is back in stock.");
        }
    }

    public class SmsNotificationService : IObserver
    {
        private readonly string _customerPhoneNumber;

        public SmsNotificationService(string customerPhoneNumber)
        {
            _customerPhoneNumber = customerPhoneNumber;
        }

        public void Update(string productName)
        {
            Console.WriteLine($"[SMS Alert to {_customerPhoneNumber}] {productName} is now available to order.");
        }
    }

    public class PushNotificationService : IObserver
    {
        private readonly string _deviceId;

        public PushNotificationService(string deviceId)
        {
            _deviceId = deviceId;
        }

        public void Update(string productName)
        {
            Console.WriteLine($"[App Push to Device {_deviceId}] Hurry! {productName} is restocked.");
        }
    }

    // ==========================================
    // 5. Client
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Observer Pattern (E-Commerce Notifications) ===\n");

            // DIP মানার জন্য কনক্রিট ক্লাসের বদলে IProductManager ইন্টারফেস ব্যবহার করা হলো
            IProductManager iphone = new Product("iPhone 16 Pro Max");

            IObserver emailService = new EmailNotificationService("bedata@example.com");
            IObserver smsService = new SmsNotificationService("+8801700000000");
            IObserver pushService = new PushNotificationService("Device_X193_Android");

            iphone.AddObserver(emailService);
            iphone.AddObserver(smsService);
            iphone.AddObserver(pushService);

            // প্রোডাক্ট রিস্টক করা হলো (অটোমেটিক্যালি সবাইকে জানিয়ে দেবে)
            iphone.Restock();
        }
    }
}
