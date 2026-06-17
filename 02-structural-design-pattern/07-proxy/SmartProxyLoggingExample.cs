using System;

namespace ProxyPattern.SmartProxy
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface IBankService
    {
        void Withdraw(int amount);
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি হলো ব্যাংকের আসল সার্ভিস, যা শুধু টাকা তোলার লজিক জানে।
    public class RealBankService : IBankService
    {
        public void Withdraw(int amount)
        {
            Console.WriteLine($"[Bank] Successfully withdrew ${amount} from account.");
        }
    }

    // ==========================================
    // 3. Smart/Logging Proxy
    // ==========================================
    // এই প্রক্সিটি রিয়েল অবজেক্টকে কল করার আগে এবং পরে কিছু এক্সট্রা (Smart) কাজ করে, যেমন লগিং বা সিকিউরিটি অডিট।
    // আসল ক্লাসটিকে মডিফাই না করেই আমরা লগিং ফিচার অ্যাড করছি!
    public class LoggingProxyBank : IBankService
    {
        private IBankService _realBank;

        public LoggingProxyBank(IBankService realBank)
        {
            _realBank = realBank;
        }

        public void Withdraw(int amount)
        {
            // আসল কাজ করার আগে এক্সট্রা কাজ (Before Execution Log)
            Console.WriteLine($"[Smart Proxy Log] Transaction started at {DateTime.Now}. Attempting to withdraw ${amount}.");

            try
            {
                // আসল কাজ
                _realBank.Withdraw(amount);

                // আসল কাজ করার পরে এক্সট্রা কাজ (After Execution Log)
                Console.WriteLine($"[Smart Proxy Log] Transaction completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Smart Proxy Log] Transaction FAILED: {ex.Message}");
            }
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 3. Smart Proxy Example (Logging & Auditing) ===\n");

            // রিয়েল সার্ভিস
            IBankService realBank = new RealBankService();

            // প্রক্সির ভেতর রিয়েল সার্ভিস পুশ করে দিচ্ছি
            IBankService atmProxy = new LoggingProxyBank(realBank);

            // ক্লায়েন্ট টাকা তুলছে। সে শুধু টাকা পাবে, কিন্তু পেছনে প্রক্সি সব লগিং করে ফেলছে!
            Console.WriteLine("Client goes to ATM and requests $500...");
            atmProxy.Withdraw(500);
        }
    }
}
