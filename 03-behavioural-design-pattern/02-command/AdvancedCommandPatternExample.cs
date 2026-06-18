using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Command.Advanced
{
    // ==========================================
    // 1. Receiver
    // ==========================================
    // (DIP এর জন্য ইন্টারফেস এবং তার ইমপ্লিমেন্টেশন দুটোই Receiver-এর অংশ)
    
    public interface IDevice
    {
        void TurnOn();
        void TurnOff();
    }

    public class Light : IDevice
    {
        public void TurnOn() => Console.WriteLine("[Light] 💡 Light turned ON.");
        public void TurnOff() => Console.WriteLine("[Light] 💡 Light turned OFF.");
    }

    public class Speaker : IDevice
    {
        public void TurnOn() => Console.WriteLine("[Speaker] 🎵 Playing Party Music.");
        public void TurnOff() => Console.WriteLine("[Speaker] 🎵 Music Stopped.");
    }

    // ==========================================
    // 2. Command Interface
    // ==========================================
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // ==========================================
    // 3. Concrete Commands
    // ==========================================
    
    public class TurnOnCommand : ICommand
    {
        private readonly IDevice _device;
        public TurnOnCommand(IDevice device) { _device = device; }
        public void Execute() => _device.TurnOn();
        public void Undo() => _device.TurnOff();
    }

    public class TurnOffCommand : ICommand
    {
        private readonly IDevice _device;
        public TurnOffCommand(IDevice device) { _device = device; }
        public void Execute() => _device.TurnOff();
        public void Undo() => _device.TurnOn();
    }

    // ------------------------------------------
    // ✨ MACRO COMMAND (Composite Command) ✨
    // ------------------------------------------
    // এটি এমন একটি স্পেশাল কমান্ড যার ভেতরে অনেকগুলো কমান্ডের লিস্ট থাকে।
    public class MacroCommand : ICommand
    {
        private readonly IReadOnlyList<ICommand> _commands;

        public MacroCommand(IEnumerable<ICommand> commands)
        {
            // IEnumerable রিসিভ করে লিস্টে কনভার্ট করে নিলাম (Strict DIP)
            _commands = new List<ICommand>(commands);
        }

        public void Execute()
        {
            Console.WriteLine("[Macro] Executing multiple commands at once...");
            foreach (var cmd in _commands)
            {
                cmd.Execute();
            }
        }

        public void Undo()
        {
            Console.WriteLine("[Macro] Undoing all commands in reverse order...");
            // আনডু করার সময় সবসময় উল্টো দিক থেকে লুপ চালাতে হয়
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }
    }

    // ==========================================
    // 4. Invoker (Queueing Example)
    // ==========================================
    // (DIP এর জন্য ইন্টারফেস এবং তার ইমপ্লিমেন্টেশন দুটোই Invoker-এর অংশ)

    public interface ISmartHomeHub
    {
        void AddCommandToQueue(ICommand command);
        void ExecuteAllInQueue();
        void UndoLastCommand(); // আনডু করার কন্ট্রাক্ট
    }

    public class SmartHomeHub : ISmartHomeHub
    {
        // কমান্ডগুলোকে সাথে সাথে রান না করে লাইনে (Queue) দাঁড় করানো হচ্ছে
        private readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();
        
        // আনডু করার জন্য এক্সিকিউট হওয়া কমান্ডগুলোর হিস্ট্রি (Stack) সেভ রাখা হচ্ছে!
        private readonly Stack<ICommand> _executionHistory = new Stack<ICommand>();

        public void AddCommandToQueue(ICommand command)
        {
            _commandQueue.Enqueue(command);
            Console.WriteLine("[Hub] 📥 Command added to background queue.");
        }

        public void ExecuteAllInQueue()
        {
            Console.WriteLine("\n[Hub] 🚀 Executing all queued commands...");
            while (_commandQueue.Count > 0)
            {
                ICommand cmd = _commandQueue.Dequeue();
                cmd.Execute(); // ইনভোকার জানেও না সে কী রান করছে!
                
                // রিকোয়েস্ট রান হওয়ার পর আনডু করার জন্য হিস্ট্রিতে পুশ করে রাখা হলো
                _executionHistory.Push(cmd);
            }
        }

        public void UndoLastCommand()
        {
            if (_executionHistory.Count > 0)
            {
                Console.WriteLine("\n[Hub] ↩️ Undoing last executed command from history...");
                ICommand lastCmd = _executionHistory.Pop();
                lastCmd.Undo();
            }
            else
            {
                Console.WriteLine("\n[Hub] ⚠️ No history to undo.");
            }
        }
    }

    // ==========================================
    // Client Code 
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Advanced Command Pattern (Queue & Macro) ===\n");

            // ১. রিসিভার তৈরি
            IDevice light = new Light();
            IDevice speaker = new Speaker();

            // ২. বেসিক কমান্ড তৈরি
            ICommand lightOn = new TurnOnCommand(light);
            ICommand speakerOn = new TurnOnCommand(speaker);
            ICommand lightOff = new TurnOffCommand(light);
            ICommand speakerOff = new TurnOffCommand(speaker);

            // ============================================
            // 🎯 QUEUEING EXAMPLE (ব্যাকগ্রাউন্ড জব সিমুলেশন)
            // ============================================
            Console.WriteLine("--- QUEUEING EXAMPLE ---");
            ISmartHomeHub hub = new SmartHomeHub();
            
            // রিকোয়েস্টগুলো সাথে সাথে রান হবে না, লাইনে দাঁড়াবে
            hub.AddCommandToQueue(lightOn);
            hub.AddCommandToQueue(speakerOn);
            
            // এবার একসাথে সব রান করানো হলো (যেমন: Cron Job বা Background Task)
            hub.ExecuteAllInQueue();

            // ============================================
            // 🎯 MACRO COMMAND EXAMPLE (পার্টি মোড!)
            // ============================================
            Console.WriteLine("\n--- MACRO COMMAND EXAMPLE ---");
            
            // একটি 'Party Mode' কমান্ড বানানো হলো যার ভেতরে লাইট এবং স্পিকার দুটোর কমান্ড আছে
            List<ICommand> partyCommands = new List<ICommand> { lightOn, speakerOn };
            ICommand partyModeMacro = new MacroCommand(partyCommands);

            // ম্যাক্রো কমান্ড ইনভোকারের (Hub) মাধ্যমে কল করা হলো
            Console.WriteLine("--> Enabling Party Mode via Hub:");
            hub.AddCommandToQueue(partyModeMacro);
            hub.ExecuteAllInQueue();

            // ইনভোকার (Hub) যেহেতু হিস্ট্রি ট্র্যাক করছে, তাই তাকে আনডু করতে বললেই সে করে দেবে!
            Console.WriteLine("\n--> Wait, neighbors are complaining! Undo Party Mode:");
            hub.UndoLastCommand();
        }
    }
}
