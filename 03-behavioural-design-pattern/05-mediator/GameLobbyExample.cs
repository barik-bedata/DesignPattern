using System;
using System.Collections.Generic;
using System.Linq;

namespace BehavioralDesignPattern.Mediator.GameLobby
{
    // 1. Mediator Interface (লবির নিয়ম)
    public interface IGameLobby
    {
        void RegisterPlayer(IPlayer player);
        void NotifyReady(IPlayer player);
    }

    // 2. Colleague Interface (প্লেয়ারদের কমন চেহারা)
    public interface IPlayer
    {
        string Name { get; }
        bool IsReady { get; }
        void SetReady();
        void ReceiveMessage(string message);
    }

    // 3. Concrete Mediator (আসল গেম লবি ম্যানেজার)
    public class MultiplayerLobby : IGameLobby
    {
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public void RegisterPlayer(IPlayer player) => _players.Add(player);

        public void NotifyReady(IPlayer player)
        {
            Console.WriteLine($"[Lobby Manager] {player.Name} is READY.");
            
            // লবি চেক করছে যে সবাই রেডি হয়েছে কি না। 
            // প্লেয়ারদের নিজেদের চেক করতে হচ্ছে না যে কে কে রেডি।
            if (_players.All(p => p.IsReady))
            {
                Console.WriteLine("\n[Lobby Manager] All players are ready! STARTING THE MATCH IN 3...2...1...!");
                foreach (var p in _players) p.ReceiveMessage("MATCH STARTED!");
            }
        }
    }

    // 4. Concrete Colleague (আসল গেমার)
    public class Gamer : IPlayer
    {
        private readonly IGameLobby _lobby;
        public string Name { get; }
        public bool IsReady { get; private set; }

        public Gamer(IGameLobby lobby, string name)
        {
            _lobby = lobby;
            Name = name;
        }

        public void SetReady()
        {
            IsReady = true;
            _lobby.NotifyReady(this); // লবিকে জানাচ্ছে যে সে রেডি
        }

        public void ReceiveMessage(string message) => Console.WriteLine($"[{Name}] sees on screen: {message}");
    }

    // ==========================================
    // 5. Client (মেইন অ্যাপ্লিকেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (Game Lobby) ===\n");

            IGameLobby lobby = new MultiplayerLobby();
            
            IPlayer p1 = new Gamer(lobby, "Player1");
            IPlayer p2 = new Gamer(lobby, "Player2");

            lobby.RegisterPlayer(p1);
            lobby.RegisterPlayer(p2);

            p1.SetReady();
            p2.SetReady(); // যখনই p2 রেডি হবে, লবি গেম স্টার্ট করে দেবে!
        }
    }
}
