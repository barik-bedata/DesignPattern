using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.Ecommerce
{
    // ==========================================
    // 1. Observer Interface (মডার্ন নেমিং)
    // ==========================================
    public interface INotificationService
    {
        void SendNotification(string productName);
    }

    // ==========================================
    // 2. Subject Interface (For Observers)
    // ==========================================
    public interface IStockSubject
    {
        void Subscribe(INotificationService service);
        void Unsubscribe(INotificationService service);
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
        private readonly List<INotificationService> _notificationServices = new List<INotificationService>();
        
        public string ProductName { get; }
        public bool IsInStock { get; private set; }

        public Product(string productName)
        {
            ProductName = productName;
            IsInStock = false;
        }

        public void Subscribe(INotificationService service) => _notificationServices.Add(service);
        public void Unsubscribe(INotificationService service) => _notificationServices.Remove(service);

        private void NotifySubscribers()
        {
            Console.WriteLine($"\n[System] Triggering {_notificationServices.Count} notification services for '{ProductName}'...");
            foreach (var service in _notificationServices)
            {
                service.SendNotification(ProductName);
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
    
    public class EmailNotificationService : INotificationService
    {
        private readonly string _customerEmail;
        
        public EmailNotificationService(string customerEmail)
        {
            _customerEmail = customerEmail;
        }

        public void SendNotification(string productName)
        {
            Console.WriteLine($"[Email Alert to {_customerEmail}] Good news! {productName} is back in stock.");
        }
    }

    public class SmsNotificationService : INotificationService
    {
        private readonly string _customerPhoneNumber;

        public SmsNotificationService(string customerPhoneNumber)
        {
            _customerPhoneNumber = customerPhoneNumber;
        }

        public void SendNotification(string productName)
        {
            Console.WriteLine($"[SMS Alert to {_customerPhoneNumber}] {productName} is now available to order.");
        }
    }

    public class PushNotificationService : INotificationService
    {
        private readonly string _deviceId;

        public PushNotificationService(string deviceId)
        {
            _deviceId = deviceId;
        }

        public void SendNotification(string productName)
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

            INotificationService emailService = new EmailNotificationService("bedata@example.com");
            INotificationService smsService = new SmsNotificationService("+8801700000000");
            INotificationService pushService = new PushNotificationService("Device_X193_Android");

            iphone.Subscribe(emailService);
            iphone.Subscribe(smsService);
            iphone.Subscribe(pushService);

            iphone.UpdateStockStatus(true);
        }
    }
}
