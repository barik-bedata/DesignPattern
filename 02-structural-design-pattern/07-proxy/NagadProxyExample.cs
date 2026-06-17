using System;
using System.Threading;

namespace ProxyPattern.RealWorld.Nagad
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface INagadService
    {
        string CheckBalance(string mobileNumber);
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি নগদের মেইন সার্ভার। এখান থেকে ব্যালেন্স জানতে ২-৩ সেকেন্ড সময় লাগে।
    public class RealNagadServer : INagadService
    {
        public string CheckBalance(string mobileNumber)
        {
            Console.WriteLine($"[Nagad Server] ⏳ Connecting to central database for {mobileNumber}...");
            Thread.Sleep(2000); // ২ সেকেন্ড সময় নিচ্ছে সিমুলেট করছি
            return "5,430 BDT";
        }
    }

    // ==========================================
    // 3. Proxy (Caching Proxy)
    // ==========================================
    // এটি নগদ অ্যাপ। ইউজার বারবার "Tap to view balance" এ চাপলে অ্যাপ বারবার সার্ভারে যায় না, 
    // সে কিছুক্ষণ ব্যালেন্স ক্যাশ করে রাখে।
    public class NagadCacheProxy : INagadService
    {
        private INagadService _realNagadServer;
        private string _cachedBalance = null;
        private DateTime _lastCheckedTime;

        public NagadCacheProxy(INagadService realNagadServer)
        {
            _realNagadServer = realNagadServer;
        }

        public string CheckBalance(string mobileNumber)
        {
            // যদি আগে ব্যালেন্স চেক করা থাকে এবং তা ১০ সেকেন্ডের পুরোনো না হয়, 
            // তবে ক্যাশ থেকে রিটার্ন করো (সার্ভারে প্রেশার কমাও)
            if (_cachedBalance != null && (DateTime.Now - _lastCheckedTime).TotalSeconds < 10)
            {
                Console.WriteLine($"[Nagad App] ⚡ Loaded from Cache instantly!");
                return _cachedBalance;
            }

            // ক্যাশ না থাকলে বা পুরোনো হয়ে গেলে সার্ভারে হিট করো
            _cachedBalance = _realNagadServer.CheckBalance(mobileNumber);
            _lastCheckedTime = DateTime.Now;

            return _cachedBalance;
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 3. Nagad Caching Proxy Example ===\n");

            INagadService realServer = new RealNagadServer();
            INagadService nagadApp = new NagadCacheProxy(realServer);

            Console.WriteLine("--- User Taps for Balance (1st Time) ---");
            string b1 = nagadApp.CheckBalance("01600000000"); // Takes 2 seconds
            Console.WriteLine($"Balance: {b1}\n");

            Console.WriteLine("--- User Taps for Balance Again (2nd Time within 10 sec) ---");
            string b2 = nagadApp.CheckBalance("01600000000"); // Instant from Cache
            Console.WriteLine($"Balance: {b2}\n");
        }
    }
}
