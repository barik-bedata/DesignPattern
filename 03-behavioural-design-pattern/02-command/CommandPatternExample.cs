using System;

namespace BehavioralDesignPattern.Command
{
    // ==========================================
    // 1. Receiver (যে আসল কাজটা করতে জানে)
    // ==========================================
    // (DIP এর জন্য ইন্টারফেস এবং তার ইমপ্লিমেন্টেশন দুটোই Receiver-এর অংশ)
    
    public interface IDevice
    {
        void TurnOn();
        void TurnOff();
    }
    
    public class TV : IDevice
    {
        public void TurnOn()
        {
            Console.WriteLine("[TV Receiver] 📺 Television is now ON.");
        }

        public void TurnOff()
        {
            Console.WriteLine("[TV Receiver] 📺 Television is now OFF.");
        }
    }

    public class AirConditioner : IDevice
    {
        public void TurnOn()
        {
            Console.WriteLine("[AC Receiver] ❄️ Air Conditioner is now ON.");
        }

        public void TurnOff()
        {
            Console.WriteLine("[AC Receiver] ❄️ Air Conditioner is now OFF.");
        }
    }

    // ==========================================
    // 2. Command Interface (নির্দেশের কমন নিয়ম)
    // ==========================================
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // ==========================================
    // 3. Concrete Commands (নির্দিষ্ট বাটন)
    // ==========================================
    
    public class TurnOnCommand : ICommand
    {
        private readonly IDevice _device;

        public TurnOnCommand(IDevice device)
        {
            _device = device;
        }

        public void Execute()
        {
            _device.TurnOn();
        }

        public void Undo()
        {
            _device.TurnOff();
        }
    }

    public class TurnOffCommand : ICommand
    {
        private readonly IDevice _device;

        public TurnOffCommand(IDevice device)
        {
            _device = device;
        }

        public void Execute()
        {
            _device.TurnOff();
        }

        public void Undo()
        {
            _device.TurnOn();
        }
    }

    // ==========================================
    // 4. Invoker (যে নির্দেশটা জারি করে)
    // ==========================================
    // (DIP এর জন্য ইন্টারফেস এবং তার ইমপ্লিমেন্টেশন দুটোই Invoker-এর অংশ)

    public interface IRemoteControl
    {
        void SetCommands(ICommand onCommand, ICommand offCommand);
        void PressOnButton();
        void PressOffButton();
        void PressUndoButton();
    }

    // এটি হলো আপনার ইউনিভার্সাল রিমোট কন্ট্রোল।
    public class RemoteControl : IRemoteControl
    {
        private ICommand _onCommand;
        private ICommand _offCommand;
        private ICommand _lastExecuted;

        public void SetCommands(ICommand onCommand, ICommand offCommand)
        {
            _onCommand = onCommand;
            _offCommand = offCommand;
        }

        public void PressOnButton()
        {
            _onCommand.Execute();
            _lastExecuted = _onCommand;
            Console.WriteLine("[Remote Invoker] 🔘 ON Button pressed...");
        }

        public void PressOffButton()
        {
            _offCommand.Execute();
            _lastExecuted = _offCommand;
            Console.WriteLine("[Remote Invoker] 🔘 OFF Button pressed...");
        }

        public void PressUndoButton()
        {
            Console.WriteLine("[Remote Invoker] ↩️ Undo button pressed...");
            _lastExecuted?.Undo();
        }
    }

    // ==========================================
    // Client Code (ইউজার)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Command Pattern (100% SOLID - Universal Remote) ===\n");

            // ১. রিসিভার তৈরি
            IDevice myTv = new TV();
            IDevice myAc = new AirConditioner();

            // ২. ইনভোকার তৈরি 
            IRemoteControl remote = new RemoteControl();

            // ============ টিভি টেস্ট ============
            Console.WriteLine("--> Testing TV:");
            ICommand turnOnTv = new TurnOnCommand(myTv);
            ICommand turnOffTv = new TurnOffCommand(myTv);

            // রিমোটে অন এবং অফ বাটন একবার কনফিগার করা হলো (যেমন আসল রিমোট থাকে)
            remote.SetCommands(turnOnTv, turnOffTv);

            // এখন আপনি আরামসে রিমোটের বাটন চাপুন!
            remote.PressOnButton();
            remote.PressOffButton();
            
            Console.WriteLine("Wait, Undo that!");
            remote.PressUndoButton();

            // ============ এসি টেস্ট ============
            Console.WriteLine("\n--> Testing AC with the same remote:");
            ICommand turnOnAc = new TurnOnCommand(myAc);
            ICommand turnOffAc = new TurnOffCommand(myAc);
            
            // রিমোটে এখন এসির কনফিগারেশন সেট করা হলো
            remote.SetCommands(turnOnAc, turnOffAc);

            remote.PressOnButton();
            remote.PressUndoButton(); // ঠান্ডা লাগছে, আবার অফ করে দাও!
            
            Console.WriteLine("Wait, it's too cold! Undo!");
            remote.PressUndoButton();
        }
    }
}
