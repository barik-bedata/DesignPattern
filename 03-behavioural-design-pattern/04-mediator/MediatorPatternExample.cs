using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Mediator
{
    // ==========================================
    // 1. Mediator Interface (টাওয়ারের নিয়মকানুন)
    // ==========================================
    // এটি কনক্রিট কলিগদের সাথে কমিউনিকেশনের কন্ট্রাক্ট তৈরি করে।
    public interface IAirTrafficControl
    {
        void RegisterAircraft(IAircraft aircraft);
        void SendMessage(string message, IAircraft sender);
    }

    // ==========================================
    // 2. Colleague Interface (প্লেনগুলোর কমন চেহারা)
    // ==========================================
    // প্রতিটি কলিগ শুধু এই ইন্টারফেসের নিয়ম মানবে।
    public interface IAircraft
    {
        string Callsign { get; }
        void Send(string message);
        void Receive(string message);
    }

    // DRY (Don't Repeat Yourself) এর জন্য একটি অ্যাবস্ট্রাক্ট বেস ক্লাস।
    // এটি IColleague (IAircraft) ইন্টারফেস ফলো করে।
    public abstract class AircraftBase : IAircraft
    {
        protected readonly IAirTrafficControl _atc; // ডিপেন্ড করছে ইন্টারফেসের ওপর (DIP)
        public string Callsign { get; }

        protected AircraftBase(IAirTrafficControl atc, string callsign)
        {
            _atc = atc;
            Callsign = callsign;
        }

        public void Send(string message)
        {
            Console.WriteLine($"{Callsign} is sending message to ATC: '{message}'");
            _atc.SendMessage(message, this); // সরাসরি অন্য প্লেনকে নয়, ATC কে মেসেজ দিচ্ছে!
        }

        public abstract void Receive(string message);
    }

    // ==========================================
    // 4. Concrete Colleague (আসল প্লেনগুলো)
    // ==========================================
    public class Boeing777 : AircraftBase
    {
        public Boeing777(IAirTrafficControl atc, string callsign) : base(atc, callsign) { }

        public override void Receive(string message)
        {
            Console.WriteLine($"[Boeing 777 - {Callsign}] receives ATC Broadcast: '{message}'");
        }
    }

    public class AirbusA380 : AircraftBase
    {
        public AirbusA380(IAirTrafficControl atc, string callsign) : base(atc, callsign) { }

        public override void Receive(string message)
        {
            Console.WriteLine($"[Airbus A380 - {Callsign}] receives ATC Broadcast: '{message}'");
        }
    }

    public class Helicopter : AircraftBase
    {
        public Helicopter(IAirTrafficControl atc, string callsign) : base(atc, callsign) { }

        public override void Receive(string message)
        {
            Console.WriteLine($"[Helicopter - {Callsign}] receives ATC Broadcast: '{message}'");
        }
    }

    // ==========================================
    // 3. Concrete Mediator (আসল ATC টাওয়ার)
    // ==========================================
    // এটি সব কলিগের (প্লেনের) ডেটা ম্যানেজ করে এবং কমিউনিকেশন কন্ট্রোল করে।
    public class AtcTower : IAirTrafficControl
    {
        // টাওয়ার শুধু IAircraft ইন্টারফেস চেনে, কোনো কনক্রিট প্লেনকে চেনে না (DIP)
        private readonly List<IAircraft> _aircrafts = new List<IAircraft>();

        public void RegisterAircraft(IAircraft aircraft)
        {
            if (!_aircrafts.Contains(aircraft))
            {
                _aircrafts.Add(aircraft);
                Console.WriteLine($"[ATC TOWER] 🛬 Aircraft '{aircraft.Callsign}' registered to the radar.");
            }
        }

        public void SendMessage(string message, IAircraft sender)
        {
            Console.WriteLine($"\n[ATC TOWER] Routing message from {sender.Callsign} to other aircrafts...");
            
            // সেন্ডার বাদে বাকি সব প্লেনকে মেসেজ ব্রডকাস্ট করে দেওয়া হচ্ছে
            foreach (var aircraft in _aircrafts)
            {
                if (aircraft != sender)
                {
                    aircraft.Receive(message);
                }
            }
            Console.WriteLine(); // Just for console formatting
        }
    }

    // ==========================================
    // Client Code (সিমুলেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (Air Traffic Control) ===\n");

            // ১. টাওয়ার তৈরি (Mediator)
            IAirTrafficControl dhakaAtc = new AtcTower();

            // ২. প্লেন তৈরি (তাদের ভেতরে টাওয়ারের রেফারেন্স দিয়ে দেওয়া হলো)
            IAircraft flightBiman = new Boeing777(dhakaAtc, "BG-001");
            IAircraft flightEmirates = new AirbusA380(dhakaAtc, "EK-582");
            IAircraft rescueCopter = new Helicopter(dhakaAtc, "Rescue-Alpha");

            // ৩. প্লেনগুলোকে টাওয়ারের রাডারে রেজিস্টার করা হলো
            dhakaAtc.RegisterAircraft(flightBiman);
            dhakaAtc.RegisterAircraft(flightEmirates);
            dhakaAtc.RegisterAircraft(rescueCopter);
            Console.WriteLine("--------------------------------------------------");

            // ৪. কমিউনিকেশন শুরু! (প্লেনগুলো নিজেদের মধ্যে কথা বলবে না, টাওয়ারের মাধ্যমে বলবে)
            
            // Biman বলছে সে ল্যান্ড করবে
            flightBiman.Send("Requesting emergency landing on Runway 1!");

            // Emirates তার রেসপন্স দিচ্ছে
            flightEmirates.Send("Understood, we are holding our position in the sky.");
        }
    }
}
