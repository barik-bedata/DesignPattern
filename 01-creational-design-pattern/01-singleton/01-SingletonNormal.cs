using System;
using System.Collections.Generic;

namespace SingletonPattern
{
    // ===== Example 1: Logger =====
    public class Logger
    {
        private static Logger _instance;
        private List<string> _logs;

        // Private constructor so that it cannot be instantiated from outside
        private Logger()
        {
            _logs = new List<string>();
            Console.WriteLine("Logger তৈরি হলো — শুধু একবারই!");
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }
            return _instance;
        }

        public void Log(string module, string message)
        {
            string entry = $"[{module}] {message}";
            _logs.Add(entry);
            Console.WriteLine(entry);
        }

        public List<string> GetLogs()
        {
            return _logs;
        }
    }

    // ===== Example 2: Database Connection =====
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        public string Connection { get; private set; }

        private DatabaseConnection()
        {
            // connection তৈরি করা expensive — শুধু একবার হোক
            Connection = "Connected!";
            Console.WriteLine("DB connection তৈরি হলো");
        }

        public static DatabaseConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection();
            }
            return _instance;
        }

        public string Query(string sql)
        {
            return $"Result of: {sql}";
        }
    }

    // ===== Example 3: App Config =====
    public class AppConfig
    {
        private static AppConfig _instance;

        public string Theme { get; private set; }
        public string Language { get; private set; }
        public string ApiKey { get; private set; }

        private AppConfig()
        {
            // disk থেকে পড়া slow — একবারই করো
            Theme = "dark";
            Language = "bn";
            ApiKey = "abc123";
            Console.WriteLine("Config disk থেকে পড়া হলো (30ms লাগলো)");
        }

        public static AppConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
            }
            return _instance; // পরেরবার instant — 0ms!
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Example 1: Logger =====");
            // PaymentModule আর CartModule একই Logger share করছে
            var log1 = Logger.GetInstance();
            log1.Log("PaymentModule", "Payment successful");

            var log2 = Logger.GetInstance();
            log2.Log("CartModule", "Item added");

            Console.WriteLine($"Is log1 and log2 same object? {ReferenceEquals(log1, log2)}"); // true — একই object!
            
            Console.WriteLine("All Logs:");
            foreach(var log in log1.GetLogs()) 
            {
                Console.WriteLine($" - {log}");
            }


            Console.WriteLine("\n===== Example 2: Database Connection =====");
            // 100টা API call হলেও DB connection একটাই থাকবে
            var db1 = DatabaseConnection.GetInstance(); // connection তৈরি হলো
            var db2 = DatabaseConnection.GetInstance(); // কিছুই হলো না, পুরোনোটা পেলো
            var db3 = DatabaseConnection.GetInstance(); // একই কথা

            Console.WriteLine(db1.Query("SELECT * FROM products"));
            Console.WriteLine(db2.Query("SELECT * FROM users")); // db1 আর db2 একই object!
            Console.WriteLine($"Is db1 and db2 same object? {ReferenceEquals(db1, db2)}");


            Console.WriteLine("\n===== Example 3: App Config =====");
            var config1 = AppConfig.GetInstance(); // 30ms লাগলো
            var config2 = AppConfig.GetInstance(); // 0ms — cached!
            var config3 = AppConfig.GetInstance(); // 0ms — cached!
            Console.WriteLine($"Theme: {config1.Theme}, Language: {config1.Language}, API Key: {config1.ApiKey}");
        }
    }
}
