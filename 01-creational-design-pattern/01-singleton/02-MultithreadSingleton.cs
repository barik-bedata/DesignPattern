using System;
using System.Threading;
using System.Threading.Tasks;

// ==============================================================
// 🟢 SINGLETON PATTERN: MULTITHREADED EXAMPLES
// ==============================================================
namespace SingletonPattern.Multithread
{
    // ==============================================================
    // Example 1: Logger (লগার) - Double-Check Locking
    // ==============================================================
    namespace LoggerExample
    {
        // 🔹 ইন্টারফেস তৈরি করা হলো
        public interface ILogger
        {
            void Log(string message);
        }

        // 🔹 থ্রেড-সেফ সিঙ্গেলটন লগার ক্লাস যা ILogger ইমপ্লিমেন্ট করে
        public class ThreadSafeLogger : ILogger
        {
            // volatile: এনশিওর করে যে এই ভেরিয়েবলের ভ্যালু সবসময় সরাসরি Main Memory (RAM) থেকে পড়া হবে,
            // কোনো CPU Cache থেকে নয়। এর ফলে Instruction Reordering এর সমস্যাও হয় না।
            private static volatile ThreadSafeLogger _instance;
            
            // লক করার জন্য একটি ডামি অবজেক্ট (পাহারা দেওয়ার জন্য)
            private static readonly object _lock = new object();

            // ১. Private Constructor
            private ThreadSafeLogger()
            {
                Console.WriteLine(">> [ThreadSafeLogger] তৈরি হলো! (Double-Check Locking)");
                Thread.Sleep(100); // অবজেক্ট তৈরিতে সময় লাগছে বোঝাতে
            }

            // ২. Public Static Method
            public static ThreadSafeLogger GetInstance()
            {
                // First check (Lock করার আগেই চেক করা, যাতে অকারণে সব থ্রেড লক হয়ে পারফরম্যান্স নষ্ট না হয়)
                if (_instance == null)
                {
                    // Lock: একসাথে একাধিক থ্রেড এখানে ঢুকতে পারবে না। একজন ঢুকলে বাকিরা লাইনে দাঁড়াবে।
                    lock (_lock)
                    {
                        // Second check (Lock এর ভেতরে আবার চেক করা, কারণ লাইনে দাঁড়িয়ে থাকা অন্য কোনো থ্রেড হয়তো ততক্ষণে অবজেক্ট বানিয়ে ফেলেছে)
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeLogger();
                        }
                    }
                }
                return _instance;
            }

            public void Log(string message)
            {
                Console.WriteLine($"[ThreadSafe Log]: {message}");
            }
        }
    }


    // ==============================================================
    // Example 2: Database Connection Pool - Double-Check Locking
    // ==============================================================
    namespace DatabaseExample
    {
        // 🔹 ইন্টারফেস তৈরি করা হলো
        public interface IDatabaseConnectionPool
        {
            void ExecuteQuery(int threadId, string query);
        }

        // 🔹 থ্রেড-সেফ ডেটাবেস ক্লাস যা IDatabaseConnectionPool ইমপ্লিমেন্ট করে
        public class ThreadSafeDatabasePool : IDatabaseConnectionPool
        {
            // volatile: ভ্যালু সরাসরি Main Memory (RAM) থেকে পড়া হবে।
            private static volatile ThreadSafeDatabasePool _instance;
            
            // লক করার জন্য অবজেক্ট
            private static readonly object _lock = new object();

            public string ConnectionString { get; private set; }

            // ১. Private Constructor
            private ThreadSafeDatabasePool()
            {
                Console.WriteLine(">> [ThreadSafeDatabasePool] ডেটাবেস কানেকশন তৈরি হলো! (Double-Check Locking)");
                Thread.Sleep(200); // ডেটাবেস কানেক্ট হতে সময় লাগে
                ConnectionString = "Server=ThreadSafeSQL; Database=AppDB;";
            }

            // ২. Public Static Method
            public static ThreadSafeDatabasePool GetInstance()
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeDatabasePool();
                        }
                    }
                }
                return _instance;
            }

            public void ExecuteQuery(int threadId, string query)
            {
                Console.WriteLine($"[DB Query (Thread {threadId})] Executing: {query}");
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
            Console.WriteLine("===== MULTITHREADED SINGLETON TEST =====");
            Console.WriteLine("একসাথে ২০ জন ইউজার ডেটাবেস ও লগারের কাছে রিকোয়েস্ট পাঠাচ্ছে...\n");

            // Parallel.For ব্যবহার করে একসাথে (একই মিলি-সেকেন্ডে) একাধিক থ্রেড রান করা হচ্ছে
            Parallel.For(1, 21, i =>
            {
                // থ্রেড-সেফ না হলে এখানে একাধিকবার "তৈরি হলো!" লেখাটা প্রিন্ট হতো!
                
                // ১. লগার টেস্ট (ইন্টারফেস ব্যবহার করে)
                LoggerExample.ILogger logger = LoggerExample.ThreadSafeLogger.GetInstance();
                // logger.Log($"User {i} action logged."); 
                
                // ২. ডেটাবেস টেস্ট (ইন্টারফেস ব্যবহার করে, Double-Check Locking এর মাধ্যমে)
                DatabaseExample.IDatabaseConnectionPool db = DatabaseExample.ThreadSafeDatabasePool.GetInstance();
                // db.ExecuteQuery(i, "SELECT * FROM Users"); 
            });

            Console.WriteLine("\n[Success] ২০টা থ্রেড রান করার পরও দেখুন, অবজেক্টগুলো মাত্র একবারই তৈরি হয়েছে!");
        }
    }
}
