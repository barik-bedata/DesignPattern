using System;
using System.Collections.Generic;
using System.Threading;

// ==============================================================
// 🟢 SINGLETON PATTERN: NORMAL (Single-Threaded) EXAMPLES
// ==============================================================
namespace SingletonPattern.Normal
{
    // ==============================================================
    // Example 1: Logger (লগার)
    // ==============================================================
    namespace LoggerExample
    {
        // 🔹 ইন্টারফেস তৈরি করা হলো
        public interface ILogger
        {
            void Log(string message);
            void ShowAllLogs();
        }

        // 🔹 সিঙ্গেলটন লগার ক্লাস যা ILogger ইমপ্লিমেন্ট করে
        public class Logger : ILogger
        {
            private static Logger _instance;
            private List<string> _logs;

            // ১. Private Constructor
            private Logger()
            {
                _logs = new List<string>();
                Console.WriteLine(">> [Logger] সিস্টেমের প্রধান লগার তৈরি হলো! (একবারই হবে)");
            }

            // ২. Public Static Method
            public static Logger GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }

            public void Log(string message)
            {
                _logs.Add(message);
                Console.WriteLine($"[LOG]: {message}");
            }

            public void ShowAllLogs()
            {
                Console.WriteLine("\n--- Stored Logs ---");
                foreach (var log in _logs)
                {
                    Console.WriteLine(log);
                }
            }
        }
    }

    // ==============================================================
    // Example 2: Database Connection Pool (ডেটাবেস পুল)
    // ==============================================================
    namespace DatabaseExample
    {
        // 🔹 ইন্টারফেস তৈরি করা হলো
        public interface IDatabaseConnectionPool
        {
            void ExecuteQuery(string query);
        }

        // 🔹 সিঙ্গেলটন ডেটাবেস পুল ক্লাস যা IDatabaseConnectionPool ইমপ্লিমেন্ট করে
        public class DatabaseConnectionPool : IDatabaseConnectionPool
        {
            private static DatabaseConnectionPool _instance;

            public string ConnectionString { get; private set; }
            public bool IsConnected { get; private set; }
            private int _queryCount = 0;

            // ১. Private Constructor
            private DatabaseConnectionPool()
            {
                Console.WriteLine(">> [Database] ডেটাবেসের সাথে কানেকশন তৈরি হচ্ছে... (Taking Time)");
                Thread.Sleep(500);

                ConnectionString = "Server=myServer;Database=myDB;User=admin;";
                IsConnected = true;
                Console.WriteLine(">> [Database] কানেকশন সাকসেসফুল!");
            }

            // ২. Public Static Method
            public static DatabaseConnectionPool GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnectionPool();
                }
                return _instance;
            }

            public void ExecuteQuery(string query)
            {
                if (IsConnected)
                {
                    _queryCount++;
                    Console.WriteLine($"[DB Query #{_queryCount}] Executing: {query}");
                }
                else
                {
                    Console.WriteLine("[DB Error] No Active Connection!");
                }
            }
        }
    }

    // ==============================================================
    // Main Method (Runner)
    // ==============================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 1. LOGGER SINGLETON TEST =====");

            // পেমেন্ট মডিউল লগার ডাকলো (ইন্টারফেস ব্যবহার করে)
            LoggerExample.ILogger paymentLogger = LoggerExample.Logger.GetInstance();
            paymentLogger.Log("Payment of 500 TK Successful.");

            // কার্ট মডিউল লগার ডাকলো (ইন্টারফেস ব্যবহার করে)
            LoggerExample.ILogger cartLogger = LoggerExample.Logger.GetInstance();
            cartLogger.Log("Item 'Shirt' added to cart.");

            // চেক করা হচ্ছে আসলেই তারা একই অবজেক্ট কি না
            Console.WriteLine($"[Check] Is paymentLogger same as cartLogger? : {ReferenceEquals(paymentLogger, cartLogger)}");

            paymentLogger.ShowAllLogs();


            Console.WriteLine("\n===== 2. DATABASE SINGLETON TEST =====");

            // প্রথমবার ডাকলেই শুধু কানেকশন তৈরি হবে (ইন্টারফেস ব্যবহার করে)
            Console.WriteLine("-> User 1 is requesting data...");
            DatabaseExample.IDatabaseConnectionPool db1 = DatabaseExample.DatabaseConnectionPool.GetInstance();
            db1.ExecuteQuery("SELECT * FROM Users");

            // দ্বিতীয়বার আর কানেকশন তৈরি হবে না
            Console.WriteLine("\n-> User 2 is requesting data...");
            DatabaseExample.IDatabaseConnectionPool db2 = DatabaseExample.DatabaseConnectionPool.GetInstance();
            db2.ExecuteQuery("SELECT * FROM Products WHERE Price > 100");

            // তৃতীয়বার
            Console.WriteLine("\n-> User 3 is requesting data...");
            DatabaseExample.IDatabaseConnectionPool db3 = DatabaseExample.DatabaseConnectionPool.GetInstance();
            db3.ExecuteQuery("UPDATE Inventory SET Stock = 50");

            Console.WriteLine($"\n[Check] Are all users using the exact same DB Connection? : {ReferenceEquals(db1, db2) && ReferenceEquals(db2, db3)}");
        }
    }
}
