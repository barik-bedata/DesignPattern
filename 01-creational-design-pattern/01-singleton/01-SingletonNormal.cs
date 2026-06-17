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
    // কেন Singleton? পুরো অ্যাপ্লিকেশনে একটাই লগার থাকা উচিত, যাতে সব ক্লাসের লগ 
    // একই ফাইলে বা একই লিস্টে জমা হয়। বারবার নতুন লগার বানালে লগ হারিয়ে যাবে।
    public class Logger
    {
        private static Logger _instance;
        private List<string> _logs;

        // ১. Private Constructor: বাইরের কেউ `new Logger()` লিখতে পারবে না।
        private Logger()
        {
            _logs = new List<string>();
            Console.WriteLine(">> [Logger] সিস্টেমের প্রধান লগার তৈরি হলো! (একবারই হবে)");
        }

        // ২. Public Static Method: যেখান থেকেই লগার ডাকা হোক, এই মেথডটাই কল করতে হবে।
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
            foreach(var log in _logs)
            {
                Console.WriteLine(log);
            }
        }
    }

    // ==============================================================
    // Example 2: Database Connection Pool (ফুল ডেটাবেস এক্সাম্পল)
    // ==============================================================
    // কেন Singleton? ডেটাবেস কানেকশন তৈরি করা খুবই ভারী (Expensive) কাজ। 
    // প্রতিটি ইউজার রিকোয়েস্টের জন্য নতুন কানেকশন বানালে ডেটাবেস সার্ভার ক্র্যাশ করতে পারে। 
    // তাই একটিমাত্র கனেকশন পুল (Connection Pool) বানিয়ে সবার মাঝে শেয়ার করা হয়।
    public class DatabaseConnectionPool
    {
        private static DatabaseConnectionPool _instance;
        
        public string ConnectionString { get; private set; }
        public bool IsConnected { get; private set; }
        private int _queryCount = 0;

        // প্রাইভেট কনস্ট্রাক্টর
        private DatabaseConnectionPool()
        {
            Console.WriteLine(">> [Database] ডেটাবেসের সাথে কানেকশন তৈরি হচ্ছে... (Taking Time)");
            Thread.Sleep(500); // কানেকশন তৈরিতে সময় লাগছে বোঝানোর জন্য
            
            ConnectionString = "Server=myServer;Database=myDB;User=admin;";
            IsConnected = true;
            Console.WriteLine(">> [Database] কানেকশন সাকসেসফুল!");
        }

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
            if(IsConnected)
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

    // ==============================================================
    // Main Method (Runner)
    // ==============================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 1. LOGGER SINGLETON TEST =====");
            
            // Payment মডিউল লগার ডাকলো
            var paymentLogger = Logger.GetInstance();
            paymentLogger.Log("Payment of 500 TK Successful.");

            // Cart মডিউল লগার ডাকলো
            var cartLogger = Logger.GetInstance();
            cartLogger.Log("Item 'Shirt' added to cart.");

            // চেক করা হচ্ছে আসলেই তারা একই অবজেক্ট কি না
            Console.WriteLine($"[Check] Is paymentLogger same as cartLogger? : {ReferenceEquals(paymentLogger, cartLogger)}");
            
            paymentLogger.ShowAllLogs(); // দুটো লগই এখানে দেখাবে কারণ অবজেক্ট একটাই!


            Console.WriteLine("\n===== 2. DATABASE SINGLETON TEST =====");
            
            // প্রথমবার ডাকলেই শুধু কানেকশন তৈরি হবে
            Console.WriteLine("-> User 1 is requesting data...");
            var db1 = DatabaseConnectionPool.GetInstance();
            db1.ExecuteQuery("SELECT * FROM Users");

            // দ্বিতীয়বার আর কানেকশন তৈরি হবে না, আগেরটাই ব্যবহার হবে (0ms delay)
            Console.WriteLine("\n-> User 2 is requesting data...");
            var db2 = DatabaseConnectionPool.GetInstance();
            db2.ExecuteQuery("SELECT * FROM Products WHERE Price > 100");

            // তৃতীয়বার
            Console.WriteLine("\n-> User 3 is requesting data...");
            var db3 = DatabaseConnectionPool.GetInstance();
            db3.ExecuteQuery("UPDATE Inventory SET Stock = 50");

            Console.WriteLine($"\n[Check] Are all users using the exact same DB Connection? : {ReferenceEquals(db1, db2) && ReferenceEquals(db2, db3)}");
        }
    }
}
