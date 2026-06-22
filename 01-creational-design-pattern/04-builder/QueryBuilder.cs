using System;
using System.Collections.Generic;

// ==============================================================
// ❌ VIOLATION: The Bad Way (স্ট্রিং কনক্যাটেনেশন দিয়ে SQL লেখা)
// ==============================================================
namespace BuilderPattern.SqlQuery.Violation
{
    public class ReportService
    {
        public void GenerateReport(bool onlyActiveUsers, bool sortByAge)
        {
            Console.WriteLine(">> [Violation] স্ট্রিং জোড়া দিয়ে কুয়েরি বানানো হচ্ছে...");

            // ❌ সমস্যা: স্পেস দিতে ভুলে যাওয়া, কমা মিস করা, খুব সহজেই সিনট্যাক্স এরর হতে পারে।
            string sql = "SELECT Id, Name, Email FROM Users ";

            if (onlyActiveUsers)
            {
                sql += "WHERE Status = 'Active' "; // স্পেস না দিলে এরর খাবে
            }

            if (sortByAge)
            {
                sql += "ORDER BY Age DESC";
            }

            Console.WriteLine($"[Generated SQL] {sql}\n");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: Raw SQL Concatenation ===");
            var report = new ReportService();
            report.GenerateReport(onlyActiveUsers: true, sortByAge: true);
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (Using Builder Pattern)
// ==============================================================
namespace BuilderPattern.SqlQuery.Solution
{
    // 🧱 বিল্ডার প্যাটার্নের ৫টি মূল কম্পোনেন্ট:

    // ১. Product: যে জটিল অবজেক্টটি আমরা তৈরি করতে চাচ্ছি (The complex object being built)
    // ==============================================================
    public class SqlQuery
    {
        public string SelectClause { get; set; } = "*";
        public string FromClause { get; set; } = string.Empty;
        public List<string> WhereConditions { get; set; } = new List<string>();
        public string OrderByClause { get; set; } = string.Empty;
        public int? LimitCount { get; set; }

        public override string ToString()
        {
            string query = $"SELECT {SelectClause} FROM {FromClause}";

            if (WhereConditions.Count > 0)
            {
                query += $" WHERE {string.Join(" AND ", WhereConditions)}";
            }

            if (!string.IsNullOrEmpty(OrderByClause))
            {
                query += $" ORDER BY {OrderByClause}";
            }

            if (LimitCount.HasValue)
            {
                query += $" LIMIT {LimitCount.Value}";
            }

            return query + ";";
        }
    }

    // ২. Builder Interface: অবজেক্ট তৈরির ধাপগুলো নির্ধারণ করে (Construction Steps Contract)
    // ==============================================================
    public interface ISqlQueryBuilder
    {
        ISqlQueryBuilder Select(params string[] columns);
        ISqlQueryBuilder From(string table);
        ISqlQueryBuilder Where(string condition);
        ISqlQueryBuilder OrderBy(string column, bool ascending = true);
        ISqlQueryBuilder Limit(int count);
        string Build();
    }

    // ৩. Concrete Builder: Builder Interface ইমপ্লিমেন্ট করে ধাপে ধাপে প্রোডাক্ট তৈরি করে
    // ==============================================================
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private SqlQuery _query = new SqlQuery();

        // 🌟 Fluent Interface: প্রতিটি মেথড 'this' রিটার্ন করে
        public ISqlQueryBuilder Select(params string[] columns)
        {
            _query.SelectClause = string.Join(", ", columns);
            return this;
        }

        public ISqlQueryBuilder From(string table)
        {
            _query.FromClause = table;
            return this;
        }

        public ISqlQueryBuilder Where(string condition)
        {
            _query.WhereConditions.Add(condition);
            return this;
        }

        public ISqlQueryBuilder OrderBy(string column, bool ascending = true)
        {
            string direction = ascending ? "ASC" : "DESC";
            _query.OrderByClause = $"{column} {direction}";
            return this;
        }

        public ISqlQueryBuilder Limit(int count)
        {
            _query.LimitCount = count;
            return this;
        }

        public string Build()
        {
            string result = _query.ToString();
            // নতুন কুয়েরি বানানোর জন্য রিসেট করে দিলাম
            _query = new SqlQuery();
            return result;
        }
    }

    // ৪. Director: নির্দিষ্ট সিকোয়েন্স বা ধাপে অবজেক্ট বিল্ড করার কাজটি পরিচালনা করে (যেমন- ReportService)
    // ==============================================================
    public class ReportService
    {
        private readonly ISqlQueryBuilder _builder;

        public ReportService(ISqlQueryBuilder builder)
        {
            _builder = builder;
        }

        public void GenerateReport(bool onlyActiveUsers, bool sortByAge)
        {
            Console.WriteLine(">> [Solution] Fluent Builder ব্যবহার করে কুয়েরি বানানো হচ্ছে...");

            // 🌟 ম্যাজিক: Fluent API এর মতো চেইন করে কল করা যাচ্ছে (যেমন ORM এ থাকে)
            _builder.Select("Id", "Name", "Email")
                    .From("Users");

            // কন্ডিশনাল লজিক অ্যাড করা খুবই সহজ এবং নিরাপদ
            if (onlyActiveUsers)
            {
                _builder.Where("Status = 'Active'");
            }

            if (sortByAge)
            {
                _builder.OrderBy("Age", ascending: false);
            }

            // ফাইনালি বিল্ড করা
            string finalSql = _builder.Build();
            Console.WriteLine($"[Generated SQL] {finalSql}\n");
        }
    }

    // ৫. Builder Client: Concrete Builder অবজেক্ট তৈরি করে Director-এর কাছে পাঠায় (বা নিজে সরাসরি কুয়েরি বিল্ড করে)
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ✅ SOLUTION RUN: Fluent Query Builder Pattern ===");
            
            ISqlQueryBuilder builder = new SqlQueryBuilder();
            var reportService = new ReportService(builder);

            // ১. ফুল রিপোর্ট
            Console.WriteLine("--- Report 1: Active Users Sorted by Age ---");
            reportService.GenerateReport(onlyActiveUsers: true, sortByAge: true);

            // ২. সিম্পল রিপোর্ট
            Console.WriteLine("--- Report 2: All Users without Sorting ---");
            reportService.GenerateReport(onlyActiveUsers: false, sortByAge: false);

            // ৩. ইনলাইন বিল্ড করা (Director ছাড়াই)
            Console.WriteLine("--- Inline Builder Usage ---");
            var customQuery = new SqlQueryBuilder()
                                .Select("ProductName", "Price")
                                .From("Products")
                                .Where("Price > 500")
                                .Where("Stock > 0")
                                .Limit(10)
                                .Build();
            
            Console.WriteLine($"[Custom Query] {customQuery}\n");
        }
    }
}

// 💻 Application Client / Main Program: সমগ্র অ্যাপ্লিকেশনের রানার বা এন্ট্রি পয়েন্ট
class Program
{
    static void Main()
    {
        BuilderPattern.SqlQuery.Violation.ViolationRunner.Run();
        BuilderPattern.SqlQuery.Solution.SolutionRunner.Run();
    }
}
