using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Iterator
{
    // ==========================================
    // 1. Iterator Interface (রিমোট কন্ট্রোলের নিয়ম)
    // ==========================================
    // এটি ঠিক করে দেয় কীভাবে ডেটা ট্রাভার্স (লুপ) করতে হবে।
    public interface IChannelIterator
    {
        void First();
        void Next();
        bool IsDone();
        string CurrentItem();
    }

    // ==========================================
    // 3. Aggregate Interface (টিভির রিড-অনলি নিয়ম - For Iterator)
    // ==========================================
    // এই ইন্টারফেসে AddChannel নেই। এটি শুধু Iterator এর জন্য। (ISP মানা হলো)
    public interface ITvChannelCollection
    {
        IChannelIterator CreateIterator();
        int Count { get; }
        string GetChannel(int index);
    }

    // ==========================================
    // 3.1 Manager Interface (চ্যানেল সেটআপের নিয়ম - For Client)
    // ==========================================
    // ক্লায়েন্ট এই ইন্টারফেস ব্যবহার করে চ্যানেল অ্যাড করবে। (DIP মানা হলো)
    public interface IChannelManager : ITvChannelCollection
    {
        void AddChannel(string channelName);
    }

    // ==========================================
    // 4. Concrete Aggregate (আসল টিভি বা চ্যানেল লিস্ট)
    // ==========================================
    public class TvChannelCollection : IChannelManager
    {
        // ভেতরে List ব্যবহার করা হয়েছে, কিন্তু ক্লায়েন্ট এটা জানবে না
        private readonly List<string> _channels = new List<string>();

        public void AddChannel(string channelName)
        {
            _channels.Add(channelName);
        }

        public int Count => _channels.Count;

        public string GetChannel(int index)
        {
            return _channels[index];
        }

        // ক্লায়েন্টকে ভেতরের List না দিয়ে শুধু রিমোট (Iterator) দেওয়া হচ্ছে
        public IChannelIterator CreateIterator()
        {
            return new TvRemoteControl(this);
        }
    }

    // ==========================================
    // 2. Concrete Iterator (আসল রিমোট কন্ট্রোল)
    // ==========================================
    public class TvRemoteControl : IChannelIterator
    {
        // সরাসরি কনক্রিট ক্লাসের বদলে ইন্টারফেসের ওপর ডিপেন্ড করছে (DIP)
        private readonly ITvChannelCollection _collection;
        private int _currentPosition = 0;

        public TvRemoteControl(ITvChannelCollection collection)
        {
            _collection = collection;
        }

        public void First()
        {
            _currentPosition = 0;
        }

        public void Next()
        {
            _currentPosition++;
        }

        public bool IsDone()
        {
            return _currentPosition >= _collection.Count;
        }

        public string CurrentItem()
        {
            return _collection.GetChannel(_currentPosition);
        }
    }

    // ==========================================
    // 5. Client (ইউজার)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Iterator Pattern (TV Remote Control) ===\n");

            // ১. টিভি কেনা হলো এবং চ্যানেল অ্যাড করা হলো
            // ক্লায়েন্ট IChannelManager ইন্টারফেস ব্যবহার করছে (DIP)
            IChannelManager myTv = new TvChannelCollection();
            myTv.AddChannel("BTV");
            myTv.AddChannel("Somoy TV");
            myTv.AddChannel("Channel I");
            myTv.AddChannel("Independent TV");

            // ২. টিভি থেকে রিমোট কন্ট্রোল (Iterator) নেওয়া হলো
            // ইউজার জানে না টিভির ভেতর চ্যানেলগুলো Array, List নাকি Tree তে আছে।
            // ইউজারের শুধু IChannelIterator দরকার।
            IChannelIterator remote = myTv.CreateIterator();

            // ৩. রিমোট দিয়ে এক এক করে চ্যানেল চেঞ্জ করা হচ্ছে
            Console.WriteLine("Surfing channels using the Remote Control:");
            remote.First(); // প্রথম চ্যানেলে যাও

            while (!remote.IsDone())
            {
                Console.WriteLine($"Watching: {remote.CurrentItem()}");
                remote.Next(); // পরের চ্যানেলে যাও
            }
        }
    }
}
