using System;

namespace FactoryMethodPattern
{
    // ১. Product Interface
    public interface ITransport
    {
        void Deliver();
    }

    // ২. Concrete Products
    public class Truck : ITransport
    {
        public void Deliver()
        {
            Console.WriteLine("Truck: রাস্তা দিয়ে বক্সে করে প্রোডাক্ট ডেলিভারি দেওয়া হচ্ছে...");
        }
    }

    public class Ship : ITransport
    {
        public void Deliver()
        {
            Console.WriteLine("Ship: সমুদ্রপথে কন্টেইনারে করে প্রোডাক্ট ডেলিভারি দেওয়া হচ্ছে...");
        }
    }

    // 👉 নতুন যোগ করা হলো: Bicycle 👈
    public class Bicycle : ITransport
    {
        public void Deliver()
        {
            Console.WriteLine("Bicycle: গলির ভেতরে সাইকেলে করে ফাস্ট ফুড ডেলিভারি দেওয়া হচ্ছে...");
        }
    }

    // ৩. Creator / Factory Class
    public abstract class Logistics
    {
        public abstract ITransport CreateTransport();

        public void PlanDelivery()
        {
            Console.WriteLine("Logistics: ডেলিভারি প্ল্যান করা হলো। ট্রান্সপোর্ট রেডি করা হচ্ছে...");
            ITransport transport = CreateTransport();
            transport.Deliver();
        }
    }

    // ৪. Concrete Creators
    public class RoadLogistics : Logistics
    {
        public override ITransport CreateTransport()
        {
            return new Truck(); 
        }
    }

    public class SeaLogistics : Logistics
    {
        public override ITransport CreateTransport()
        {
            return new Ship(); 
        }
    }

    // 👉 নতুন যোগ করা হলো: BicycleLogistics 👈
    public class BicycleLogistics : Logistics
    {
        public override ITransport CreateTransport()
        {
            return new Bicycle(); // সে শুধু সাইকেল বানাবে
        }
    }

    // ৫. Client Code: মেইন অ্যাপ্লিকেশন
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Road Logistics Example ---");
            Logistics roadApp = new RoadLogistics();
            roadApp.PlanDelivery(); 

            Console.WriteLine("\n--- Sea Logistics Example ---");
            Logistics seaApp = new SeaLogistics();
            seaApp.PlanDelivery();  

            // 👉 নতুন সাইকেলের কোড এখানে কল করা হলো 👈
            Console.WriteLine("\n--- Local Area Logistics Example ---");
            Logistics bikeApp = new BicycleLogistics();
            bikeApp.PlanDelivery(); 
        }
    }
}
