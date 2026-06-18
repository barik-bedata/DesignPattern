using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Mediator.ChatRoom
{
    // 1. Mediator Interface (চ্যাট সার্ভারের নিয়ম)
    public interface IChatServer
    {
        void RegisterUser(IUser user);
        void BroadcastMessage(string message, IUser sender);
    }

    // 2. Colleague Interface (ইউজারদের কমন চেহারা)
    public interface IUser
    {
        string Name { get; }
        void Send(string message);
        void Receive(string message, string senderName);
    }

    // 3. Concrete Mediator (আসল চ্যাট সার্ভার)
    public class ChatRoomServer : IChatServer
    {
        private readonly List<IUser> _users = new List<IUser>();

        public void RegisterUser(IUser user)
        {
            _users.Add(user);
            Console.WriteLine($"[Server] {user.Name} has joined the chat.");
        }

        public void BroadcastMessage(string message, IUser sender)
        {
            // যে মেসেজ পাঠিয়েছে সে বাদে বাকি সবাইকে ব্রডকাস্ট করা হচ্ছে
            foreach (var user in _users)
            {
                if (user != sender)
                {
                    user.Receive(message, sender.Name);
                }
            }
        }
    }

    // 4. Concrete Colleague (আসল চ্যাট ইউজার)
    public class ChatUser : IUser
    {
        private readonly IChatServer _server; // User শুধু Server কে চেনে
        public string Name { get; }

        public ChatUser(IChatServer server, string name)
        {
            _server = server;
            Name = name;
        }

        public void Send(string message)
        {
            Console.WriteLine($"\n{Name} sends: {message}");
            _server.BroadcastMessage(message, this); // সার্ভারকে মেসেজ দিচ্ছে
        }

        public void Receive(string message, string senderName)
        {
            Console.WriteLine($"  -> {Name} received from {senderName}: {message}");
        }
    }

    // ==========================================
    // 5. Client (মেইন অ্যাপ্লিকেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (Chat Room) ===\n");

            // ১. সার্ভার তৈরি
            IChatServer server = new ChatRoomServer();

            // ২. ইউজার তৈরি
            IUser bedata = new ChatUser(server, "Bedata");
            IUser alice = new ChatUser(server, "Alice");
            IUser bob = new ChatUser(server, "Bob");

            // ৩. সার্ভারে জয়েন করা
            server.RegisterUser(bedata);
            server.RegisterUser(alice);
            server.RegisterUser(bob);

            // ৪. চ্যাটিং
            bedata.Send("Hi everyone!");
            alice.Send("Hello Bedata!");
        }
    }
}
