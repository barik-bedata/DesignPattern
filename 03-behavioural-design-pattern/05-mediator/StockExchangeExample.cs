using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Mediator.StockExchange
{
    // 1. Mediator Interface (ব্রোকারের নিয়ম)
    public interface IStockBroker
    {
        void RegisterTrader(ITrader trader);
        void ExecuteTrade(string stock, int quantity, ITrader buyer);
    }

    // 2. Colleague Interface (ট্রেডারদের কমন চেহারা)
    public interface ITrader
    {
        string Name { get; }
        void Buy(string stock, int quantity);
        void ReceiveNotification(string message);
    }

    // 3. Concrete Mediator (আসল স্টক এক্সচেঞ্জ ব্রোকার)
    public class NyseBroker : IStockBroker
    {
        private readonly List<ITrader> _traders = new List<ITrader>();

        public void RegisterTrader(ITrader trader) => _traders.Add(trader);

        public void ExecuteTrade(string stock, int quantity, ITrader buyer)
        {
            Console.WriteLine($"\n[Broker] Executing trade: {buyer.Name} buys {quantity} shares of {stock}");
            
            // একজন স্টক কিনলে বাকি ট্রেডারদের নোটিফিকেশন পাঠিয়ে দেওয়া হচ্ছে
            foreach (var trader in _traders)
            {
                if (trader != buyer)
                {
                    trader.ReceiveNotification($"Market Update: {quantity} shares of {stock} just sold.");
                }
            }
        }
    }

    // 4. Concrete Colleague (আসল ট্রেডার)
    public class StockTrader : ITrader
    {
        private readonly IStockBroker _broker; // ট্রেডার শুধু ব্রোকারকে চেনে
        public string Name { get; }

        public StockTrader(IStockBroker broker, string name)
        {
            _broker = broker;
            Name = name;
        }

        public void Buy(string stock, int quantity) => _broker.ExecuteTrade(stock, quantity, this);
        
        public void ReceiveNotification(string message) => Console.WriteLine($"[{Name}] received: {message}");
    }

    // ==========================================
    // 5. Client (মেইন অ্যাপ্লিকেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (Stock Exchange) ===\n");

            IStockBroker broker = new NyseBroker();
            
            ITrader trader1 = new StockTrader(broker, "Bedata");
            ITrader trader2 = new StockTrader(broker, "Alice");

            broker.RegisterTrader(trader1);
            broker.RegisterTrader(trader2);

            trader1.Buy("AAPL", 100);
        }
    }
}
