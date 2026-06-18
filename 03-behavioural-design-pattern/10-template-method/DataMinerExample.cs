using System;

namespace BehavioralDesignPattern.TemplateMethod.DataMiner
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // এটি হলো বেস ক্লাস যা ডেটা মাইনিংয়ের মূল স্ট্রাকচার (Skeleton) ডিফাইন করে।
    public abstract class DataMiner
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // এই মেথডটি কাজের সিকোয়েন্স লক করে দেয়। প্রথমে ফাইল ওপেন হবে, তারপর এক্সট্রাক্ট, পার্স, সেভ এবং সবশেষে ক্লোজ।
        public void MineData(string path)
        {
            OpenFile(path);
            ExtractData();
            ParseData();
            SaveToDatabase();
            CloseFile();
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // ফাইল টাইপের ওপর ভিত্তি করে এক্সট্রাক্ট এবং পার্স করার লজিক ভিন্ন হবে। তাই এগুলো abstract রাখা হয়েছে।
        protected abstract void ExtractData();
        protected abstract void ParseData();

        // Common methods (সব ফাইলের জন্যই ফাইল ওপেন ও ক্লোজ করার সিস্টেম একই)
        private void OpenFile(string path)
        {
            Console.WriteLine($"[Common] Opening file: {path}");
        }

        private void SaveToDatabase()
        {
            Console.WriteLine("[Common] Saving parsed data to Database.");
        }

        private void CloseFile()
        {
            Console.WriteLine("[Common] Closing file.\n");
        }
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // PDF থেকে ডেটা পড়ার নির্দিষ্ট লজিক এখানে লেখা হয়।
    public class PdfDataMiner : DataMiner
    {
        protected override void ExtractData()
        {
            Console.WriteLine("[PDF Miner] Extracting raw text from PDF document.");
        }

        protected override void ParseData()
        {
            Console.WriteLine("[PDF Miner] Parsing PDF text to structured format.");
        }
    }

    // CSV থেকে ডেটা পড়ার নির্দিষ্ট লজিক এখানে লেখা হয়।
    public class CsvDataMiner : DataMiner
    {
        protected override void ExtractData()
        {
            Console.WriteLine("[CSV Miner] Extracting comma-separated values.");
        }

        protected override void ParseData()
        {
            Console.WriteLine("[CSV Miner] Parsing CSV rows into data models.");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    // ক্লায়েন্ট শুধু নির্দিষ্ট Miner এর অবজেক্ট বানিয়ে Template Method কল করবে।
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Data Miner) ===\n");

            DataMiner pdfMiner = new PdfDataMiner();
            pdfMiner.MineData("report.pdf");

            DataMiner csvMiner = new CsvDataMiner();
            csvMiner.MineData("users.csv");
        }
    }
}
