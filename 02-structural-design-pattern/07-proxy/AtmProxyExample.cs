using System;

namespace ProxyPattern.RealWorld.ATM
{
    // ==========================================
    // 1. Subject Interface (DIP)
    // ==========================================
    public interface IBankAccount
    {
        void Withdraw(int amount);
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি ব্যাংকের মেইন সার্ভার। এর কোনো পিন চেক করার দরকার নেই, সে সরাসরি টাকা মাইনাস করে দেয়।
    public class RealBankAccount : IBankAccount
    {
        private int _balance = 10000;

        public void Withdraw(int amount)
        {
            _balance -= amount;
            Console.WriteLine($"[Core Banking] 💸 Successfully withdrawn {amount} BDT. Remaining Balance: {_balance} BDT.");
        }
    }

    // ==========================================
    // 3. Proxy (Protection Proxy)
    // ==========================================
    // এটি হলো ATM বুথ! এটি মেইন সার্ভারের একটি প্রক্সি।
    // টাকা তোলার আগে সে পিন চেক করে সিকিউরিটি নিশ্চিত করে।
    public class AtmProxy : IBankAccount
    {
        private IBankAccount _realBankAccount;
        private string _correctPin = "1234";
        private string _userEnteredPin;

        public AtmProxy(IBankAccount realBankAccount, string userEnteredPin)
        {
            _realBankAccount = realBankAccount;
            _userEnteredPin = userEnteredPin;
        }

        public void Withdraw(int amount)
        {
            // প্রক্সির কাজ: সিকিউরিটি চেক করা (Protection)
            if (_userEnteredPin == _correctPin)
            {
                Console.WriteLine("[ATM Proxy] ✅ PIN Matched! Forwarding request to Core Banking...");
                _realBankAccount.Withdraw(amount);
            }
            else
            {
                Console.WriteLine("[ATM Proxy] ❌ Access Denied! Incorrect PIN.");
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
            Console.WriteLine("=== 1. ATM Protection Proxy Example ===\n");

            IBankAccount realBank = new RealBankAccount();

            // কাস্টমার ভুল পিন দিয়ে ট্রাই করছে
            Console.WriteLine("--- Trying with Wrong PIN ---");
            IBankAccount atmWrongPin = new AtmProxy(realBank, "9999");
            atmWrongPin.Withdraw(2000);

            Console.WriteLine("\n--- Trying with Correct PIN ---");
            IBankAccount atmCorrectPin = new AtmProxy(realBank, "1234");
            atmCorrectPin.Withdraw(2000);
        }
    }
}
