using System;

namespace BehavioralDesignPattern.Mediator.SmartHome
{
    // 1. Mediator Interface (স্মার্ট হাবের নিয়ম)
    public interface ISmartHomeHub
    {
        void Notify(IDevice sender, string eventType);
    }

    // 2. Colleague Interface (ডিভাইসগুলোর কমন চেহারা)
    public interface IDevice
    {
        void TriggerEvent(string eventType);
    }

    // 3. Concrete Mediator (আসল স্মার্ট হাব)
    public class HomeHub : ISmartHomeHub
    {
        private IDevice _light;
        private IDevice _ac;

        public void RegisterDevices(IDevice light, IDevice ac)
        {
            _light = light;
            _ac = ac;
        }

        public void Notify(IDevice sender, string eventType)
        {
            // যদি সেন্সর মোশন ডিটেক্ট করে, তবে হাব একা একাই লাইট এবং এসি অন করে দেবে!
            // সেন্সরকে লাইট বা এসি সম্পর্কে জানতে হচ্ছে না।
            if (eventType == "MotionDetected")
            {
                Console.WriteLine("[Hub] Motion detected! Turning on lights and AC...");
                (_light as SmartLight)?.TurnOn();
                (_ac as SmartAC)?.TurnOn();
            }
        }
    }

    // 4. Concrete Colleagues (আসল স্মার্ট ডিভাইসগুলো)
    public class MotionSensor : IDevice
    {
        private readonly ISmartHomeHub _hub;
        public MotionSensor(ISmartHomeHub hub) { _hub = hub; }
        
        public void TriggerEvent(string eventType) => _hub.Notify(this, eventType); // হাবকে জানাচ্ছে
    }

    public class SmartLight : IDevice
    {
        public void TriggerEvent(string eventType) { }
        public void TurnOn() => Console.WriteLine("[Light] Light is ON.");
    }

    public class SmartAC : IDevice
    {
        public void TriggerEvent(string eventType) { }
        public void TurnOn() => Console.WriteLine("[AC] Air Conditioner is ON.");
    }

    // ==========================================
    // 5. Client (মেইন অ্যাপ্লিকেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (Smart Home) ===\n");

            HomeHub hub = new HomeHub();
            
            IDevice sensor = new MotionSensor(hub);
            IDevice light = new SmartLight();
            IDevice ac = new SmartAC();

            hub.RegisterDevices(light, ac);

            // সেন্সর ট্রিগার হলো, বাকি কাজ হাব করবে
            sensor.TriggerEvent("MotionDetected");
        }
    }
}
