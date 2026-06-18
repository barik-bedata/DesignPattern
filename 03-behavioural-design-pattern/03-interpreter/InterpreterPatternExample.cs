using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Interpreter
{
    // ==========================================
    // 1. AbstractExpression (কমন নিয়ম)
    // ==========================================
    // এটি সবার জন্য একটি কমন কন্ট্রাক্ট তৈরি করে।
    public interface IExpression
    {
        int Interpret(IContext context);
    }

    // ==========================================
    // 2. TerminalExpression (শেষ মাথা / বেসিক ইলিমেন্ট)
    // ==========================================
    // এটি হলো সংখ্যা বা ভেরিয়েবল (যেমন: 5, X, Y), যাকে আর ছোট করা যায় না।
    public class NumberExpression : IExpression
    {
        private readonly string _nameOrNumber;

        public NumberExpression(string nameOrNumber)
        {
            _nameOrNumber = nameOrNumber;
        }

        public int Interpret(IContext context)
        {
            // যদি এটি সরাসরি কোনো সংখ্যা হয় (যেমন "5"), তবে সেটিই রিটার্ন করবে
            if (int.TryParse(_nameOrNumber, out int number))
            {
                return number;
            }
            
            // আর যদি এটি কোনো ভেরিয়েবল হয় (যেমন "X"), তবে Context থেকে তার মান খুঁজে আনবে
            return context.GetVariable(_nameOrNumber);
        }
    }

    // ==========================================
    // 3. NonterminalExpression (যৌগিক বা অপারেটর)
    // ==========================================
    // এটি হলো অপারেটর (যেমন: + বা -)। এরা একা কাজ করতে পারে না, ডান-বামে দুটি Terminal লাগে।
    
    // যোগ করার এক্সপ্রেশন
    public class AddExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public AddExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(IContext context)
        {
            return _left.Interpret(context) + _right.Interpret(context);
        }
    }

    // বিয়োগ করার এক্সপ্রেশন
    public class SubtractExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public SubtractExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(IContext context)
        {
            return _left.Interpret(context) - _right.Interpret(context);
        }
    }

    // ==========================================
    // 4. Context (গ্লোবাল ডেটা / ভেরিয়েবল স্টোর)
    // ==========================================
    // এখানে আমরা গ্লোবাল ভ্যালুগুলো স্টোর করে রাখি।
    public interface IContext
    {
        void SetVariable(string variable, int value);
        int GetVariable(string variable);
    }

    public class Context : IContext
    {
        private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();

        public void SetVariable(string variable, int value)
        {
            _variables[variable] = value;
        }

        public int GetVariable(string variable)
        {
            if (_variables.ContainsKey(variable))
                return _variables[variable];
            
            throw new Exception($"Variable '{variable}' not found in context!");
        }
    }

    // ==========================================
    // 6. Interpreter (Manager / Parser)
    // ==========================================
    // এটিই হলো সেই ৬ষ্ঠ কম্পোনেন্ট! এর কাজ হলো একটি স্ট্রিং (যেমন: "X + Y - 2") 
    // ইনপুট নিয়ে তাকে ভেঙে একটি Abstract Syntax Tree (AST) তৈরি করা। 
    // যদি এই ক্লাসটা না থাকতো, তবে ক্লায়েন্টকে নিজের হাতে এই গাছ বানাতে হতো!
    public interface IInterpreter
    {
        IExpression Parse(string input);
    }

    public class MathInterpreter : IInterpreter
    {
        public IExpression Parse(string input)
        {
            string[] tokens = input.Split(' '); // স্ট্রিংটিকে স্পেস দিয়ে ভেঙে ফেলা হলো

            // প্রথম টোকেনটিকে ট্রি (Tree)-এর শুরু হিসেবে ধরে নিলাম
            IExpression syntaxTree = new NumberExpression(tokens[0]);

            // এরপর লুপ চালিয়ে অপারেটর (+, -) এবং ডানদিকের সংখ্যাটি বের করে ট্রি বড় করছি
            for (int i = 1; i < tokens.Length; i += 2)
            {
                string operatorToken = tokens[i];
                IExpression rightNode = new NumberExpression(tokens[i + 1]);

                if (operatorToken == "+")
                    syntaxTree = new AddExpression(syntaxTree, rightNode);
                else if (operatorToken == "-")
                    syntaxTree = new SubtractExpression(syntaxTree, rightNode);
            }

            return syntaxTree; // সম্পূর্ণ ট্রি (AST) রিটার্ন করা হলো
        }
    }

    // ==========================================
    // 5. Client (ইউজার)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Interpreter Pattern (6 Components) ===\n");

            // ১. Context তৈরি করা হলো (ভেরিয়েবলের মান সেট করা)
            IContext context = new Context();
            context.SetVariable("Salary", 50000);
            context.SetVariable("Bonus", 10000);
            context.SetVariable("Tax", 5000);

            // ২. Interpreter (Parser) তৈরি করা হলো
            IInterpreter interpreter = new MathInterpreter();

            // ৩. ক্লায়েন্ট একটি সিম্পল স্ট্রিং ইনপুট দিলো
            string expressionStr = "Salary + Bonus - Tax + 500";
            Console.WriteLine($"Evaluating Expression: {expressionStr}");

            // ৪. Interpreter স্ট্রিংটিকে পার্স করে Syntax Tree (AST) বানিয়ে ফেললো
            IExpression astRoot = interpreter.Parse(expressionStr);

            // ৫. সবশেষে ট্রি-এর রুট ধরে Interpret কল করা হলো
            int result = astRoot.Interpret(context);

            Console.WriteLine($"\nFinal Result: {result}");
        }
    }
}
