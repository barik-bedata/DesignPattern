using System;
using System.Collections.Generic;

namespace ProxyPattern.CachingProxy
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface IDatabase
    {
        string GetUserRole(int userId);
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি আসল ডাটাবেস। এখানে কোয়েরি করতে অনেক সময় লাগে।
    public class RealDatabase : IDatabase
    {
        public string GetUserRole(int userId)
        {
            Console.WriteLine($"[RealDB] ⏳ Connecting to remote Database and running heavy SQL query for User {userId}...");
            // সিমুলেশন: ইউজারের রোল রিটার্ন করছে
            return userId == 1 ? "Admin" : "Guest";
        }
    }

    // ==========================================
    // 3. Caching Proxy
    // ==========================================
    // এটি রিকোয়েস্ট ক্যাশ করে রাখে যেন একই রিকোয়েস্ট বারবার ডাটাবেসে না যায়।
    public class CachingProxyDatabase : IDatabase
    {
        private IDatabase _realDb;
        private Dictionary<int, string> _cache = new Dictionary<int, string>();

        public CachingProxyDatabase(IDatabase realDb)
        {
            _realDb = realDb;
        }

        public string GetUserRole(int userId)
        {
            // ক্যাশে থাকলে ডাটাবেসে যাবে না, সরাসরি ক্যাশ থেকে দিয়ে দেবে!
            if (_cache.ContainsKey(userId))
            {
                Console.WriteLine($"[Proxy Cache] ⚡ Returned Role for User {userId} instantly from memory!");
                return _cache[userId];
            }

            // ক্যাশে না থাকলে আসল ডাটাবেসে কল করবে এবং পরে ক্যাশ করে রাখবে
            string role = _realDb.GetUserRole(userId);
            _cache[userId] = role;
            
            return role;
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 2. Caching Proxy Example (Database Query) ===\n");

            IDatabase realDb = new RealDatabase();
            IDatabase dbProxy = new CachingProxyDatabase(realDb);

            Console.WriteLine("--- First Request (Cache Miss) ---");
            string role1 = dbProxy.GetUserRole(1); // ডাটাবেসে হিট করবে
            Console.WriteLine($"Result: {role1}\n");

            Console.WriteLine("--- Second Request (Cache Hit) ---");
            string role2 = dbProxy.GetUserRole(1); // ডাটাবেসে যাবে না, ক্যাশ থেকে আসবে!
            Console.WriteLine($"Result: {role2}\n");

            Console.WriteLine("--- Requesting Different User (Cache Miss) ---");
            string role3 = dbProxy.GetUserRole(2); // ডাটাবেসে হিট করবে
            Console.WriteLine($"Result: {role3}\n");
        }
    }
}
