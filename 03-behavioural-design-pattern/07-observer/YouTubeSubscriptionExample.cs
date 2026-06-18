using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Observer.YouTube
{
    // ==========================================
    // 1. Observer Interface (Subscriber)
    // ==========================================
    public interface ISubscriber
    {
        void ReceiveNotification(string channelName, string videoTitle);
    }

    // ==========================================
    // 2. Subject Interface (Channel)
    // ==========================================
    public interface IYouTubeChannel
    {
        void Subscribe(ISubscriber subscriber);
        void Unsubscribe(ISubscriber subscriber);
        void UploadVideo(string videoTitle);
    }

    // ==========================================
    // 3. Concrete Subject (আসল ইউটিউব চ্যানেল)
    // ==========================================
    public class YouTubeChannel : IYouTubeChannel
    {
        private readonly List<ISubscriber> _subscribers = new List<ISubscriber>();
        public string ChannelName { get; }

        public YouTubeChannel(string name)
        {
            ChannelName = name;
        }

        public void Subscribe(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
            Console.WriteLine($"[YouTube] Someone subscribed to '{ChannelName}'. Total: {_subscribers.Count}");
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
            Console.WriteLine($"[YouTube] Someone unsubscribed from '{ChannelName}'.");
        }

        public void UploadVideo(string videoTitle)
        {
            Console.WriteLine($"\n[{ChannelName}] Uploaded new video: '{videoTitle}'");
            NotifySubscribers(videoTitle); // ভিডিও আপলোড হলেই সবাইকে জানাচ্ছে
        }

        private void NotifySubscribers(string videoTitle)
        {
            foreach (var sub in _subscribers)
            {
                sub.ReceiveNotification(ChannelName, videoTitle);
            }
        }
    }

    // ==========================================
    // 4. Concrete Observer (আসল ইউজার)
    // ==========================================
    public class UserSubscriber : ISubscriber
    {
        public string Name { get; }

        public UserSubscriber(string name)
        {
            Name = name;
        }

        public void ReceiveNotification(string channelName, string videoTitle)
        {
            Console.WriteLine($"  -> Hey {Name}, '{channelName}' just uploaded: {videoTitle}");
        }
    }

    // ==========================================
    // 5. Client
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Observer Pattern (YouTube Subscription) ===\n");

            // ১. চ্যানেল তৈরি করা হলো (Subject)
            IYouTubeChannel techChannel = new YouTubeChannel("Tech With Bedata");

            // ২. ইউজাররা সাবস্ক্রাইব করলো (Observers)
            ISubscriber user1 = new UserSubscriber("Alice");
            ISubscriber user2 = new UserSubscriber("Bob");
            ISubscriber user3 = new UserSubscriber("Charlie");

            techChannel.Subscribe(user1);
            techChannel.Subscribe(user2);
            techChannel.Subscribe(user3);

            // ৩. প্রথম ভিডিও আপলোড হলো!
            techChannel.UploadVideo("SOLID Principles in C#");

            // ৪. একজন আনসাবস্ক্রাইব করলো
            techChannel.Unsubscribe(user2);

            // ৫. দ্বিতীয় ভিডিও আপলোড হলো (এবার Bob নোটিফিকেশন পাবে না)
            techChannel.UploadVideo("Observer Pattern Explained");
        }
    }
}
