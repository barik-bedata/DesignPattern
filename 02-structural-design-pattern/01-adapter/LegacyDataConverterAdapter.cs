using System;

namespace AdapterPattern.RealWorldExamples
{
    // ১. Target Interface (আমাদের পুরনো ব্যাংকের সিস্টেম যা শুধু XML সাপোর্ট করে)
    public interface IBankLegacySystem
    {
        void ProcessData(string xmlData);
    }

    public class OldBankSoftware : IBankLegacySystem
    {
        public void ProcessData(string xmlData)
        {
            Console.WriteLine("Processing XML Data in Legacy Bank System: " + xmlData);
        }
    }

    // ২. Adaptee (ঢাকা স্টক এক্সচেঞ্জের নতুন API যা JSON দেয়)
    public class DseStockApi
    {
        public string GetStockDataInJson()
        {
            return "{ \"company\": \"GP\", \"price\": 350.5 }";
        }
    }

    // ৩. Adapter (যাতে JSON ডেটাকে XML এ কনভার্ট করে ব্যাংকে পাঠানো যায়)
    public class JsonToXmlAdapter : IBankLegacySystem
    {
        private DseStockApi _stockApi;

        public JsonToXmlAdapter(DseStockApi stockApi)
        {
            _stockApi = stockApi;
        }

        // ব্যাংকের চেনা মেথড
        public void ProcessData(string ignoredData)
        {
            // ১. স্টক এক্সচেঞ্জ থেকে JSON নেওয়া
            string jsonData = _stockApi.GetStockDataInJson();
            Console.WriteLine("Adapter: Got JSON from DSE API -> " + jsonData);

            // ২. JSON কে XML এ রূপান্তর করা (উদাহরণস্বরূপ)
            string xmlData = $"<Stock><Company>GP</Company><Price>350.5</Price></Stock>";
            Console.WriteLine("Adapter: Converted JSON to XML -> " + xmlData);

            // ৩. ব্যাংকের সিস্টেমে XML পাঠানো
            Console.WriteLine("Adapter: Sending XML to the Bank System...");
            
            // ব্যাংকের নিজস্ব প্রসেসিং লজিক এখানে কল হবে (সিমুলেশন)
            Console.WriteLine("Processing XML Data in Legacy Bank System: " + xmlData);
        }
    }

    // Client Code
    public class BankDashboard
    {
        public static void Main()
        {
            Console.WriteLine("--- Legacy Bank System updating Stock Data ---");

            // নতুন API
            DseStockApi modernApi = new DseStockApi();

            // Adapter তৈরি করে ব্যাংকের সিস্টেমে দেওয়া হলো
            IBankLegacySystem adapter = new JsonToXmlAdapter(modernApi);
            
            // ব্যাংকের সিস্টেম তার চেনা ProcessData কল করছে
            adapter.ProcessData("dummy");
        }
    }
}
