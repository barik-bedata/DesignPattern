using System;

namespace BehavioralDesignPattern.State.VendingMachine
{
    // ==========================================
    // 1. State Interface/Base Class
    // ==========================================
    // Defines common methods for all states, allowing Context to work with them without knowing concrete types.
    public interface IVendingMachineState
    {
        void InsertMoney(IVendingMachine context, int amount);
        void EjectMoney(IVendingMachine context);
        void SelectProduct(IVendingMachine context);
    }

    // ==========================================
    // 2. Concrete States
    // ==========================================
    // Implement the State interface, encapsulating behavior for specific states and defining Context’s actions in those states.
    public class NoMoneyState : IVendingMachineState
    {
        public void InsertMoney(IVendingMachine context, int amount)
        {
            Console.WriteLine($"[No Money] Inserted ${amount}.");
            context.ChangeState(new HasMoneyState(amount));
        }

        public void EjectMoney(IVendingMachine context) => Console.WriteLine("[No Money] Cannot eject. No money inserted.");
        public void SelectProduct(IVendingMachine context) => Console.WriteLine("[No Money] Cannot select product. Please insert money first.");
    }

    public class HasMoneyState : IVendingMachineState
    {
        private int _currentBalance;

        public HasMoneyState(int balance)
        {
            _currentBalance = balance;
        }

        public void InsertMoney(IVendingMachine context, int amount)
        {
            _currentBalance += amount;
            Console.WriteLine($"[Has Money] Inserted extra ${amount}. Total: ${_currentBalance}");
        }

        public void EjectMoney(IVendingMachine context)
        {
            Console.WriteLine($"[Has Money] Returning ${_currentBalance} to customer.");
            context.ChangeState(new NoMoneyState());
        }

        public void SelectProduct(IVendingMachine context)
        {
            Console.WriteLine($"[Has Money] Product selected. Deducting balance and dispensing...");
            context.ChangeState(new DispensingState());
            context.DispenseProduct(); 
        }
    }

    public class DispensingState : IVendingMachineState
    {
        public void InsertMoney(IVendingMachine context, int amount) => Console.WriteLine("[Dispensing] Please wait. Currently dispensing a product.");
        public void EjectMoney(IVendingMachine context) => Console.WriteLine("[Dispensing] Cannot eject money. Product is already coming out.");
        public void SelectProduct(IVendingMachine context) => Console.WriteLine("[Dispensing] Already dispensing a product.");
    }

    // ==========================================
    // 3. Context
    // ==========================================
    // Maintains a reference to the current state, delegates behavior to it, and provides an interface for clients.
    
    // DIP মানার জন্য IVendingMachine ইন্টারফেস (Context Interface) তৈরি করা হলো
    public interface IVendingMachine
    {
        void ChangeState(IVendingMachineState newState);
        void DispenseProduct();
    }

    public class VendingMachine : IVendingMachine
    {
        private IVendingMachineState _currentState;

        public VendingMachine()
        {
            _currentState = new NoMoneyState();
        }

        public void ChangeState(IVendingMachineState newState)
        {
            _currentState = newState;
        }

        public void DispenseProduct()
        {
            Console.WriteLine(">>> VENDING MACHINE: Here is your Coke! <<<");
            ChangeState(new NoMoneyState()); // Machine resets after dispense
        }

        // Context নিজে কোনো কাজ করে না। সে তার বর্তমান State (অবজেক্ট)-এর কাছে কাজটি পাঠিয়ে দেয় (Delegate করে)।
        public void InsertMoney(int amount) 
        {
            Console.WriteLine($"--> [Context] Delegating InsertMoney() to {_currentState.GetType().Name}");
            _currentState.InsertMoney(this, amount);
        }

        public void EjectMoney() 
        {
            Console.WriteLine($"--> [Context] Delegating EjectMoney() to {_currentState.GetType().Name}");
            _currentState.EjectMoney(this);
        }

        public void SelectProduct() 
        {
            Console.WriteLine($"--> [Context] Delegating SelectProduct() to {_currentState.GetType().Name}");
            _currentState.SelectProduct(this);
        }
    }

    // ==========================================
    // Client Usage
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== State Pattern (Vending Machine) ===\n");

            VendingMachine machine = new VendingMachine();

            machine.SelectProduct(); 

            machine.InsertMoney(10);
            machine.InsertMoney(5);
            machine.SelectProduct(); 

            Console.WriteLine("\n--- Another Customer ---");
            machine.InsertMoney(20);
            machine.EjectMoney(); 
        }
    }
}
