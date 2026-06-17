using System;
using System.Threading;

namespace PrototypePattern.PrivateFieldExample
{
    public class DatabaseConnection
    {
        // Public properties that take heavy time to parse/configure (e.g. from XML/JSON)
        public int MaxPoolSize { get; set; }
        public int TimeoutSeconds { get; set; }
        public string SecurityProtocol { get; set; }

        // Private fields that we want to change during clone
        private string _dbProvider;
        private string _connectionString;

        public DatabaseConnection(string dbProvider, string connectionString)
        {
            Console.WriteLine($"   -> [Heavy Task] Loading core network configs, parsing security certificates for {_dbProvider}...");
            Thread.Sleep(1500); // 1.5s delay to simulate heavy setup (XML parsing, File I/O)
            
            // Common Heavy Configs
            MaxPoolSize = 100;
            TimeoutSeconds = 30;
            SecurityProtocol = "TLS 1.2";
            
            _dbProvider = dbProvider;
            _connectionString = connectionString;
        }

        // ==============================================================
        // কাস্টম ক্লোন মেথড (প্রাইভেট ডেটা চেঞ্জ করার স্মার্ট উপায়)
        // ==============================================================
        // ICloneable এর ডিফল্ট Clone() মেথডে প্যারামিটার পাঠানো যায় না, 
        // তাই আমরা একটি কাস্টম ক্লোন মেথড বানালাম যা বাহির থেকে নতুন প্রাইভেট ডেটা রিসিভ করবে।
        public DatabaseConnection CloneWithNewProvider(string newProvider, string newConnectionString)
        {
            Console.WriteLine($"   -> [Cloning] Quick Copying heavy configs and switching private provider to {newProvider}...");
            
            // শ্যালো কপি করে ভারী কনফিগারেশনগুলো (MaxPoolSize, Timeout, Security) দ্রুত কপি করে নিলাম
            DatabaseConnection copy = (DatabaseConnection)this.MemberwiseClone();
            
            // এরপর প্রাইভেট ফিল্ডগুলোতে নতুন ডেটা বসিয়ে দিলাম (যেহেতু মেথডটি ক্লাসের ভেতরে, তাই প্রাইভেট ফিল্ড অ্যাক্সেস করা যায়)
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

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Prototype Pattern: Private Field Modification Demo ===\n");

            // ধাপ ১: আমরা প্রথমে MySQL এর জন্য একটি কানেকশন তৈরি করলাম। এটি বানাতে অনেক সময় লাগবে।
            Console.WriteLine("--- 1. Creating Master Connection (MySQL) ---");
            DatabaseConnection mysqlConnection = new DatabaseConnection("MySQL", "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;");
            mysqlConnection.Connect();

            // ধাপ ২: এখন আমাদের PostgreSQL কানেকশন লাগবে। নতুন করে new দিয়ে বানালে আবার সময় নষ্ট হবে।
            // তাই আমরা আগের অবজেক্টটাকেই ক্লোন করবো এবং ক্লোন করার সময় প্রাইভেট ফিল্ডে PostgreSQL এর ডেটা পাঠিয়ে দেবো।
            Console.WriteLine("--- 2. Cloning to create PostgreSQL Connection ---");
            DatabaseConnection postgresConnection = mysqlConnection.CloneWithNewProvider(
                "PostgreSQL", 
                "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;"
            );
            
            postgresConnection.Connect();

            // ধাপ ৩: আরেকটি Oracle কানেকশন ক্লোন করি
            Console.WriteLine("--- 3. Cloning to create Oracle Connection ---");
            DatabaseConnection oracleConnection = mysqlConnection.CloneWithNewProvider(
                "Oracle", 
                "Data Source=MyOracleDB;User Id=myUsername;Password=myPassword;"
            );
            
            oracleConnection.Connect();
        }
    }
}
