using System;
using System.Threading;

namespace PrototypePattern.PrivateFieldExample
{
    // ==============================================================
    // ১. Prototype Interface (প্রোটোটাইপ ইন্টারফেস)
    // ==============================================================
    // এই ইন্টারফেসে আমরা বলে দিচ্ছি যে ডেটাবেস কানেকশন ক্লোন করা যাবে, 
    // এবং ক্লোন করার সময় নতুন প্রোভাইডার ও কানেকশন স্ট্রিং পাস করা যাবে।
    public interface IDatabaseConnection
    {
        IDatabaseConnection CloneWithNewProvider(string newProvider, string newConnectionString);
        void Connect();
    }

    // ==============================================================
    // ২. Concrete Prototype (কংক্রিট প্রোটোটাইপ)
    // ==============================================================
    // এটি আসল ডেটাবেস কানেকশন ক্লাস যা বানাতে অনেক সময় লাগে, 
    // কিন্তু ক্লোন করার লজিক (Custom Clone) यहीं (এখানেই) লেখা থাকে।
    public class DatabaseConnection : IDatabaseConnection
    {
        // Public properties (ভারী কনফিগারেশন যেগুলো তৈরি করতে সময় লাগে)
        public int MaxPoolSize { get; set; }
        public int TimeoutSeconds { get; set; }
        public string SecurityProtocol { get; set; }

        // Private fields (যেগুলো আমরা ক্লোন করার সময় বাহির থেকে পরিবর্তন করবো)
        private string _dbProvider;
        private string _connectionString;

        public DatabaseConnection(string dbProvider, string connectionString)
        {
            Console.WriteLine($"   -> [Heavy Task] Loading core network configs, parsing security certificates for {dbProvider}...");
            Thread.Sleep(1500); // 1.5s delay to simulate setup
            
            MaxPoolSize = 100;
            TimeoutSeconds = 30;
            SecurityProtocol = "TLS 1.2";
            
            _dbProvider = dbProvider;
            _connectionString = connectionString;
        }

        // এখানেই আসল কপি এবং প্রাইভেট ডেটা আপডেটের কাজ হচ্ছে
        public IDatabaseConnection CloneWithNewProvider(string newProvider, string newConnectionString)
        {
            Console.WriteLine($"   -> [Cloning] Quick Copying heavy configs and switching private provider to {newProvider}...");
            
            // শ্যালো কপি করে ভারী কনফিগারেশনগুলো (MaxPoolSize, Timeout, Security) দ্রুত কপি করে নিলাম
            DatabaseConnection copy = (DatabaseConnection)this.MemberwiseClone();
            
            // এরপর প্রাইভেট ফিল্ডগুলোতে নতুন ডেটা বসিয়ে দিলাম
            copy._dbProvider = newProvider;
            copy._connectionString = newConnectionString;

            return copy;
        }

        public void Connect()
        {
            Console.WriteLine($"[Connected] {_dbProvider} Database connected using: '{_connectionString}'");
            Console.WriteLine($"          (Pool: {MaxPoolSize}, Timeout: {TimeoutSeconds}s, Security: {SecurityProtocol})\n");
        }
    }

    // ==============================================================
    // ৩. Client (ক্লায়েন্ট)
    // ==============================================================
    // ক্লায়েন্ট (যেমন DatabaseManager) শুধু জানে তার কাছে একটি মাস্টার কানেকশন আছে। 
    // নতুন কানেকশন লাগলে সে সরাসরি 'new' কল না করে, মাস্টারকে বলে ক্লোন করে দিতে।
    public class DatabaseManager
    {
        private IDatabaseConnection _masterConnection;

        // ম্যানেজারের কাছে মাস্টার কপিটি সেভ রাখা হলো
        public DatabaseManager(IDatabaseConnection masterConnection)
        {
            _masterConnection = masterConnection;
        }

        // ক্লায়েন্ট নতুন ডেটাবেসের জন্য ক্লোন চাইছে (সে জানে না ভেতরে কীভাবে কপি হচ্ছে)
        public IDatabaseConnection GetConnectionFor(string provider, string connectionString)
        {
            return _masterConnection.CloneWithNewProvider(provider, connectionString);
        }
    }

    // ==============================================================
    // ৪. Main Class (মেইন ক্লাস)
    // ==============================================================
    // এখান থেকে প্রোগ্রাম রান হচ্ছে এবং ক্লায়েন্টের মাধ্যমে বিভিন্ন ডেটাবেস কানেক্ট করা হচ্ছে।
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Prototype Pattern Components: DB Connection Demo ===\n");

            // ধাপ ১: মাস্টার কানেকশন তৈরি (MySQL এর জন্য, এতে সময় লাগবে)
            Console.WriteLine("--- 1. Creating Master Connection ---");
            DatabaseConnection masterMySql = new DatabaseConnection("MySQL", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;");
            
            // ধাপ ২: ক্লায়েন্ট (ম্যানেজার) এর কাছে মাস্টার কপিটি দিয়ে দিলাম
            DatabaseManager dbManager = new DatabaseManager(masterMySql);
            masterMySql.Connect();

            // ধাপ ৩: ক্লায়েন্টকে ব্যবহার করে নতুন কানেকশন (PostgreSQL) তৈরি (কোনো সময় লাগবে না)
            Console.WriteLine("--- 2. Asking Client for PostgreSQL Connection ---");
            IDatabaseConnection postgresConnection = dbManager.GetConnectionFor(
                "PostgreSQL", 
                "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;"
            );
            postgresConnection.Connect();

            // ধাপ ৪: ক্লায়েন্টকে ব্যবহার করে নতুন কানেকশন (Oracle) তৈরি
            Console.WriteLine("--- 3. Asking Client for Oracle Connection ---");
            IDatabaseConnection oracleConnection = dbManager.GetConnectionFor(
                "Oracle", 
                "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;"
            );
            oracleConnection.Connect();
        }
    }
}
