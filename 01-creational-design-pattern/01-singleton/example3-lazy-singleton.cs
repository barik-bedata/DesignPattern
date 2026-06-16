using System;

namespace SingletonPattern
{
    // ==========================================
    // Example: Lazy<T> in Action (Deferred Instantiation)
    // ==========================================
    public class HeavyDatabaseConnection
    {
        // Lazy<T> এর আসল ম্যাজিক হলো- এটি অ্যাপ চালু হওয়ার সাথে সাথে মেমরি ব্লক করে না।
        // যতক্ষণ না কেউ প্রথমবার GetInstance() ডাকছে, ততক্ষণ অবজেক্ট তৈরিই হয় না!
        private static readonly Lazy<HeavyDatabaseConnection> _lazyInstance = 
            new Lazy<HeavyDatabaseConnection>(() => new HeavyDatabaseConnection());

        private HeavyDatabaseConnection()
        {
            // ধরি, এই অবজেক্টটি তৈরি হতে অনেক সময় ও মেমরি লাগে
            Console.WriteLine("--> [HeavyDatabaseConnection] বিশাল ডেটাবেস কানেকশন তৈরি হচ্ছে... (Expensive Task!)");
        }

        public static HeavyDatabaseConnection GetInstance()
        {
            return _lazyInstance.Value;
        }

        public void ExecuteQuery(string query)
        {
            Console.WriteLine($"Query Executing: {query}");
        }
    }

    class Program3
    {
        static void Main(string[] args)
        {
            Console.WriteLine("১. অ্যাপ চালু হলো...");
            Console.WriteLine("২. অন্যান্য টুকটাক কাজ চলছে, কিন্তু ডেটাবেস কানেকশন এখনও তৈরি হয়নি। মেমরি একদম ফ্রি!");
            
            Console.ReadLine(); // (টেস্টিংয়ের জন্য আপনি চাইলে এখানে পজ করে দেখতে পারেন)

            Console.WriteLine("\n৩. ইউজার 'Show Users' বাটনে ক্লিক করলো। এখন ডেটাবেস লাগবে...");
            
            // ঠিক এই লাইনে এসে প্রথমবার কনস্ট্রাক্টর কল হবে (Lazy loading)
            var db1 = HeavyDatabaseConnection.GetInstance();
            db1.ExecuteQuery("SELECT * FROM Users");

            Console.WriteLine("\n৪. ইউজার আবার 'Show Orders' বাটনে ক্লিক করলো...");
            
            // এবার আর নতুন করে কানেকশন তৈরি হবে না, আগেরটাই ব্যবহার হবে
            var db2 = HeavyDatabaseConnection.GetInstance();
            db2.ExecuteQuery("SELECT * FROM Orders");

            Console.WriteLine($"\n[Test] দুটো কানেকশন কি একই? {ReferenceEquals(db1, db2)}");
        }
    }
}
