using System;
using System.Collections.Generic;

namespace CompositePatternExample.ECommerce
{
    // ১. Component
    public interface IProduct
    {
        double GetPrice();
    }

    // ২. Composite Interface (শুধু বক্সের জন্য)
    public interface IBox : IProduct
    {
        void AddProduct(IProduct product);
    }

    // ৩. Leaf (সিঙ্গেল প্রোডাক্ট - এর ভেতরে আর কিছু থাকে না)
    public class SingleItem : IProduct
    {
        private string _name;
        private double _price;

        public SingleItem(string name, double price)
        {
            _name = name;
            _price = price;
        }

        public double GetPrice()
        {
            Console.WriteLine($"  Item: {_name} - Price: ${_price}");
            return _price;
        }
    }

    // ৪. Composite (বক্স, যার ভেতরে আইটেম বা আরও ছোট বক্স থাকতে পারে)
    public class DeliveryBox : IBox
    {
        private string _boxName;
        private List<IProduct> _items = new List<IProduct>();

        public DeliveryBox(string boxName)
        {
            _boxName = boxName;
        }

        public void AddProduct(IProduct product)
        {
            _items.Add(product);
        }

        // বক্সের দাম হলো তার ভেতরের সব জিনিসের দামের যোগফল!
        public double GetPrice()
        {
            Console.WriteLine($"\n--- Opening {_boxName} ---");
            double total = 0;
            foreach (var item in _items)
            {
                total += item.GetPrice();
            }
            Console.WriteLine($"=> Total Price of {_boxName}: ${total}");
            return total;
        }
    }

    // ৫. Client
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== E-Commerce Delivery Box Example (Composite Pattern) ===");

            // Leaf আইটেম
            IProduct phone = new SingleItem("iPhone 15", 999.99);
            IProduct charger = new SingleItem("Apple Charger", 29.99);
            IProduct earphone = new SingleItem("AirPods", 199.99);

            // ছোট বক্স (শুধু এক্সেসরিজ রাখার জন্য)
            IBox accessoriesBox = new DeliveryBox("Accessories Box");
            accessoriesBox.AddProduct(charger);
            accessoriesBox.AddProduct(earphone);

            // বড় ডেলিভারি বক্স (যার ভেতরে ফোন এবং ছোট বক্সটি থাকবে)
            IBox mainDeliveryBox = new DeliveryBox("Main Delivery Box");
            mainDeliveryBox.AddProduct(phone);
            mainDeliveryBox.AddProduct(accessoriesBox); // Box এর ভেতরে Box (Composite এর ম্যাজিক)

            // Client শুধু মেইন বক্সের দাম জানতে চায়
            Console.WriteLine("\n[ Calculating Total Order Price ]");
            double finalPrice = mainDeliveryBox.GetPrice();
            
            Console.WriteLine($"\n*** Final Order Value: ${finalPrice} ***");
        }
    }
}
