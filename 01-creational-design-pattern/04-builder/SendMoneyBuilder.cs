using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Telescoping Constructor Anti-pattern)
// ==============================================================
// সমস্যা: একটি "Send Money" রিকোয়েস্ট তৈরি করতে অনেকগুলো প্যারামিটার লাগে।
// সাধারণ কন্সট্রাক্টর ব্যবহার করলে কোনটা রেফারেন্স, কোনটা প্রোমোকোড আর কোনটা চার্জ-ফ্রি ফ্ল্যাগ, 
// সেটা মনে রাখা অসম্ভব। এর ফলে ভুল অ্যাকাউন্টে বা ভুল অ্যামাউন্ট সেন্ড মানি হয়ে যেতে পারে!

namespace BuilderPattern.SendMoney.Violation
{
    // ১. প্রোডাক্ট ক্লাস (The God Object)
    public class SendMoneyRequest
    {
        public string SenderNumber { get; }
        public string ReceiverNumber { get; }
        public decimal Amount { get; }
        public string Reference { get; }
        public bool IsChargeFree { get; }
        public string PromoCode { get; }

        // Telescoping Constructor - অনেকগুলো প্যারামিটার, যার মধ্যে কিছু ঐচ্ছিক (Optional)
        public SendMoneyRequest(string sender, string receiver, decimal amount, string reference, bool isChargeFree, string promoCode)
        {
            SenderNumber = sender;
            ReceiverNumber = receiver;
            Amount = amount;
            Reference = reference;
            IsChargeFree = isChargeFree;
            PromoCode = promoCode;
        }

        public void Execute()
        {
            Console.WriteLine($"[Violation Send Money...] From: {SenderNumber} To: {ReceiverNumber} | Amount: {Amount} TK");
            Console.WriteLine($"   -> Ref: {Reference ?? "N/A"}, Charge Free: {IsChargeFree}, Promo: {PromoCode ?? "N/A"}\n");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION: কন্সট্রাক্টর দিয়ে Send Money ===\n");
            
            // সমস্যা ১: শুধু টাকা পাঠাতে চাই, কিন্তু বাধ্য হয়ে null, false, null পাঠাতে হচ্ছে!
            var normalTransfer = new SendMoneyRequest("01711111111", "01922222222", 500, null, false, null);
            normalTransfer.Execute();

            // সমস্যা ২: প্যারামিটার সিরিয়াল মনে রাখা কঠিন।
            var promoTransfer = new SendMoneyRequest("01711111111", "01833333333", 1000, "Gift", true, "EID2026");
            promoTransfer.Execute();
        }
    }
}


// ==============================================================
// ✅ SOLUTION: The Good Way (Using Builder Pattern)
// ==============================================================
// সমাধান: "Send Money" রিকোয়েস্ট তৈরি করার কাজটা Builder এর হাতে দেওয়া।

namespace BuilderPattern.SendMoney.Solution
{
    // 🧱 বিল্ডার প্যাটার্নের ৫টি মূল কম্পোনেন্ট:

    // ১. Product: যে জটিল রিকোয়েস্ট অবজেক্টটি আমরা তৈরি করতে চাচ্ছি
    // ==============================================================
    public class SendMoneyRequest
    {
        public string SenderNumber { get; set; }
        public string ReceiverNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public bool IsChargeFree { get; set; }
        public string PromoCode { get; set; }

        public void Execute()
        {
            Console.WriteLine($"[✅ Builder Send Money...] From: {SenderNumber} To: {ReceiverNumber} | Amount: {Amount} TK");
            Console.WriteLine($"   -> Ref: {Reference ?? "N/A"}, Charge Free: {IsChargeFree}, Promo: {PromoCode ?? "N/A"}\n");
        }
    }

    // ২. Builder Interface: রিকোয়েস্ট ধাপে ধাপে তৈরি করার ধাপগুলো
    // ==============================================================
    public interface ISendMoneyBuilder
    {
        ISendMoneyBuilder SetSender(string sender);
        ISendMoneyBuilder SetReceiver(string receiver);
        ISendMoneyBuilder SetAmount(decimal amount);
        ISendMoneyBuilder SetReference(string reference);
        ISendMoneyBuilder MakeChargeFree();
        ISendMoneyBuilder ApplyPromoCode(string promoCode);
        SendMoneyRequest Build();
    }

