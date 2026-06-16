using System;
using System.Threading.Tasks;

namespace SingletonPattern
{
    // ==========================================
    // Method 1: Double-Check Locking (Traditional)
    // ==========================================
    public class ThreadSafeLogger
    {
        // volatile এনশিওর করে যে এই ভেরিয়েবলের ভ্যালু সবসময় সরাসরি Main Memory (RAM) থেকে পড়া হবে,
        // কোনো CPU Cache থেকে নয়। এর ফলে Instruction Reordering এর সমস্যাও হয় না।
        private static volatile ThreadSafeLogger _instance;
        
        // লক করার জন্য একটি ডামি অবজেক্ট
        private static readonly object _lock = new object();

        private ThreadSafeLogger()
        {
            Console.WriteLine("ThreadSafeLogger তৈরি হলো! (Method 1)");
        }

        public static ThreadSafeLogger GetInstance()
        {
            // First check (Lock করার আগেই চেক করা, যাতে পারফরম্যান্স ভালো থাকে)
            if (_instance == null)
            {
                // Lock: একসাথে একাধিক থ্রেড এখানে ঢুকতে পারবে না
                lock (_lock)
                {
                    // Second check (Lock এর ভেতরে আবার চেক করা, কারণ অন্য থ্রেড আগে ঢুকে অবজেক্ট বানিয়ে ফেলতে পারে)
                    if (_instance == null)
                    {
                        _instance = new ThreadSafeLogger();
                    }
                }
            }
            return _instance;
        }
    }


    // ==========================================
    // Method 2: Using Lazy<T> (Modern & Best Way C# 4+)
    // ==========================================
    public class ModernLogger
    {
        // Lazy<T> বাই ডিফল্ট থ্রেড-সেফ। এটাই C# এ বর্তমানে সবচেয়ে বেশি ব্যবহৃত হয়।
        private static readonly Lazy<ModernLogger> _lazyInstance = 
            new Lazy<ModernLogger>(() => new ModernLogger());

        private ModernLogger()
        {
            Console.WriteLine("ModernLogger তৈরি হলো! (Method 2 - Lazy)");
        }

        public static ModernLogger GetInstance()
        {
            return _lazyInstance.Value;
        }
    }

    class Program2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Testing Multithreaded Environment ---");

            // একসাথে ১০টি থ্রেড চালিয়ে (Parallel) অবজেক্ট ডাকার চেষ্টা করা হচ্ছে 
            Parallel.For(0, 10, i =>
            {
                var logger1 = ThreadSafeLogger.GetInstance();
                var logger2 = ModernLogger.GetInstance();
            });

            // আপনি আউটপুটে দেখবেন কনস্ট্রাক্টর শুধু একবারই কল হয়েছে!
            // যদি থ্রেড-সেফ না হতো, তবে ১০ বার "তৈরি হলো!" লেখাটি প্রিন্ট হতো।
        }
    }
}
