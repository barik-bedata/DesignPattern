using System;
using System.Collections.Generic;

namespace ProxyPattern.InternetAccess
{
    // ==========================================
    // 1. Subject Interface (DIP Followed)
    // ==========================================
    // ক্লায়েন্ট (অফিসের কর্মী) শুধু এই ইন্টারফেসটিকে চিনবে। 
    // সে জানবেও না সে কি আসল ইন্টারনেট ব্যবহার করছে নাকি প্রক্সি!
    public interface IInternet
    {
        void ConnectTo(string serverHost);
    }


    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি হলো আসল ইন্টারনেট সার্ভিস। এর কোনো রেস্ট্রিকশন নেই, সে সব জায়গায় কানেক্ট করতে পারে।
    public class RealInternet : IInternet
    {
        public void ConnectTo(string serverHost)
        {
            // আসল কাজটা এখানেই হচ্ছে
            Console.WriteLine($"[Real Internet] Successfully connected to {serverHost}");
        }
    }


    // ==========================================
    // 3. Proxy
    // ==========================================
    // এটি আসল ইন্টারনেটের প্রতিনিধি। এটি রিকোয়েস্ট ফিল্টার (Protection Proxy) করে।
    public class ProxyInternet : IInternet
    {
        // প্রক্সির ভেতরে রিয়েল অবজেক্টের রেফারেন্স আছে (Composition)
        private IInternet _realInternet; 
        private List<string> _bannedSites;

        // Constructor Injection (DIP)
        public ProxyInternet(IInternet realInternet)
        {
            _realInternet = realInternet;

            // কোম্পানির ব্যান করা সাইটগুলোর লিস্ট
            _bannedSites = new List<string> 
            { 
                "facebook.com", 
                "youtube.com", 
                "netflix.com",
                "instagram.com"
            };
        }

        public void ConnectTo(string serverHost)
        {
            string hostLower = serverHost.ToLower();

            // প্রক্সির নিজস্ব লজিক (অ্যাক্সেস কন্ট্রোল)
            if (_bannedSites.Contains(hostLower))
            {
                Console.WriteLine($"[Proxy] Access Denied: Connection to '{serverHost}' is BLOCKED by company policy!");
            }
            else
            {
                // যদি সাইট ব্যানড না হয়, তবে রিকোয়েস্টটি রিয়েল ইন্টারনেটের কাছে পাঠিয়ে দাও (Delegation)
                Console.WriteLine($"[Proxy] Request allowed for '{serverHost}'. Forwarding to Real Internet...");
                _realInternet.ConnectTo(serverHost);
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
            Console.WriteLine("=== Proxy Design Pattern (Protection Proxy) ===\n");

            // ১. রিয়েল সাবজেক্ট তৈরি করা হলো
            IInternet realInternet = new RealInternet();

            // ২. রিয়েল সাবজেক্টকে প্রক্সির ভেতরে ঢুকিয়ে দেওয়া হলো (Dependency Injection)
            // ক্লায়েন্ট (অফিস কর্মী) এখন শুধু প্রক্সির সাথে কানেক্টেড থাকবে।
            IInternet officeNetwork = new ProxyInternet(realInternet);

            // অফিসের কর্মী বিভিন্ন সাইটে ঢোকার চেষ্টা করছে:
            
            // Educational / Dev sites
            officeNetwork.ConnectTo("stackoverflow.com");
            Console.WriteLine();
            
            officeNetwork.ConnectTo("github.com");
            Console.WriteLine();

            // Entertainment sites (Should be blocked by Proxy)
            officeNetwork.ConnectTo("youtube.com");
            Console.WriteLine();

            officeNetwork.ConnectTo("Facebook.com");
        }
    }
}
