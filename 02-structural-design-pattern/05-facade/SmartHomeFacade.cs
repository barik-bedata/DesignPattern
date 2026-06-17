using System;

namespace FacadePatternExample
{
    // ==========================================
    // 1. Interfaces for Subsystems (DIP মানার জন্য!)
    // ==========================================

    public interface ITV
    {
        void TurnOn();
        void TurnOff();
    }

    public interface ISoundSystem
    {
        void TurnOn();
        void SetVolume(int level);
        void TurnOff();
    }

    public interface IMediaPlayer
    {
        void TurnOn();
        void PlayMedia(string mediaName);
        void TurnOff();
    }

    public interface ILights
    {
        void Dim();
        void TurnOn();
    }

    // ==========================================
    // 2. Concrete Subsystems (যে ক্লাসগুলোর অনেক লজিক থাকে)
    // ==========================================

    public class SmartTV : ITV
    {
        public void TurnOn() => Console.WriteLine("Smart TV is turned ON.");
        public void TurnOff() => Console.WriteLine("Smart TV is turned OFF.");
    }

    public class SurroundSoundSystem : ISoundSystem
    {
        public void TurnOn() => Console.WriteLine("Surround Sound System is turned ON.");
        public void SetVolume(int level) => Console.WriteLine($"Sound volume set to {level}.");
        public void TurnOff() => Console.WriteLine("Surround Sound System is turned OFF.");
    }

    // আগে DVDPlayer ছিল, এখন আমরা Netflix ব্যবহার করছি। ইন্টারফেস থাকায় Facade এ কোনো চেঞ্জ লাগবে না!
    public class NetflixStreamingService : IMediaPlayer
    {
        public void TurnOn() => Console.WriteLine("Netflix App is launched.");
        public void PlayMedia(string mediaName) => Console.WriteLine($"Streaming movie: '{mediaName}'...");
        public void TurnOff() => Console.WriteLine("Netflix App is closed.");
    }

    public class SmartRoomLights : ILights
    {
        public void Dim() => Console.WriteLine("Room lights are dimmed for the movie.");
        public void TurnOn() => Console.WriteLine("Room lights are back to normal.");
    }


    // ==========================================
    // 3. The Facade (DIP Followed - ১০০% Loosely Coupled)
    // ==========================================

    public class SmartHomeFacade
    {
        // কংক্রিট ক্লাসের ওপর নির্ভর না করে ইন্টারফেসের (Abstraction) ওপর নির্ভর করা হচ্ছে! (DIP)
        private readonly ITV _tv;
        private readonly ISoundSystem _soundSystem;
        private readonly IMediaPlayer _mediaPlayer;
        private readonly ILights _lights;

        public SmartHomeFacade(ITV tv, ISoundSystem soundSystem, IMediaPlayer mediaPlayer, ILights lights)
        {
            _tv = tv;
            _soundSystem = soundSystem;
            _mediaPlayer = mediaPlayer;
            _lights = lights;
        }

        // ক্লায়েন্টের জন্য একদম সিম্পল একটি মেথড!
        public void WatchMovie(string movieName)
        {
            Console.WriteLine("\n[Facade] Get ready to watch a movie! Initializing system...");
            _lights.Dim();
            _tv.TurnOn();
            _soundSystem.TurnOn();
            _soundSystem.SetVolume(50);
            _mediaPlayer.TurnOn();
            _mediaPlayer.PlayMedia(movieName);
            Console.WriteLine("[Facade] Enjoy the movie!\n");
        }

        public void EndMovie()
        {
            Console.WriteLine("\n[Facade] Shutting down the home theater system...");
            _mediaPlayer.TurnOff();
            _soundSystem.TurnOff();
            _tv.TurnOff();
            _lights.TurnOn();
            Console.WriteLine("[Facade] System is OFF.\n");
        }
    }


    // ==========================================
    // 4. Client Code
    // ==========================================
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Facade Design Pattern (With Dependency Inversion Principle) ===\n");

            // সাব-সিস্টেমগুলো তৈরি করা হচ্ছে
            ITV myTv = new SmartTV();
            ISoundSystem mySound = new SurroundSoundSystem();
            IMediaPlayer myPlayer = new NetflixStreamingService(); // আমরা চাইলে কালকে DVDPlayer ও দিতে পারি!
            ILights myLights = new SmartRoomLights();

            // Facade তৈরি করে ইন্টারফেসগুলো ইনজেক্ট করছি (Dependency Injection)
            SmartHomeFacade smartRemote = new SmartHomeFacade(myTv, mySound, myPlayer, myLights);

            // Client এর জীবন কত সহজ দেখুন!
            smartRemote.WatchMovie("Inception");

            // মুভি দেখা শেষ!
            smartRemote.EndMovie();
        }
    }
}
