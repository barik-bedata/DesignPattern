using System;
using System.Collections.Generic;
using System.Linq;

namespace BehavioralDesignPattern.Memento
{
    // ==========================================
    // 2. Memento Interface (স্ন্যাপশট বা মেমোরি কার্ডের নিয়ম)
    // ==========================================
    // এটি শুধু ডেটা রিড করার পারমিশন দেয়, মডিফাই করার কোনো অপশন নেই।
    public interface IMemento
    {
        string GetName();
        string GetState();
        DateTime GetDate();
    }

    // ==========================================
    // 1. Originator Interface (যার স্টেট সেভ করা হবে তার নিয়ম)
    // ==========================================
    // এই ইন্টারফেসটি ক্লায়েন্ট বা Caretaker ব্যবহার করবে (DIP মানার জন্য)।
    public interface IOriginator
    {
        IMemento Save();
        void Restore(IMemento memento);
    }

    // ==========================================
    // Concrete Memento (আসল স্ন্যাপশট / Saved Game)
    // ==========================================
    public class GameMemento : IMemento
    {
        private readonly string _state;
        private readonly DateTime _date;

        public GameMemento(string state)
        {
            _state = state;
            _date = DateTime.Now;
        }

        public string GetState() => _state;
        public string GetName() => $"{_date:HH:mm:ss} - {_state}";
        public DateTime GetDate() => _date;
    }

    // ==========================================
    // Concrete Originator (আসল প্লেয়ার বা গেম)
    // ==========================================
    public class GamePlayer : IOriginator
    {
        private string _state; // গেমের বর্তমান অবস্থা (যেমন: "Level 1, Health 100")

        public GamePlayer(string initialState)
        {
            Console.WriteLine($"[Game] Starting game... Initial state: {initialState}");
            _state = initialState;
        }

        // গেম খেলা হচ্ছে এবং স্টেট চেঞ্জ হচ্ছে
        public void Play(string newState)
        {
            Console.WriteLine($"[Game] Playing... State changed to: {newState}");
            _state = newState;
        }

        // নিজের বর্তমান স্টেটের একটি স্ন্যাপশট (Memento) তৈরি করে দিচ্ছে
        public IMemento Save()
        {
            Console.WriteLine("[Game] Saving current state to a Memento...");
            return new GameMemento(_state);
        }

        // পুরনো Memento থেকে আগের অবস্থায় ফিরে যাওয়া (Undo/Load Game)
        public void Restore(IMemento memento)
        {
            _state = memento.GetState();
            Console.WriteLine($"[Game] Restored to previous state: {_state}");
        }
    }

    // ==========================================
    // 3. Caretaker (Undo History বা Save Manager)
    // ==========================================
    public class GameSaveManager
    {
        private readonly List<IMemento> _mementos = new List<IMemento>();
        private readonly IOriginator _originator;

        // Caretaker সরাসরি Concrete Class না চিনে IOriginator ইন্টারফেস চেনে (DIP)
        public GameSaveManager(IOriginator originator)
        {
            _originator = originator;
        }

        // গেম সেভ করার রিকোয়েস্ট (Backup)
        public void Backup()
        {
            Console.WriteLine("\n[SaveManager] Requesting save...");
            var memento = _originator.Save();
            _mementos.Add(memento);
        }

        // গেম আনডু (Undo) করার রিকোয়েস্ট
        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                Console.WriteLine("[SaveManager] No saved games found!");
                return;
            }

            Console.WriteLine("\n[SaveManager] Undoing last move...");
            var lastMemento = _mementos.Last(); // লিস্টের শেষেরটা ধরলাম
            _mementos.Remove(lastMemento); // লিস্ট থেকে রিমুভ করে দিচ্ছি

            _originator.Restore(lastMemento); // গেমকে আগের অবস্থায় পাঠাচ্ছি
        }

        public void ShowHistory()
        {
            Console.WriteLine("\n[SaveManager] Saved Games History:");
            foreach (var m in _mementos)
            {
                Console.WriteLine($"  -> {m.GetName()}");
            }
        }
    }

    // ==========================================
    // 4. Client (ইউজার)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Memento Pattern (Video Game Save/Undo) ===\n");

            // ১. গেম চালু করা হলো (Originator)
            // এখানে IOriginator দিয়ে ডিক্লেয়ার করা যায়, কিন্তু Play() মেথডের জন্য আমরা Concrete Class রাখছি।
            // আসল DIP মেইনটেইন করা হয়েছে GameSaveManager এর কনস্ট্রাক্টরে।
            GamePlayer player = new GamePlayer("Level 1, Health 100%");
            
            // ২. সেভ ম্যানেজার তৈরি (Caretaker)
            GameSaveManager saveManager = new GameSaveManager(player); // এখানে DIP মানা হচ্ছে

            // প্লেয়ার লেভেল ২ তে গেলো
            player.Play("Level 2, Health 80%");
            saveManager.Backup(); // সেভ পয়েন্ট ১

            // প্লেয়ার লেভেল ৩ তে গেলো
            player.Play("Level 3, Health 50%");
            saveManager.Backup(); // সেভ পয়েন্ট ২

            // বসের সাথে ফাইট করতে গিয়ে হেলথ অনেক কমে গেলো!
            player.Play("Level 3, Boss Fight, Health 5%");

            saveManager.ShowHistory(); // হিস্ট্রি চেক

            // প্লেয়ার দেখলো অবস্থা খারাপ, সে আনডু (Undo) করলো!
            Console.WriteLine("\n--- Player realizes they are about to die! Loading saved game... ---");
            saveManager.Undo(); // লেভেল ৩ এর শুরুতে ফিরে যাবে

            // প্লেয়ার আবার আনডু করলো!
            Console.WriteLine("\n--- Player wants to prepare better. Loading older saved game... ---");
            saveManager.Undo(); // লেভেল ২ এর শুরুতে ফিরে যাবে
        }
    }
}
