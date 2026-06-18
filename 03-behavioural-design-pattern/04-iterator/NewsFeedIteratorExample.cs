using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Iterator.NewsFeed
{
    // ==========================================
    // 1. Iterator Interface
    // ==========================================
    // এটি জাভার java.util.Iterator এর মতো। 
    public interface IPostIterator
    {
        bool HasNext();
        string Next();
    }

    // ==========================================
    // 3. Aggregate Interface (For Read-Only Iterator)
    // ==========================================
    public interface IProfileFeed
    {
        IPostIterator CreateIterator();
        
        // ধরুন এটি একটি ডাটাবেজ API যা পেজিনেশন (Pagination) সাপোর্ট করে
        List<string> GetPostsFromApi(int pageNumber, int pageSize);
    }

    // ==========================================
    // 3.1 Manager Interface (For Client / Admin)
    // ==========================================
    public interface IFeedManager : IProfileFeed
    {
        void AddPostToDatabase(string post);
    }

    // ==========================================
    // 4. Concrete Aggregate (আসল নিউজফিড ডাটাবেজ)
    // ==========================================
    public class FacebookFeed : IFeedManager
    {
        private readonly List<string> _database = new List<string>();

        public void AddPostToDatabase(string post)
        {
            _database.Add(post);
        }

        // রিয়েল ওয়ার্ল্ডে এখানে SQL/NoSQL কোয়েরি থাকে: SELECT * FROM Posts LIMIT 2 OFFSET 0
        public List<string> GetPostsFromApi(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            int take = Math.Min(pageSize, Math.Max(0, _database.Count - skip));
            
            if (take <= 0) return new List<string>(); // আর কোনো পোস্ট নেই
            
            Console.WriteLine($"\n[System Log] Fetched Page {pageNumber} from Database (Size: {take} posts)....");
            return _database.GetRange(skip, take);
        }

        public IPostIterator CreateIterator()
        {
            // আমরা পেজ সাইজ ২ করে দিচ্ছি, অর্থাৎ ইটারেটর একসাথে মাত্র ২টো পোস্ট লোড করবে
            return new PaginationIterator(this, pageSize: 2);
        }
    }

    // ==========================================
    // 2. Concrete Iterator (আসল ম্যাজিক: Pagination Hider)
    // ==========================================
    public class PaginationIterator : IPostIterator
    {
        private readonly IProfileFeed _feed;
        private readonly int _pageSize;
        private int _currentPage = 1;
        private int _currentPostIndex = 0;
        
        private List<string> _currentBatch; // বর্তমানে যে পেজটি লোড করা আছে

        public PaginationIterator(IProfileFeed feed, int pageSize)
        {
            _feed = feed;
            _pageSize = pageSize;
            // শুরুতে প্রথম পেজ লোড করে নিচ্ছি
            _currentBatch = _feed.GetPostsFromApi(_currentPage, _pageSize);
        }

        public bool HasNext()
        {
            // যদি কারেন্ট পেজে এখনো পোস্ট বাকি থাকে
            if (_currentPostIndex < _currentBatch.Count) 
            {
                return true;
            }

            // কারেন্ট পেজ শেষ! এবার চুপিচুপি ডাটাবেজ থেকে পরের পেজ নিয়ে আসো
            _currentPage++;
            _currentBatch = _feed.GetPostsFromApi(_currentPage, _pageSize);
            _currentPostIndex = 0;

            return _currentBatch.Count > 0;
        }

        public string Next()
        {
            string post = _currentBatch[_currentPostIndex];
            _currentPostIndex++;
            return post;
        }
    }

    // ==========================================
    // 5. Client
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Iterator Pattern (Social Media Feed Pagination) ===\n");

            // ১. ডাটাবেজে ৫টি পোস্ট অ্যাড করা হলো
            IFeedManager myFeed = new FacebookFeed();
            myFeed.AddPostToDatabase("Post 1: Hello World!");
            myFeed.AddPostToDatabase("Post 2: Eating breakfast 🍳");
            myFeed.AddPostToDatabase("Post 3: Going to office 🚗");
            myFeed.AddPostToDatabase("Post 4: Coding in C# 💻");
            myFeed.AddPostToDatabase("Post 5: Heading home 🏠");

            // ২. ইউজার স্ক্রল করার জন্য ইটারেটর (Scroller) নিল
            IProfileFeed readOnlyFeed = myFeed;
            IPostIterator feedScroller = readOnlyFeed.CreateIterator();

            Console.WriteLine("\nUser is scrolling the feed...");
            Console.WriteLine("-----------------------------");

            // ৩. ক্লায়েন্ট শুধু HasNext() আর Next() চেনে।
            // ক্লায়েন্ট জানেই না যে ইটারেটর ব্যাকগ্রাউন্ডে পেজিনেট (Paginate) করে ২টো ২টো করে ডেটা আনছে!
            while (feedScroller.HasNext())
            {
                Console.WriteLine($"[Phone Screen Shows] {feedScroller.Next()}");
                Console.WriteLine("   (User scrolls down...)");
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("No more posts to show.");
        }
    }
}
