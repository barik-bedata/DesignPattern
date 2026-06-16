using System;

// ==============================================================
// ✅ SOLUTION: The Good Way (Using Builder Pattern)
// ==============================================================
// সমাধান: "Send Money" রিকোয়েস্ট তৈরি করার কাজটা Builder এর হাতে দেওয়া।
// এতে শুধু যে প্যারামিটারগুলো দরকার, সেগুলোই চেইন (Method Chaining) করে সেট করা যায়। 
// অপ্রয়োজনীয় null বা false পাস করতে হয় না।

namespace BuilderPattern.SendMoney.Solution
{
    // ১. Product
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

    // ২. Builder Interface
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

    // ৩. Concrete Builder (bKash/Nagad Builder)
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
            // ভ্যালিডেশন: বিল্ড করার আগে চেক করা যায় সব ঠিক আছে কি না!
            if (string.IsNullOrEmpty(_request.SenderNumber) || string.IsNullOrEmpty(_request.ReceiverNumber))
            {
                throw new InvalidOperationException("সেন্ডার এবং রিসিভার নাম্বার অবশ্যই দিতে হবে!");
            }
            
            return _request;
        }
    }

    // Client
    class Program
    {
        static void Main()
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
