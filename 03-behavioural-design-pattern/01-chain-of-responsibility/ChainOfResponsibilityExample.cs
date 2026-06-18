using System;

namespace BehavioralDesignPattern.ChainOfResponsibility
{
    // ==========================================
    // 1. Request Interface & DTO (Data Object)
    // ==========================================
    
    /* 
     * NOTE: For simple applications, creating an interface for a simple DTO 
     * (Data Transfer Object) is considered "Over-engineering" and it is usually 
     * better to avoid it. However, for large Enterprise applications where we might 
     * have completely different types of requests (e.g., HomeLoanRequest, CarLoanRequest) 
     * that share common behaviors, using an interface makes the system highly scalable.
     */
    public interface ILoanRequest
    {
        string CustomerName { get; }
        int Amount { get; }
        string Purpose { get; }
    }

    public class LoanRequest : ILoanRequest
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
        void ProcessLoan(ILoanRequest request); // এখন ইন্টারফেস রিসিভ করছে!
    }

    // ==========================================
    // 3. Base Handler — এখন 100% DRY & SOLID
    // ==========================================
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
        public void ProcessLoan(ILoanRequest request)
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

        // সাব-ক্লাসকে এই দুটো মেথড ইমপ্লিমেন্ট করতে হবে (প্যারামিটার হিসেবে ইন্টারফেস)
        protected abstract bool CanHandle(ILoanRequest request);
        protected abstract void HandleLoan(ILoanRequest request);
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

        protected override bool CanHandle(ILoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(ILoanRequest request)
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

        protected override bool CanHandle(ILoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(ILoanRequest request)
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

        protected override bool CanHandle(ILoanRequest request)
        {
            return request.Amount <= _approvalLimit;
        }

        protected override void HandleLoan(ILoanRequest request)
        {
            Console.WriteLine($"[Director] 👑 Executive Approval granted for loan of {request.Amount} BDT for {request.CustomerName}.");
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
            Console.WriteLine("=== Chain of Responsibility Pattern (Enterprise Level DTO + DIP) ===\n");

            ILoanApprover security = new SecurityChecker(); 
            ILoanApprover cashier = new Cashier(approvalLimit: 150000);   
            ILoanApprover manager = new Manager(approvalLimit: 800000);  
            ILoanApprover director = new Director(approvalLimit: 5000000); 

            security.SetNext(cashier);
            cashier.SetNext(manager);
            manager.SetNext(director);

            Console.WriteLine("=== Request 1 ===");
            // ক্লায়েন্ট এখন রিকোয়েস্ট তৈরি করে ইন্টারফেসে রাখছে!
            ILoanRequest req1 = new LoanRequest("Rahim", 50000, "Buy a Laptop");
            security.ProcessLoan(req1);

            Console.WriteLine("\n=== Request 2 ===");
            ILoanRequest req2 = new LoanRequest("Karim", 300000, "Buy a Car");
            security.ProcessLoan(req2);

            Console.WriteLine("\n=== Request 3 ===");
            ILoanRequest req3 = new LoanRequest("Jodu", 1500000, "Start a Business");
            security.ProcessLoan(req3);
        }
    }

    // Security Checker
    public class SecurityChecker : BaseLoanApprover
    {
        public SecurityChecker() : base("Security Dept") { }

        protected override bool CanHandle(ILoanRequest request)
        {
            return request.CustomerName == "Hacker"; 
        }

        protected override void HandleLoan(ILoanRequest request)
        {
            Console.WriteLine($"[Security Dept] 🛑 REJECTED! Customer '{request.CustomerName}' is blacklisted.");
        }
    }
}