using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Interpreter.JQL
{
    // ==========================================
    // 4. Context (Jira Issue Data)
    // ==========================================
    public interface IIssueContext
    {
        string GetFieldValue(string fieldName);
    }

    public class IssueContext : IIssueContext
    {
        private readonly Dictionary<string, string> _fields = new Dictionary<string, string>();

        public void SetField(string name, string value) => _fields[name] = value;
        
        public string GetFieldValue(string fieldName) 
            => _fields.TryGetValue(fieldName, out var val) ? val : "";
    }

    // ==========================================
    // 1. AbstractExpression
    // ==========================================
    public interface IJqlExpression
    {
        bool Interpret(IIssueContext context);
    }

    // ==========================================
    // 2. TerminalExpression (যেমন: Status = Done)
    // ==========================================
    public class EqualsExpression : IJqlExpression
    {
        private readonly string _field;
        private readonly string _value;

        public EqualsExpression(string field, string value)
        {
            _field = field;
            _value = value;
        }

        public bool Interpret(IIssueContext context)
        {
            // ডাটাবেজের ফিল্ডের সাথে ইউজারের দেওয়া ভ্যালু মিলছে কি না চেক করা হচ্ছে
            return context.GetFieldValue(_field).Equals(_value, StringComparison.OrdinalIgnoreCase);
        }
    }

    // ==========================================
    // 3. NonterminalExpression (যেমন: AND)
    // ==========================================
    public class AndExpression : IJqlExpression
    {
        private readonly IJqlExpression _left;
        private readonly IJqlExpression _right;

        public AndExpression(IJqlExpression left, IJqlExpression right)
        {
            _left = left;
            _right = right;
        }

        public bool Interpret(IIssueContext context)
        {
            // বাম পাশ এবং ডান পাশ দুটোই True হলে পুরো রুলস True হবে
            return _left.Interpret(context) && _right.Interpret(context);
        }
    }

    // ==========================================
    // 6. Interpreter (JQL Parser)
    // ==========================================
    public interface IJqlInterpreter
    {
        IJqlExpression Parse(string query);
    }

    public class JqlParser : IJqlInterpreter
    {
        public IJqlExpression Parse(string query)
        {
            // সিম্পল পার্সার: "Assignee=Bedata AND Status=Done"
            string[] tokens = query.Split(new[] { " AND " }, StringSplitOptions.None);
            
            IJqlExpression root = ParseEquals(tokens[0]);

            for (int i = 1; i < tokens.Length; i++)
            {
                root = new AndExpression(root, ParseEquals(tokens[i]));
            }

            return root;
        }

        private IJqlExpression ParseEquals(string token)
        {
            var parts = token.Split('=');
            return new EqualsExpression(parts[0].Trim(), parts[1].Trim());
        }
    }

    // ==========================================
    // 5. Client (Jira Search Bar)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== JQL Interpreter Example ===");

            // ১. একটি ডামি জিরা ইস্যু (Context) তৈরি করা হলো
            IssueContext issue = new IssueContext();
            issue.SetField("Assignee", "Bedata");
            issue.SetField("Status", "Done");

            // ২. জিরা পার্সার তৈরি
            IJqlInterpreter parser = new JqlParser();

            // ৩. ইউজারের দেওয়া JQL স্ট্রিং
            string jqlQuery = "Assignee=Bedata AND Status=Done";
            
            // ৪. স্ট্রিং পার্স করে AST বানানো হলো
            IJqlExpression ast = parser.Parse(jqlQuery);

            // ৫. এই নির্দিষ্ট ইস্যুটি কোয়েরির সাথে ম্যাচ করে কি না চেক করা হচ্ছে
            bool isMatch = ast.Interpret(issue);
            
            Console.WriteLine($"Query: {jqlQuery}");
            Console.WriteLine($"Is this issue a match? : {isMatch}");
        }
    }
}
