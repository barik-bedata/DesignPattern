using System;

namespace BehavioralDesignPattern.State.AtmMachine
{
    // ==========================================
    // 1. State Interface/Base Class
    // ==========================================
    // Defines common methods for all states, allowing Context to work with them without knowing concrete types.
    public interface IAtmState
    {
        void InsertCard(IAtmMachine context);
        void EjectCard(IAtmMachine context);
        void EnterPin(IAtmMachine context, int pin);
        void WithdrawCash(IAtmMachine context, int amount);
    }

    // ==========================================
    // 2. Concrete States
    // ==========================================
    // Implement the State interface, encapsulating behavior for specific states and defining Context’s actions in those states.
    public class NoCardState : IAtmState
    {
        public void InsertCard(IAtmMachine context)
        {
            Console.WriteLine("[No Card] Card inserted securely.");
            context.ChangeState(new HasCardState());
        }

        public void EjectCard(IAtmMachine context) => Console.WriteLine("[No Card] No card to eject.");
        public void EnterPin(IAtmMachine context, int pin) => Console.WriteLine("[No Card] Please insert card first.");
        public void WithdrawCash(IAtmMachine context, int amount) => Console.WriteLine("[No Card] Please insert card first.");
    }

    public class HasCardState : IAtmState
    {
        public void InsertCard(IAtmMachine context) => Console.WriteLine("[Has Card] Card is already inserted.");

        public void EjectCard(IAtmMachine context)
        {
            Console.WriteLine("[Has Card] Ejecting card.");
            context.ChangeState(new NoCardState());
        }

        public void EnterPin(IAtmMachine context, int pin)
        {
            if (pin == context.ValidPin)
            {
                Console.WriteLine("[Has Card] PIN Accepted.");
                context.ChangeState(new PinEnteredState());
            }
            else
            {
                Console.WriteLine("[Has Card] Incorrect PIN. Ejecting card.");
                context.ChangeState(new NoCardState());
            }
        }

        public void WithdrawCash(IAtmMachine context, int amount) => Console.WriteLine("[Has Card] Please enter PIN first.");
    }

    public class PinEnteredState : IAtmState
    {
        public void InsertCard(IAtmMachine context) => Console.WriteLine("[PIN Entered] Card already inserted.");

        public void EjectCard(IAtmMachine context)
        {
            Console.WriteLine("[PIN Entered] Ejecting card.");
            context.ChangeState(new NoCardState());
        }

        public void EnterPin(IAtmMachine context, int pin) => Console.WriteLine("[PIN Entered] PIN already entered.");

        public void WithdrawCash(IAtmMachine context, int amount)
        {
            if (context.CashInMachine < amount)
            {
                Console.WriteLine($"[PIN Entered] ATM has insufficient cash. Machine has: ${context.CashInMachine}");
            }
            else
            {
                Console.WriteLine($"[PIN Entered] Dispensing ${amount}. Please collect your cash.");
                context.DeductCash(amount);
            }
            
            Console.WriteLine("[PIN Entered] Transaction complete. Ejecting card.");
            context.ChangeState(new NoCardState());
        }
    }

    // ==========================================
    // 3. Context
    // ==========================================
    // Maintains a reference to the current state, delegates behavior to it, and provides an interface for clients.
    
    // DIP মানার জন্য IAtmMachine ইন্টারফেস (Context Interface) তৈরি করা হলো
    public interface IAtmMachine
    {
        int CashInMachine { get; }
        int ValidPin { get; } // OCP: Hardcoded PIN এড়ানোর জন্য ভ্যালিড পিন Context থেকে আসবে
        void ChangeState(IAtmState newState);
        void DeductCash(int amount);
    }

    public class AtmMachine : IAtmMachine
    {
        private IAtmState _currentState;
        public int CashInMachine { get; private set; }
        public int ValidPin { get; private set; } // OCP: পিন এখন ডাইনামিক

        public AtmMachine(int initialCash, int validPin)
        {
            CashInMachine = initialCash;
            ValidPin = validPin;
            _currentState = new NoCardState(); // Encapsulated default state
        }

        public void ChangeState(IAtmState newState)
        {
            _currentState = newState;
        }

        public void DeductCash(int amount)
        {
            CashInMachine -= amount;
        }

        // Context নিজে কোনো কাজ করে না। সে তার বর্তমান State (অবজেক্ট)-এর কাছে কাজটি পাঠিয়ে দেয় (Delegate করে)।
        // ক্লায়েন্ট জানে না ব্যাকএন্ডে কোন স্টেট কাজ করছে।
        public void InsertCard() 
        {
            Console.WriteLine($"--> [Context] Delegating InsertCard() to {_currentState.GetType().Name}");
            _currentState.InsertCard(this);
        }

        public void EjectCard() 
        {
            Console.WriteLine($"--> [Context] Delegating EjectCard() to {_currentState.GetType().Name}");
            _currentState.EjectCard(this);
        }

        public void EnterPin(int pin) 
        {
            Console.WriteLine($"--> [Context] Delegating EnterPin() to {_currentState.GetType().Name}");
            _currentState.EnterPin(this, pin);
        }

        public void WithdrawCash(int amount) 
        {
            Console.WriteLine($"--> [Context] Delegating WithdrawCash() to {_currentState.GetType().Name}");
            _currentState.WithdrawCash(this, amount);
        }
    }

    // ==========================================
    // Client Usage
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== State Pattern (ATM Machine) ===\n");

            AtmMachine atm = new AtmMachine(2000, 1234);

            atm.InsertCard();
            atm.EnterPin(1234);
            atm.WithdrawCash(500);

            Console.WriteLine("\n--- Scenario 2: Incorrect PIN ---");
            atm.InsertCard();
            atm.EnterPin(9999);

            Console.WriteLine("\n--- Scenario 3: Trying to withdraw without PIN ---");
            atm.InsertCard();
            atm.WithdrawCash(100);
        }
    }
}
