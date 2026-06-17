using System;

namespace FacadePatternExample
{
    // ==========================================
    // 1. Complex Subsystems (যে ক্লাসগুলোর অনেক লজিক থাকে)
    // ==========================================

    public class TV
    {
        public void TurnOn() => Console.WriteLine("TV is turned ON.");
        public void TurnOff() => Console.WriteLine("TV is turned OFF.");
    }

    public class SoundSystem
    {
        public void TurnOn() => Console.WriteLine("Sound System is turned ON.");
        public void SetVolume(int level) => Console.WriteLine($"Sound volume set to {level}.");
        public void TurnOff() => Console.WriteLine("Sound System is turned OFF.");
    }

    public class DVDPlayer
    {
        public void TurnOn() => Console.WriteLine("DVD Player is turned ON.");
        public void PlayMovie(string movie) => Console.WriteLine($"Playing movie: '{movie}'...");
        public void TurnOff() => Console.WriteLine("DVD Player is turned OFF.");
    }

    public class RoomLights
    {
        public void Dim() => Console.WriteLine("Room lights are dimmed for the movie.");
        public void TurnOn() => Console.WriteLine("Room lights are back to normal.");
    }


    // ==========================================
    // 2. The Facade (ক্লায়েন্টের জন্য সহজ একটি ইন্টারফেস বা কন্ট্রোলার)
    // ==========================================

    public class SmartHomeFacade
    {
        private TV _tv;
        private SoundSystem _soundSystem;
        private DVDPlayer _dvdPlayer;
        private RoomLights _lights;

        public SmartHomeFacade(TV tv, SoundSystem soundSystem, DVDPlayer dvdPlayer, RoomLights lights)
        {
            _tv = tv;
            _soundSystem = soundSystem;
            _dvdPlayer = dvdPlayer;
            _lights = lights;
        }

        // ক্লায়েন্টের জন্য একদম সিম্পল একটি মেথড! সে ভেতরের জটিলতা কিছুই জানবে না।
        public void WatchMovie(string movieName)
        {
            Console.WriteLine("\n[Facade] Get ready to watch a movie! Initializing system...");
            _lights.Dim();
            _tv.TurnOn();
            _soundSystem.TurnOn();
            _soundSystem.SetVolume(50);
            _dvdPlayer.TurnOn();
            _dvdPlayer.PlayMovie(movieName);
            Console.WriteLine("[Facade] Enjoy the movie!\n");
        }

        // মুভি দেখা শেষ হলে সিস্টেম বন্ধ করার জন্য আরেকটি সিম্পল মেথড
        public void EndMovie()
        {
            Console.WriteLine("\n[Facade] Shutting down the home theater system...");
            _dvdPlayer.TurnOff();
            _soundSystem.TurnOff();
            _tv.TurnOff();
            _lights.TurnOn();
            Console.WriteLine("[Facade] System is OFF.\n");
        }
    }


    // ==========================================
    // 3. Client Code
    // ==========================================
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Facade Design Pattern ===\n");

            // সাব-সিস্টেমগুলো তৈরি করা হচ্ছে
            TV myTv = new TV();
            SoundSystem mySound = new SoundSystem();
            DVDPlayer myDvd = new DVDPlayer();
            RoomLights myLights = new RoomLights();

            // Facade তৈরি করে সাব-সিস্টেমগুলো ভেতরে দিয়ে দিচ্ছি
            SmartHomeFacade smartRemote = new SmartHomeFacade(myTv, mySound, myDvd, myLights);

            // Client এর জীবন কত সহজ দেখুন! তাকে আলাদা করে ৪টা গ্যাজেট অন করতে হচ্ছে না। 
            // সে জাস্ট Facade কে একটা ইনস্ট্রাকশন দিচ্ছে।
            smartRemote.WatchMovie("Inception");

            // মুভি দেখা শেষ!
            smartRemote.EndMovie();
        }
    }
}
