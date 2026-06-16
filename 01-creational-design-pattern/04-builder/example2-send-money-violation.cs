using System;

// ==============================================================
// ❌ VIOLATION: The Bad Way (Telescoping Constructor Anti-pattern)
// ==============================================================
// সমস্যা: একটি "Send Money" রিকোয়েস্ট তৈরি করতে অনেকগুলো প্যারামিটার লাগে।
// সাধারণ কন্সট্রাক্টর ব্যবহার করলে কোনটা রেফারেন্স, কোনটা প্রোমোকোড আর কোনটা চার্জ-ফ্রি ফ্ল্যাগ, 
// সেটা মনে রাখা অসম্ভব। এর ফলে ভুল অ্যাকাউন্টে বা ভুল অ্যামাউন্ট সেন্ড মানি হয়ে যেতে পারে!

namespace BuilderPattern.SendMoney.Violation
{
    // The God Object
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
            Console.WriteLine($"[Sending Money...] From: {SenderNumber} To: {ReceiverNumber} | Amount: {Amount} TK");
            Console.WriteLine($"   -> Ref: {Reference ?? "N/A"}, Charge Free: {IsChargeFree}, Promo: {PromoCode ?? "N/A"}\n");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ❌ VIOLATION: কন্সট্রাক্টর দিয়ে Send Money ===\n");
            
            // সমস্যা ১: শুধু টাকা পাঠাতে চাই, কিন্তু বাধ্য হয়ে null, false, null পাঠাতে হচ্ছে!
            // যা কোডকে নোংরা করে দেয়।
            var normalTransfer = new SendMoneyRequest("01711111111", "01922222222", 500, null, false, null);
            normalTransfer.Execute();

            // সমস্যা ২: প্যারামিটার সিরিয়াল মনে রাখা কঠিন।
            // ভুলে amount এর জায়গায় string আর reference এর জায়গায় bool দিলে মারাত্মক বাগ তৈরি হবে।
            var promoTransfer = new SendMoneyRequest("01711111111", "01833333333", 1000, "Gift", true, "EID2026");
            promoTransfer.Execute();
        }
    }
}
