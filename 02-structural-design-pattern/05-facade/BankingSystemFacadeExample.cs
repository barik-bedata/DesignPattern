using System;

namespace FacadePattern.Banking
{
    // ==========================================
    // 1. Interfaces for Subsystems (DIP মানার জন্য)
    // ==========================================
    public interface IAccountChecker { bool IsActive(int accountNumber); }
    public interface ISecurityCodeChecker { bool IsCodeValid(int pin); }
    public interface IFundsManager { bool HaveEnoughMoney(double amount); void DeductMoney(double amount); }

    // ==========================================
    // 2. Concrete Subsystems
    // ==========================================
    public class AccountDatabase : IAccountChecker
    {
        public bool IsActive(int accountNumber)
        {
            Console.WriteLine($"[AccountDB] Checking if account {accountNumber} is active... OK.");
            return true;
        }
    }

    public class SecurityAuth : ISecurityCodeChecker
    {
        public bool IsCodeValid(int pin)
        {
            Console.WriteLine("[Security] Authenticating PIN code... SUCCESS.");
            return true;
        }
    }

    public class LedgerManager : IFundsManager
    {
        private double _balance = 10000.00; // ডিফল্ট ব্যালেন্স

        public bool HaveEnoughMoney(double amount)
        {
            Console.WriteLine($"[Ledger] Checking if balance covers ${amount}... YES.");
            return _balance >= amount;
        }

        public void DeductMoney(double amount)
        {
            _balance -= amount;
            Console.WriteLine($"[Ledger] Deducted ${amount}. New Balance: ${_balance}");
        }
    }

    // ==========================================
    // 3. The Facade (সিম্পল ইন্টারফেস)
    // ==========================================
    public class BankingFacade
    {
        // DIP Followed
        private readonly IAccountChecker _accountChecker;
        private readonly ISecurityCodeChecker _securityChecker;
        private readonly IFundsManager _fundsManager;

        public BankingFacade(IAccountChecker accountChecker, ISecurityCodeChecker securityChecker, IFundsManager fundsManager)
        {
            _accountChecker = accountChecker;
            _securityChecker = securityChecker;
            _fundsManager = fundsManager;
        }

        // উইথড্র করার জন্য সিম্পল একটি মেথড। ব্যাকএন্ডের ৫টা চেক ক্লায়েন্টকে করতে হবে না।
        public void WithdrawMoney(int accountNumber, int pin, double amount)
        {
            Console.WriteLine("\n=== [Facade] Initiating Withdrawal Process ===");
            
            // Facade নিজে সব সাবসিস্টেম চেক করছে
            if (_accountChecker.IsActive(accountNumber) && 
                _securityChecker.IsCodeValid(pin) && 
                _fundsManager.HaveEnoughMoney(amount))
            {
                Console.WriteLine("All security and balance checks passed.");
                _fundsManager.DeductMoney(amount);
                Console.WriteLine("Transaction Successful! Please collect your cash.\n");
            }
            else
            {
                Console.WriteLine("Transaction Failed!\n");
            }
        }
    }

    // ==========================================
    // 4. Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            // সাব-সিস্টেম তৈরি
            IAccountChecker db = new AccountDatabase();
            ISecurityCodeChecker auth = new SecurityAuth();
            IFundsManager ledger = new LedgerManager();

            // Facade তৈরি
            BankingFacade atmMachine = new BankingFacade(db, auth, ledger);

            // Client (কাস্টমার) শুধু এটিএম মেশিনের বাটন চাপছে! তার জানার দরকার নেই যে ভেতরে কত লজিক আছে!
            atmMachine.WithdrawMoney(12345678, 1234, 500.00);
        }
    }
}
