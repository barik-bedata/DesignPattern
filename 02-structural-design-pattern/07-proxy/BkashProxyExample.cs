using System;

namespace ProxyPattern.RealWorld.Bkash
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface IBkashService
    {
        void SendMoney(string receiverNumber, int amount);
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি বিকাশের মেইন সার্ভার। সে শুধু সেন্ড মানি করতে জানে।
    public class RealBkashServer : IBkashService
    {
        public void SendMoney(string receiverNumber, int amount)
        {
            Console.WriteLine($"[bKash Server] 📲 Successfully sent {amount} BDT to {receiverNumber}.");
        }
    }

    // ==========================================
    // 3. Proxy (Smart / Validation Proxy)
    // ==========================================
    // এটি হলো বিকাশ অ্যাপ। সার্ভারে রিকোয়েস্ট পাঠানোর আগে সে লিমিট চেক করে এবং চার্জ কাটে।
    public class BkashAppProxy : IBkashService
    {
        private IBkashService _realBkashServer;
        private int _dailyLimit = 25000;
        private int _sentToday = 0;

        public BkashAppProxy(IBkashService realBkashServer)
        {
            _realBkashServer = realBkashServer;
        }

        public void SendMoney(string receiverNumber, int amount)
        {
            // প্রক্সির কাজ ১: ভ্যালিডেশন (Smart Proxy)
            if (_sentToday + amount > _dailyLimit)
            {
                Console.WriteLine($"[bKash App] ❌ Transaction Failed! Daily limit of 25,000 BDT exceeded.");
                return;
            }

            // প্রক্সির কাজ ২: চার্জ হিসাব করা
            int charge = (amount >= 100) ? 5 : 0;
            Console.WriteLine($"[bKash App] Transaction fee applied: {charge} BDT. Forwarding to server...");

            // আসল সার্ভারকে কল করা (Delegation)
            _realBkashServer.SendMoney(receiverNumber, amount);
            
            // প্রক্সির কাজ ৩: রেকর্ড রাখা
            _sentToday += amount;
            Console.WriteLine($"[bKash App Log] Total sent today: {_sentToday} BDT.\n");
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 2. bKash Smart/Validation Proxy Example ===\n");

            IBkashService realServer = new RealBkashServer();
            IBkashService bkashApp = new BkashAppProxy(realServer);

            // ১০ হাজার টাকা সেন্ড করা হচ্ছে
            bkashApp.SendMoney("01700000000", 10000);

            // ২০ হাজার টাকা সেন্ড করার চেষ্টা (লিমিট ক্রস করবে)
            bkashApp.SendMoney("01711111111", 20000);
        }
    }
}
