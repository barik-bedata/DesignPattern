using System;

namespace AdapterPattern.RealWorldExamples
{
    // ১. Target Interface (আমাদের ওয়েবসাইটের বর্তমান পেমেন্ট সিস্টেম)
    public interface IPaymentProcessor
    {
        void Pay(double amount);
    }

    // আমাদের ওয়েবসাইটের ডিফল্ট পেমেন্ট মেথড (ক্রেডিট কার্ড)
    public class CreditCardPayment : IPaymentProcessor
    {
        public void Pay(double amount)
        {
            Console.WriteLine($"Paid {amount} Taka using Credit Card.");
        }
    }

    // ২. Adaptee (থার্ড-পার্টি API, যার মেথড সম্পূর্ণ আলাদা)
    public class BkashApi
    {
        public void SendMoneyViaBkash(string phoneNumber, double amount)
        {
            Console.WriteLine($"Successfully sent {amount} Taka via bKash to {phoneNumber}.");
        }
    }

    // ৩. Adapter (যাতে আমাদের সিস্টেম ক্রেডিট কার্ডের মতোই বিকাশ ব্যবহার করতে পারে)
    public class BkashAdapter : IPaymentProcessor
    {
        private BkashApi _bkashApi;
        private string _userPhoneNumber;

        public BkashAdapter(BkashApi bkashApi, string userPhoneNumber)
        {
            _bkashApi = bkashApi;
            _userPhoneNumber = userPhoneNumber;
        }

        // আমাদের ওয়েবসাইটের Pay মেথডকেই ইমপ্লিমেন্ট করা হলো
        public void Pay(double amount)
        {
            // ভেতরে আমরা বিকাশের API কল করে দিচ্ছি
            _bkashApi.SendMoneyViaBkash(_userPhoneNumber, amount);
        }
    }

    // Client Code
    public class EcommerceSite
    {
        public static void Main()
        {
            Console.WriteLine("--- E-commerce Checkout ---");

            // ক্রেডিট কার্ড দিয়ে পেমেন্ট
            IPaymentProcessor cardPayment = new CreditCardPayment();
            cardPayment.Pay(500.00);

            // বিকাশ দিয়ে পেমেন্ট (Adapter এর মাধ্যমে)
            BkashApi bkash = new BkashApi();
            IPaymentProcessor bkashPayment = new BkashAdapter(bkash, "017XXXXXXXX");
            bkashPayment.Pay(1200.50);
        }
    }
}
