using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.WeatherStation
{
    // ==========================================
    // 1. Observer Interface
    // ==========================================
    public interface IObserver
    {
        void Update(string weather);
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
    // 2.1. Manager Interface (SOLID Fix for GfG Code)
    // ==========================================
    // GfG এর কোডে setWeather() কল করার জন্য সরাসরি কনক্রিট ক্লাস ব্যবহার করা হয়েছিল।
    // আমরা DIP মানার জন্য এই নতুন ইন্টারফেসটি যুক্ত করেছি।
    public interface IWeatherStationManager : ISubject
    {
        void SetWeather(string newWeather);
    }

    // ==========================================
    // 3. Concrete Subject
    // ==========================================
    public class WeatherStation : IWeatherStationManager
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private string _weather;

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_weather);
            }
        }

        public void SetWeather(string newWeather)
        {
            _weather = newWeather;
            NotifyObservers(); // আবহাওয়া বদলালেই সবাইকে নোটিফাই করবে
        }
    }

    // ==========================================
    // 4. Concrete Observers (Phone, TV)
    // ==========================================
    public class PhoneDisplay : IObserver
    {
        private string _weather;

        public void Update(string weather)
        {
            _weather = weather;
            Display();
        }

        private void Display()
        {
            Console.WriteLine($"Phone Display: Weather updated - {_weather}");
        }
    }

    public class TvDisplay : IObserver
    {
        private string _weather;

        public void Update(string weather)
        {
            _weather = weather;
            Display();
        }

        private void Display()
        {
            Console.WriteLine($"TV Display: Weather updated - {_weather}");
        }
    }

    // ==========================================
    // 5. Usage (Client)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Observer Pattern (GeeksforGeeks Weather Station) ===\n");

            // আমরা GfG এর মতো কনক্রিট ক্লাস না ব্যবহার করে ইন্টারফেস (DIP) ব্যবহার করছি
            IWeatherStationManager weatherStation = new WeatherStation();

            IObserver phoneDisplay = new PhoneDisplay();
            IObserver tvDisplay = new TvDisplay();

            // Register observers
            weatherStation.AddObserver(phoneDisplay);
            weatherStation.AddObserver(tvDisplay);

            // Simulating weather changes
            Console.WriteLine("--- Day 1 ---");
            weatherStation.SetWeather("Sunny");
            
            Console.WriteLine("\n--- Day 2 ---");
            weatherStation.SetWeather("Rainy");
            
            Console.WriteLine("\n--- Day 3 ---");
            weatherStation.SetWeather("Cloudy");

            // Remove one observer
            Console.WriteLine("\n[System] TV Display turned off...");
            weatherStation.RemoveObserver(tvDisplay);

            // Notify remaining observer
            Console.WriteLine("\n--- Day 4 ---");
            weatherStation.SetWeather("Windy");
        }
    }
}
