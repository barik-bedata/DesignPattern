using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.Ecommerce
{
    // ==========================================
    // 1. Observer Interface (মডার্ন নেমিং)
    // ==========================================
    public interface IStockObserver
    {
        void OnStockAvailable(string productName);
    }

    // ==========================================
    // 2. Subject Interface (For Observers)
    // ==========================================
    public interface IStockSubject
    {
        void Subscribe(IStockObserver observer);
        void Unsubscribe(IStockObserver observer);
    }

    // ==========================================
    // 2.1 Manager Interface (For Admin/Client)
    // ==========================================
    public interface IProductManager : IStockSubject
    {
        void UpdateStockStatus(bool inStock);
    }

    // ==========================================
    // 3. Concrete Subject
    // ==========================================
    public class Product : IProductManager
    {
        private readonly List<IStockObserver> _subscribers = new List<IStockObserver>();
        
        public string ProductName { get; }
        public bool IsInStock { get; private set; }

        public Product(string productName)
        {
            ProductName = productName;
            IsInStock = false;
        }

        public void Subscribe(IStockObserver observer) => _subscribers.Add(observer);
        public void Unsubscribe(IStockObserver observer) => _subscribers.Remove(observer);

        private void NotifySubscribers()
        {
            Console.WriteLine($"\n[System] Notifying {_subscribers.Count} subscribers that '{ProductName}' is in stock...");
            foreach (var subscriber in _subscribers)
            {
                subscriber.OnStockAvailable(ProductName);
            }
        }

        public void UpdateStockStatus(bool inStock)
        {
            IsInStock = inStock;
            if (IsInStock)
            {
                NotifySubscribers();
            }
        }
    }

    // ==========================================
    // 4. Concrete Observers (Notification Services)
    // ==========================================
    
    public class EmailNotificationService : IStockObserver
    {
        private readonly string _customerEmail;
        
        public EmailNotificationService(string customerEmail)
        {
            _customerEmail = customerEmail;
        }

        public void OnStockAvailable(string productName)
        {
            Console.WriteLine($"[Email Alert to {_customerEmail}] Good news! {productName} is back in stock.");
        }
    }

    public class SmsNotificationService : IStockObserver
    {
        private readonly string _customerPhoneNumber;

        public SmsNotificationService(string customerPhoneNumber)
        {
            _customerPhoneNumber = customerPhoneNumber;
        }

        public void OnStockAvailable(string productName)
        {
            Console.WriteLine($"[SMS Alert to {_customerPhoneNumber}] {productName} is now available to order.");
        }
    }

    public class PushNotificationService : IStockObserver
    {
        private readonly string _deviceId;

        public PushNotificationService(string deviceId)
        {
            _deviceId = deviceId;
        }

        public void OnStockAvailable(string productName)
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

            IStockObserver emailSubscriber = new EmailNotificationService("bedata@example.com");
            IStockObserver smsSubscriber = new SmsNotificationService("+8801700000000");
            IStockObserver pushSubscriber = new PushNotificationService("Device_X193_Android");

            iphone.Subscribe(emailSubscriber);
            iphone.Subscribe(smsSubscriber);
            iphone.Subscribe(pushSubscriber);

            iphone.UpdateStockStatus(true);
        }
    }
}
