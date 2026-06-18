using System;

namespace BehavioralDesignPattern.TemplateMethod.DatabaseQuery
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // ডেটাবেস কোয়েরি রান করার বেস ক্লাস।
    public abstract class DatabaseQueryRunner
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // কোয়েরি রান করার সিকোয়েন্স: কানেকশন তৈরি -> কোয়েরি এক্সিকিউট -> কানেকশন ক্লোজ।
        public void ExecuteQuery()
        {
            Connect();
            RunCommand();
            Disconnect();
            Console.WriteLine("Query execution complete.\n");
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // SQL এবং NoSQL ডাটাবেসের কানেকশন ও কোয়েরি সম্পূর্ণ ভিন্ন, তাই সব abstract।
        protected abstract void Connect();
        protected abstract void RunCommand();
        protected abstract void Disconnect();
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // SQL Server ডাটাবেসের স্পেসিফিক লজিক
    public class SqlServerQueryRunner : DatabaseQueryRunner
    {
        protected override void Connect()
        {
            Console.WriteLine("[SQL Server] Connecting using SQL Connection String.");
        }

        protected override void RunCommand()
        {
            Console.WriteLine("[SQL Server] Executing 'SELECT * FROM Users' T-SQL query.");
        }

        protected override void Disconnect()
        {
            Console.WriteLine("[SQL Server] Closing SQL Connection.");
        }
    }

    // MongoDB ডাটাবেসের স্পেসিফিক লজিক
    public class MongoDbQueryRunner : DatabaseQueryRunner
    {
        protected override void Connect()
        {
            Console.WriteLine("[MongoDB] Connecting using MongoDB Connection URI.");
        }

        protected override void RunCommand()
        {
            Console.WriteLine("[MongoDB] Executing 'db.users.find()' NoSQL query.");
        }

        protected override void Disconnect()
        {
            Console.WriteLine("[MongoDB] Closing MongoDB Client.");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Database Query Runner) ===\n");

            DatabaseQueryRunner sql = new SqlServerQueryRunner();
            sql.ExecuteQuery();

            DatabaseQueryRunner mongo = new MongoDbQueryRunner();
            mongo.ExecuteQuery();
        }
    }
}
