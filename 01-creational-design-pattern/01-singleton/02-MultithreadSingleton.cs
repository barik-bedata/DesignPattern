using System;
using System.Threading;
using System.Threading.Tasks;

// ==============================================================
// 🟢 SINGLETON PATTERN: MULTITHREADED EXAMPLES
// ==============================================================
namespace SingletonPattern.Multithread
{
    // ==============================================================
    // Method 1: Double-Check Locking (লগার এর উদাহরণ)
    // ==============================================================
    // এটি পুরোনো পদ্ধতি কিন্তু ইন্টারভিউতে অনেক বেশি জিজ্ঞেস করা হয়।
    public class ThreadSafeLogger
    {
        // volatile: এনশিওর করে যে এই ভেরিয়েবলের ভ্যালু সবসময় সরাসরি Main Memory (RAM) থেকে পড়া হবে,
        // কোনো CPU Cache থেকে নয়। এর ফলে Instruction Reordering এর সমস্যাও হয় না।
        private static volatile ThreadSafeLogger _instance;
        
        // লক করার জন্য একটি ডামি অবজেক্ট (পাহারা দেওয়ার জন্য)
        private static readonly object _lock = new object();

        private ThreadSafeLogger()
        {
            Console.WriteLine(">> [ThreadSafeLogger] তৈরি হলো! (Double-Check Locking)");
            Thread.Sleep(100); // অবজেক্ট তৈরিতে সময় লাগছে বোঝাতে
        }

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


    // ==============================================================
    // Method 2: Using Lazy<T> (ফুল ডেটাবেস এর উদাহরণ - Modern & Best Way)
    // ==============================================================
    // C# এ বর্তমানে এটিই বেস্ট প্র্যাকটিস। Lazy<T> নিজে থেকেই 100% থ্রেড-সেফ।
    // কোনো Lock বা Volatile নিয়ে মাথা ঘামাতে হয় না।
    public class ModernDatabasePool
    {
        // Lazy<T> শুধু দরকারের সময়ই অবজেক্ট বানাবে এবং মাল্টিথ্রেডিংয়েও একটাই বানাবে।
        private static readonly Lazy<ModernDatabasePool> _lazyInstance = 
            new Lazy<ModernDatabasePool>(() => new ModernDatabasePool());

        public string ConnectionString { get; private set; }

        private ModernDatabasePool()
        {
            Console.WriteLine(">> [ModernDatabasePool] ডেটাবেস কানেকশন তৈরি হলো! (Lazy<T> Magic)");
            Thread.Sleep(200); // ডেটাবেস কানেক্ট হতে সময় লাগে
            ConnectionString = "Server=ModernSQL; Database=AppDB;";
        }

        public static ModernDatabasePool GetInstance()
        {
            // .Value ডাকলেই সে অবজেক্ট বানায় (যদি আগে থেকে না থাকে) বা আগেরটা ফেরত দেয়।
            return _lazyInstance.Value;
        }

        public void ExecuteQuery(int threadId, string query)
        {
            Console.WriteLine($"[DB Query (Thread {threadId})] Executing: {query}");
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
            Console.WriteLine("একসাথে ১০০ জন ইউজার ডেটাবেস ও লগারের কাছে রিকোয়েস্ট পাঠাচ্ছে...\n");

            // Parallel.For ব্যবহার করে একসাথে (একই মিলি-সেকেন্ডে) একাধিক থ্রেড রান করা হচ্ছে
            Parallel.For(1, 20, i =>
            {
                // থ্রেড-সেফ না হলে এখানে একাধিকবার "তৈরি হলো!" লেখাটা প্রিন্ট হতো!
                
                // ১. লগার টেস্ট (Double-Check Locking)
                var logger = ThreadSafeLogger.GetInstance();
                
                // ২. ডেটাবেস টেস্ট (Lazy<T>)
                var db = ModernDatabasePool.GetInstance();
                
                // ইচ্ছা করলে কুয়েরি রান করাতে পারি (কনসোল ভিড় এড়ানোর জন্য আপাতত অফ রাখছি)
                // db.ExecuteQuery(i, "SELECT * FROM Users"); 
            });

            Console.WriteLine("\n[Success] ১০০টা থ্রেড রান করার পরও দেখুন, অবজেক্টগুলো মাত্র একবারই তৈরি হয়েছে!");
        }
    }
}
