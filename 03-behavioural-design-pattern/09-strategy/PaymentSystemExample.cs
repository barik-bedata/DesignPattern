using System;

namespace BehavioralDesignPattern.Strategy.PaymentSystem
{
    // ==========================================
    // 2. Strategy Interface
    // ==========================================
    // Defines a common interface that all concrete strategies must implement.
    // Ensures consistency so all strategies are interchangeable.
    public interface IPaymentStrategy
    {
        void Pay(int amount);
    }

    // ==========================================
    // 3. Concrete Strategies
    // ==========================================
    // Provide specific implementations of the strategy interface with different algorithms or behaviors.
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        private readonly string _cardNumber;

        public CreditCardPaymentStrategy(string cardNumber)
        {
            _cardNumber = cardNumber;
        }

        public void Pay(int amount)
        {
            Console.WriteLine($"[Credit Card] Paid ${amount} using card ending in {_cardNumber.Substring(_cardNumber.Length - 4)}.");
        }
    }

    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        private readonly string _email;

        public PayPalPaymentStrategy(string email)
        {
            _email = email;
        }

        public void Pay(int amount)
        {
            Console.WriteLine($"[PayPal] Paid ${amount} using account ({_email}).");
        }
    }

    public class BkashPaymentStrategy : IPaymentStrategy
    {
        private readonly string _mobileNumber;

        public BkashPaymentStrategy(string mobileNumber)
        {
            _mobileNumber = mobileNumber;
        }

        public void Pay(int amount)
        {
            Console.WriteLine($"[bKash] Paid ${amount} using mobile number ({_mobileNumber}).");
        }
    }

    // ==========================================
    // 1. Context
    // ==========================================
    // Acts as an intermediary between the client and the strategy, delegating tasks to the selected strategy.
    public class ShoppingCart
    {
        private IPaymentStrategy _paymentStrategy;

        // Context allows setting the strategy dynamically at runtime
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
        }

        public void Checkout(int totalAmount)
        {
            if (_paymentStrategy == null)
            {
                Console.WriteLine("Error: Payment method not selected! Please select a strategy.");
                return;
            }

            Console.WriteLine($"\n--> [Context] Delegating payment task to {_paymentStrategy.GetType().Name}...");
            _paymentStrategy.Pay(totalAmount);
        }
    }

    // ==========================================
    // 4. Client
    // ==========================================
    // Responsible for selecting and configuring the appropriate strategy for the context.
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Strategy Pattern (E-Commerce Payment System) ===\n");

            // 1. Create Context
            ShoppingCart cart = new ShoppingCart();
            int amountToPay = 500;

            // 2. Client decides which strategy to use and passes it to the context
            cart.SetPaymentStrategy(new CreditCardPaymentStrategy("1234567890123456"));
            cart.Checkout(amountToPay);

            // 3. Client decides to switch strategy to PayPal dynamically
            cart.SetPaymentStrategy(new PayPalPaymentStrategy("user@example.com"));
            cart.Checkout(amountToPay);

            // 4. Client decides to switch strategy to bKash
            cart.SetPaymentStrategy(new BkashPaymentStrategy("01711000000"));
            cart.Checkout(amountToPay);
        }
    }
}
