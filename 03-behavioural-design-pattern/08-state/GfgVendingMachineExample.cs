using System;

namespace BehavioralDesignPattern.State.GfgVendingMachine
{
    // ==========================================
    // 1. State Interface/Base Class
    // ==========================================
    // Defines common methods for all states, allowing Context to work with them without knowing concrete types.
    public interface IVendingMachineState 
    {
        void HandleRequest();
    }

    // ==========================================
    // 2. Concrete States
    // ==========================================
    // Implement the State interface, encapsulating behavior for specific states and defining Context’s actions in those states.
    public class ReadyState : IVendingMachineState 
    {
        public void HandleRequest() 
        {
            Console.WriteLine("Ready state: Please select a product.");
        }
    }

    public class ProductSelectedState : IVendingMachineState 
    {
        public void HandleRequest() 
        {
            Console.WriteLine("Product selected state: Processing payment.");
        }
    }

    public class PaymentPendingState : IVendingMachineState 
    {
        public void HandleRequest() 
        {
            Console.WriteLine("Payment pending state: Dispensing product.");
        }
    }

    public class OutOfStockState : IVendingMachineState 
    {
        public void HandleRequest() 
        {
            Console.WriteLine("Out of stock state: Product unavailable. Please select another product.");
        }
    }

    // ==========================================
    // 3. Context
    // ==========================================
    // Maintains a reference to the current state, delegates behavior to it, and provides an interface for clients.
    public class VendingMachineContext 
    {
        private IVendingMachineState _state;

        public void SetState(IVendingMachineState state) 
        {
            _state = state;
        }

        public void Request() 
        {
            _state?.HandleRequest();
        }
    }

    // ==========================================
    // Client (Main Class)
    // ==========================================
    public class Program 
    {
        public static void Run() 
        {
            Console.WriteLine("=== State Pattern (GeeksforGeeks Direct Translation) ===\n");

            // Create context
            VendingMachineContext vendingMachine = new VendingMachineContext();

            // Set initial state
            vendingMachine.SetState(new ReadyState());

            // Request state change
            vendingMachine.Request();

            // Change state
            vendingMachine.SetState(new ProductSelectedState());

            // Request state change
            vendingMachine.Request();

            // Change state
            vendingMachine.SetState(new PaymentPendingState());

            // Request state change
            vendingMachine.Request();

            // Change state
            vendingMachine.SetState(new OutOfStockState());

            // Request state change
            vendingMachine.Request();
        }
    }
}
