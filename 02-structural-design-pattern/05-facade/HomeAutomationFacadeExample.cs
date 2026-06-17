using System;

namespace FacadePattern.HomeAutomation
{
    // ==========================================
    // 1. Interfaces for Subsystems (DIP মানার জন্য!)
    // ==========================================
    public interface ILighting { void TurnOnAll(); void TurnOffAll(); }
    public interface IHeating { void SetTemperature(int temp); void TurnOff(); }
    public interface ISecurity { void ArmSystem(); void DisarmSystem(); }

    // ==========================================
    // 2. Concrete Subsystems (জটিল লজিকের ক্লাসগুলো)
    // ==========================================
    public class SmartLighting : ILighting
    {
        public void TurnOnAll() => Console.WriteLine("[Lighting] All lights turned ON.");
        public void TurnOffAll() => Console.WriteLine("[Lighting] All lights turned OFF.");
    }

    public class CentralHeating : IHeating
    {
        public void SetTemperature(int temp) => Console.WriteLine($"[Heating] Temperature set to {temp}°C.");
        public void TurnOff() => Console.WriteLine("[Heating] Heater turned OFF.");
    }

    public class HomeSecuritySystem : ISecurity
    {
        public void ArmSystem() => Console.WriteLine("[Security] Alarm is ARMED. Motion sensors active.");
        public void DisarmSystem() => Console.WriteLine("[Security] Alarm is DISARMED.");
    }

    // ==========================================
    // 3. The Facade (সিম্পল ইন্টারফেস / কন্ট্রোলার)
    // ==========================================
    public class HomeAutomationFacade
    {
        // Dependency Inversion (DIP) - কংক্রিট ক্লাসের ওপর নির্ভর না করে ইন্টারফেস ব্যবহার করা হচ্ছে
        private readonly ILighting _lighting;
        private readonly IHeating _heating;
        private readonly ISecurity _security;

        public HomeAutomationFacade(ILighting lighting, IHeating heating, ISecurity security)
        {
            _lighting = lighting;
            _heating = heating;
            _security = security;
        }

        // অনেকগুলো কাজকে একটা মেথডে নিয়ে আসা (সবকিছু একসাথে)
        public void LeaveHome()
        {
            Console.WriteLine("\n=== [Facade] Leaving Home Mode Activated ===");
            _lighting.TurnOffAll();
            _heating.TurnOff();
            _security.ArmSystem();
            Console.WriteLine("House is secured. Goodbye!\n");
        }

        public void ArriveHome()
        {
            Console.WriteLine("\n=== [Facade] Arriving Home Mode Activated ===");
            _security.DisarmSystem();
            _lighting.TurnOnAll();
            _heating.SetTemperature(24);
            Console.WriteLine("Welcome back! The house is ready for you.\n");
        }
    }

    // ==========================================
    // 4. Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            // সাব-সিস্টেম তৈরি
            ILighting lighting = new SmartLighting();
            IHeating heating = new CentralHeating();
            ISecurity security = new HomeSecuritySystem();

            // Facade তৈরি করে ইনজেক্ট (Dependency Injection) করা হচ্ছে
            HomeAutomationFacade smartHome = new HomeAutomationFacade(lighting, heating, security);

            // Client এর জীবন সহজ! সে শুধু একটা মেথড কল করছে!
            smartHome.LeaveHome();
            smartHome.ArriveHome();
        }
    }
}
