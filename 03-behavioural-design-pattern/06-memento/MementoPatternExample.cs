using System;
using System.Collections.Generic;
using System.Linq;

namespace BehavioralDesignPattern.Memento
{
    // ==========================================
    // 2. Memento Interface (স্ন্যাপশট বা মেমোরি কার্ডের নিয়ম)
    // ==========================================
    // এটি একটি Opaque (অস্বচ্ছ) ইন্টারফেস। এতে GetState() নেই!
    // ফলে Caretaker চাইলেও এর ভেতরের আসল ডেটা পড়তে পারবে না।
    public interface IMemento
    {
        string GetName();
        DateTime GetDate();
    }

    // ==========================================
    // 1. Originator Interface (যার স্টেট সেভ করা হবে তার নিয়ম)
    // ==========================================
    public interface IOriginator
    {
        IMemento Save();
        void Restore(IMemento memento);
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

        public void Play(string newState)
        {
            Console.WriteLine($"[Game] Playing... State changed to: {newState}");
            _state = newState;
        }

        public IMemento Save()
        {
            Console.WriteLine("[Game] Saving current state to a Memento...");
            return new GameMemento(_state);
        }

        public void Restore(IMemento memento)
        {
            // কাস্টিং করে আসল Memento তে কনভার্ট করে নিচ্ছি। 
            // যেহেতু GameMemento একটি প্রাইভেট ক্লাস, তাই শুধু GamePlayer ই এই কাস্টিং করতে পারবে!
            if (!(memento is GameMemento concreteMemento))
            {
                throw new Exception("Unknown memento class!");
            }

            _state = concreteMemento.GetState(); // এখন শুধু প্লেয়ারই ডেটা পড়তে পারছে
            Console.WriteLine($"[Game] Restored to previous state: {_state}");
        }

        // ==========================================
        // Concrete Memento (Nested Private Class)
        // ==========================================
        // এটি GamePlayer এর ভেতরে Private অবস্থায় আছে।
        // তাই বাইরের দুনিয়া একে চেনে শুধু IMemento হিসেবে।
        private class GameMemento : IMemento
        {
            private readonly string _state;
            private readonly DateTime _date;

            public GameMemento(string state)
            {
                _state = state;
                _date = DateTime.Now;
            }

            // এই মেথডটি ইন্টারফেসে নেই। এটি শুধু GamePlayer দেখতে পাবে কারণ এটি তার Nested Class.
            public string GetState() => _state;
            
            public string GetName() => $"{_date:HH:mm:ss} - {_state}";
            public DateTime GetDate() => _date;
        }
    }

    // ==========================================
    // 3. Caretaker (Undo History বা Save Manager)
    // ==========================================
    public class GameSaveManager
    {
        private readonly List<IMemento> _mementos = new List<IMemento>();
        private readonly IOriginator _originator;

        public GameSaveManager(IOriginator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\n[SaveManager] Requesting save...");
            var memento = _originator.Save();
            _mementos.Add(memento);
            // memento.GetState() ❌ -> Caretaker ডেটা পড়তে পারবে না! (100% Encapsulation)
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                Console.WriteLine("[SaveManager] No saved games found!");
                return;
            }

            Console.WriteLine("\n[SaveManager] Undoing last move...");
            var lastMemento = _mementos.Last();
            _mementos.Remove(lastMemento); 

            _originator.Restore(lastMemento); 
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
    // 4. Client
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Memento Pattern (Video Game Save/Undo) ===\n");

            GamePlayer player = new GamePlayer("Level 1, Health 100%");
            GameSaveManager saveManager = new GameSaveManager(player);

            player.Play("Level 2, Health 80%");
            saveManager.Backup();

            player.Play("Level 3, Health 50%");
            saveManager.Backup();

            player.Play("Level 3, Boss Fight, Health 5%");
            saveManager.ShowHistory();

            Console.WriteLine("\n--- Player realizes they are about to die! Loading saved game... ---");
            saveManager.Undo(); 

            Console.WriteLine("\n--- Player wants to prepare better. Loading older saved game... ---");
            saveManager.Undo(); 
        }
    }
}
