using System;

namespace BehavioralDesignPattern.ChainOfResponsibility
{
    // ==========================================
    // 1. Request Object
    // ==========================================
    public class LoanRequest
    {
        public string CustomerName { get; set; }
        public int Amount { get; set; }
        public string Purpose { get; set; }

        public LoanRequest(string customerName, int amount, string purpose)
        {
            CustomerName = customerName;
            Amount = amount;
            Purpose = purpose;
        }
    }

    // ==========================================
    // 2. Handler Interface (DIP)
    // ==========================================
    public interface ILoanApprover
    {
        void SetNext(ILoanApprover nextApprover);
        void ProcessLoan(LoanRequest request);
    }

    // ==========================================
    // 3. Base Handler — এখন 100% DRY & SOLID
    // ==========================================
    // এই Template Method-এ ProcessLoan একবারই লেখা থাকে — 
    // subclass শুধু ঠিক করবে "সে হ্যান্ডেল করতে পারবে কি না (CanHandle)" 
    // এবং "হ্যান্ডেল করলে কী হবে (HandleLoan)"।
    public abstract class BaseLoanApprover : ILoanApprover
    {
        protected ILoanApprover _nextApprover;
        protected readonly string _roleName;

        protected BaseLoanApprover(string roleName)
        {
            _roleName = roleName;
        }

        public void SetNext(ILoanApprover nextApprover)
        {
            _nextApprover = nextApprover;
        }

        // Chain traversal logic — এখন একটাই জায়গায় আছে (Template Method)।
        public void ProcessLoan(LoanRequest request)
        {
            // ১. আমি কি এটা হ্যান্ডেল করতে পারবো? (ডিসিশন নেবে সাব-ক্লাস)
            if (CanHandle(request))
            {
                HandleLoan(request);
                return;
            }

            // ২. না পারলে সামনের জনকে পাস করো
            if (_nextApprover != null)
            {
                Console.WriteLine($"[{_roleName}] Passing to next department...");
                _nextApprover.ProcessLoan(request);
                return;
            }

            // Terminal case: chain শেষ হয়ে গেছে, কেউ approve করতে পারেনি।
            Console.WriteLine($"[{_roleName}] REJECTED. No approver in the chain can authorize {request.Amount} BDT for {request.CustomerName}.");
        }

        // সাব-ক্লাসকে এই দুটো মেথড ইমপ্লিমেন্ট করতে হবে
        protected abstract bool CanHandle(LoanRequest request);
        protected abstract void HandleLoan(LoanRequest request);
    }

    // ==========================================
    // 4. Concrete Handlers
    // ==========================================

    public class Cashier : BaseLoanApprover
    {
        private readonly int _approvalLimit;

        public Cashier(int approvalLimit) : base("Cashier") 
        {
            _approvalLimit = approvalLimit;
        }

        protected override bool CanHandle(LoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(LoanRequest request)
        {
            Console.WriteLine($"[Cashier] 💵 Approved loan of {request.Amount} BDT for {request.CustomerName}.");
        }
    }

    public class Manager : BaseLoanApprover
    {
        private readonly int _approvalLimit;

        public Manager(int approvalLimit) : base("Manager") 
        {
            _approvalLimit = approvalLimit;
        }

        protected override bool CanHandle(LoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(LoanRequest request)
        {
            Console.WriteLine($"[Manager] 💼 Approved loan of {request.Amount} BDT for {request.CustomerName}.");
        }
    }

    public class Director : BaseLoanApprover
    {
        private readonly int _approvalLimit;

        public Director(int approvalLimit = int.MaxValue) : base("Director") 
        {
            _approvalLimit = approvalLimit;
        }

        protected override bool CanHandle(LoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(LoanRequest request)
        {
            Console.WriteLine($"[Director] 👑 Executive Approval granted for loan of {request.Amount} BDT for {request.CustomerName}.");
        }
    }

    // ==========================================
    // নতুন হ্যান্ডলার: Security / Credit Score Checker 
    // (প্রমাণ যে আমাদের ডিজাইন এখন 100% Flexible)
    // ==========================================
    public class SecurityChecker : BaseLoanApprover
    {
        public SecurityChecker() : base("Security Dept") { }

        protected override bool CanHandle(LoanRequest request)
        {
            // সে টাকার অ্যামাউন্ট দেখবেই না! 
            // যদি কাস্টমারের নাম "Hacker" হয়, তবে সে এটা হ্যান্ডেল করবে (মানে রিজেক্ট করে চেইন থামিয়ে দেবে)।
            return request.CustomerName == "Hacker"; 
        }

        protected override void HandleLoan(LoanRequest request)
        {
            Console.WriteLine($"[Security Dept] 🛑 REJECTED! Customer '{request.CustomerName}' is blacklisted.");
        }
    }

    // ==========================================
    // 5. Client Code
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
            Console.WriteLine("=== Chain of Responsibility Pattern (100% SOLID & Flexible) ===\n");

            // হ্যান্ডলারগুলো তৈরি
            ILoanApprover security = new SecurityChecker(); // নতুন ডিপার্টমেন্ট অ্যাড করা হলো!
            ILoanApprover cashier = new Cashier(approvalLimit: 150000);   
            ILoanApprover manager = new Manager(approvalLimit: 800000);  
            ILoanApprover director = new Director(approvalLimit: 5000000); 

            // চেইন তৈরি: Security -> Cashier -> Manager -> Director
            security.SetNext(cashier);
            cashier.SetNext(manager);
            manager.SetNext(director);

            Console.WriteLine("=== Request 1 ===");
            LoanRequest req1 = new LoanRequest("Rahim", 50000, "Buy a Laptop");
            security.ProcessLoan(req1);

            Console.WriteLine("\n=== Request 2 ===");
            LoanRequest req2 = new LoanRequest("Karim", 300000, "Buy a Car");
            security.ProcessLoan(req2);

            Console.WriteLine("\n=== Request 3 ===");
            LoanRequest req3 = new LoanRequest("Jodu", 1500000, "Start a Business");
            security.ProcessLoan(req3);

            Console.WriteLine("\n=== Request 4 (Blacklisted User) ===");
            // এই রিকোয়েস্টটি Security Checker প্রথমেই আটকে দেবে!
            LoanRequest req4 = new LoanRequest("Hacker", 10000, "Buy Servers");
            security.ProcessLoan(req4);

            Console.WriteLine("\n=== Request 5 (Too High Amount) ===");
            LoanRequest req5 = new LoanRequest("Salam", 10000000, "Buy a Factory");
            security.ProcessLoan(req5);
        }
    }
}