    // ৩. Concrete Builder: আসল বিল্ডার যে রিকোয়েস্ট অবজেক্ট তৈরি করে
    // ==============================================================
    public class MobileBankingSendMoneyBuilder : ISendMoneyBuilder
    {
        private SendMoneyRequest _request = new SendMoneyRequest();

        public ISendMoneyBuilder SetSender(string sender)
        {
            _request.SenderNumber = sender;
            return this;
        }

        public ISendMoneyBuilder SetReceiver(string receiver)
        {
            _request.ReceiverNumber = receiver;
            return this;
        }

        public ISendMoneyBuilder SetAmount(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("অ্যামাউন্ট ০ এর বেশি হতে হবে!");
            _request.Amount = amount;
            return this;
        }

        public ISendMoneyBuilder SetReference(string reference)
        {
            _request.Reference = reference;
            return this;
        }

        public ISendMoneyBuilder MakeChargeFree()
        {
            _request.IsChargeFree = true;
            return this;
        }

        public ISendMoneyBuilder ApplyPromoCode(string promoCode)
        {
            _request.PromoCode = promoCode;
            return this;
        }

        public SendMoneyRequest Build()
        {
            // ভ্যালিডেশন: বিল্ড করার আগে চেক করা
            if (string.IsNullOrEmpty(_request.SenderNumber) || string.IsNullOrEmpty(_request.ReceiverNumber))
            {
                throw new InvalidOperationException("সেন্ডার এবং রিসিভার নাম্বার অবশ্যই দিতে হবে!");
            }
            
            SendMoneyRequest completedRequest = _request;
            _request = new SendMoneyRequest(); // রিসেট
            return completedRequest;
        }
    }

    // ৪. Director: (ঐচ্ছিক / Optional) 
    // ==============================================================
    // *নোট: এই ইনভয়েস/পেমেন্ট ডিজাইনে কোনো আলাদা ডিরেক্টরের প্রয়োজন নেই, 
    // কারণ ব্যবহারকারী নিজেই ফ্লেক্সিবলভাবে রিকোয়েস্ট ধাপে ধাপে কাস্টমাইজ করে সরাসরি বিল্ড করছেন।

    // ৫. Builder Client: Concrete Builder তৈরি করে ডিরেক্ট বা কাস্টমাইজড বিল্ড পরিচালনা করে
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ✅ SOLUTION: Builder প্যাটার্ন দিয়ে Send Money ===\n");

            // সুবিধা ১: কোনো null বা false পাস করতে হচ্ছে না। একদম ক্লিন কোড!
            ISendMoneyBuilder builder = new MobileBankingSendMoneyBuilder();
            var normalTransfer = builder
                                    .SetSender("01711111111")
                                    .SetReceiver("01922222222")
                                    .SetAmount(500)
                                    .Build();
            normalTransfer.Execute();

            // সুবিধা ২: সিরিয়াল মনে রাখার কোনো দরকার নেই, যেটাতে ইচ্ছা আগে-পরে ডেটা সেট করা যায়!
            var promoTransfer = new MobileBankingSendMoneyBuilder()
                                    .SetSender("01711111111")
                                    .SetReceiver("01833333333")
                                    .SetAmount(1000)
                                    .SetReference("Eid Salami")
                                    .MakeChargeFree()
                                    .ApplyPromoCode("EID2026")
                                    .Build();
            promoTransfer.Execute();
        }
    }
}


// ══════════════════════════════════════════
// 🚀 Application Client / Main Program (সমগ্র অ্যাপ্লিকেশনের মেইন রানার)
// ══════════════════════════════════════════
class Program
{
    static void Main()
    {
        // ভায়োলেশন রান
        BuilderPattern.SendMoney.Violation.ViolationRunner.Run();
        
        // সলিউশন রান
        BuilderPattern.SendMoney.Solution.SolutionRunner.Run();
    }
}
