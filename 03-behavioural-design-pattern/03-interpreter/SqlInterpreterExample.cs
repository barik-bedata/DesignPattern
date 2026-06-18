using System;
using System.Collections.Generic;
using System.Linq;

namespace BehavioralDesignPattern.Interpreter.SQL
{
    // ==========================================
    // 4. Context (Database Row Data)
    // ==========================================
    public interface IRowContext
    {
        int GetIntValue(string columnName);
    }

    public class UserRow : IRowContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int GetIntValue(string columnName)
        {
            return columnName switch
            {
                "Age" => Age,
                "Id" => Id,
                _ => throw new Exception($"Column {columnName} not found")
            };
        }
    }

    // ==========================================
    // 1. AbstractExpression
    // ==========================================
    public interface ISqlWhereExpression
    {
        bool Interpret(IRowContext rowContext);
    }

    // ==========================================
    // 2. TerminalExpression (যেমন: Age > 18)
    // ==========================================
    public class GreaterThanExpression : ISqlWhereExpression
    {
        private readonly string _column;
        private readonly int _value;

        public GreaterThanExpression(string column, int value)
        {
            _column = column;
            _value = value;
        }

        public bool Interpret(IRowContext rowContext)
        {
            // ডাটাবেজের নির্দিষ্ট কলামের ডেটা কি ভ্যালু থেকে বড়?
            return rowContext.GetIntValue(_column) > _value;
        }
    }

    // ==========================================
    // 6. Interpreter (SQL Parser)
    // ==========================================
    public interface ISqlInterpreter
    {
        ISqlWhereExpression ParseWhereClause(string clause);
    }

    public class SqlParser : ISqlInterpreter
    {
        public ISqlWhereExpression ParseWhereClause(string clause)
        {
            // সিম্পল পার্সার: "Age > 18"
            var parts = clause.Split('>');
            string column = parts[0].Trim();
            int value = int.Parse(parts[1].Trim());

            return new GreaterThanExpression(column, value);
        }
    }

    // ==========================================
    // 5. Client (Database Engine)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== SQL WHERE Clause Interpreter ===");

            // ১. ডাটাবেজ টেবিলের কিছু ডামি ডেটা (List of Contexts)
            List<IRowContext> usersTable = new List<IRowContext>
            {
                new UserRow { Id = 1, Name = "Alice", Age = 15 },
                new UserRow { Id = 2, Name = "Bedata", Age = 25 },
                new UserRow { Id = 3, Name = "Charlie", Age = 30 }
            };

            // ২. এসকিউএল পার্সার তৈরি
            ISqlInterpreter parser = new SqlParser();

            // ৩. ইউজারের কোয়েরি
            string sqlWhere = "Age > 18";
            Console.WriteLine($"SQL Executing: SELECT * FROM Users WHERE {sqlWhere}\n");

            // ৪. স্ট্রিং পার্স করে AST বানানো হলো
            ISqlWhereExpression whereAst = parser.ParseWhereClause(sqlWhere);

            // ৫. টেবিলের প্রতিটি রো (Row) এর ওপর AST এক্সিকিউট করে ফিল্টার করা হচ্ছে
            var adultUsers = usersTable.Where(row => whereAst.Interpret(row)).Cast<UserRow>().ToList();

            Console.WriteLine($"Found {adultUsers.Count} matching users:");
            foreach (var user in adultUsers)
            {
                Console.WriteLine($"- {user.Name} (Age: {user.Age})");
            }
        }
    }
}
