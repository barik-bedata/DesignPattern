using System;

namespace BehavioralDesignPattern.Interpreter.CustomRuleEngine
{
    // ==========================================
    // 4. Context (Shopping Cart Data)
    // ==========================================
    public interface ICartContext
    {
        decimal GetTotalAmount();
        bool IsVipCustomer();
    }

    public class CartContext : ICartContext
    {
        private readonly decimal _total;
        private readonly bool _isVip;

        public CartContext(decimal total, bool isVip)
        {
            _total = total;
            _isVip = isVip;
        }

        public decimal GetTotalAmount() => _total;
        public bool IsVipCustomer() => _isVip;
    }

    // ==========================================
    // 1. AbstractExpression
    // ==========================================
    public interface IRuleExpression
    {
        bool Interpret(ICartContext context);
    }

    // ==========================================
    // 2. TerminalExpression (CartTotal > X)
    // ==========================================
    public class AmountGreaterThanRule : IRuleExpression
    {
        private readonly decimal _targetAmount;

        public AmountGreaterThanRule(decimal targetAmount)
        {
            _targetAmount = targetAmount;
        }

        public bool Interpret(ICartContext context)
        {
            return context.GetTotalAmount() > _targetAmount;
        }
    }

    // ==========================================
    // 2. TerminalExpression (IsVip == True/False)
    // ==========================================
    public class VipCustomerRule : IRuleExpression
    {
        private readonly bool _requiredStatus;

        public VipCustomerRule(bool requiredStatus)
        {
            _requiredStatus = requiredStatus;
        }

        public bool Interpret(ICartContext context)
        {
            return context.IsVipCustomer() == _requiredStatus;
        }
    }

    // ==========================================
    // 3. NonterminalExpression (AND Logic)
    // ==========================================
    public class AndRule : IRuleExpression
    {
        private readonly IRuleExpression _left;
        private readonly IRuleExpression _right;

        public AndRule(IRuleExpression left, IRuleExpression right)
        {
            _left = left;
            _right = right;
        }

        public bool Interpret(ICartContext context)
        {
            return _left.Interpret(context) && _right.Interpret(context);
        }
    }

    // ==========================================
    // 6. Interpreter (Rule Engine Parser)
    // ==========================================
    public interface IRuleInterpreter
    {
        IRuleExpression ParseRule(string ruleString);
    }

    public class DiscountRuleParser : IRuleInterpreter
    {
        public IRuleExpression ParseRule(string ruleString)
        {
            // সিম্পল স্ট্রিং: "CartTotal > 5000 AND IsVip == True"
            // (একটি রিয়েল সিস্টেমে এখানে অনেক বড় লজিক থাকে, বোঝার সুবিধার্থে হার্ডকোড করা হলো)
            
            if (ruleString == "CartTotal > 5000 AND IsVip == True")
            {
                IRuleExpression amountRule = new AmountGreaterThanRule(5000);
                IRuleExpression vipRule = new VipCustomerRule(true);
                
                return new AndRule(amountRule, vipRule); // AST Tree তৈরি
            }

            throw new Exception("Rule format not supported in this simple parser.");
        }
    }

    // ==========================================
    // 5. Client (E-Commerce System)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Custom Rule Engine (E-commerce Discount) ===");

            // ১. ইউজারের কার্ট (Context)
            ICartContext cart = new CartContext(total: 6000, isVip: true);

            // ২. অ্যাডমিন প্যানেল থেকে আসা স্ট্রিং রুলস
            string adminRule = "CartTotal > 5000 AND IsVip == True";
            
            // ৩. রুলস পার্সার তৈরি
            IRuleInterpreter parser = new DiscountRuleParser();
            
            // ৪. স্ট্রিং থেকে AST বা লজিক ট্রি বানানো হলো
            IRuleExpression discountAst = parser.ParseRule(adminRule);

            // ৫. ইউজারের কার্টের ওপর লজিক রান করা হচ্ছে
            bool isEligibleForDiscount = discountAst.Interpret(cart);

            Console.WriteLine($"Rule Applied: {adminRule}");
            Console.WriteLine($"Is Customer Eligible for 10% Discount? : {isEligibleForDiscount}");
        }
    }
}
