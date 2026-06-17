using System;
using System.Collections.Generic;

namespace FlyweightPattern.PUBGGame
{
    // ==========================================
    // 1. Flyweight Interface
    // ==========================================
    // এটি সেই ইন্টারফেস যার মাধ্যমে ফ্লাইওয়েটগুলো বাহিরের (Extrinsic) ডেটা রিসিভ করে।
    public interface ITreeFlyweight
    {
        void Draw(int x, int y);
    }

    // ==========================================
    // 2. Concrete Flyweight (Shared Object - Intrinsic State)
    // ==========================================
    // এই ক্লাসটিতে শুধু কমন এবং ভারী ডেটা থাকবে। এটি একবারই তৈরি হবে।
    public class TreeType : ITreeFlyweight
    {
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string TextureData { get; private set; } // বিশাল 3D মডেল ডেটা (Heavy!)

        public TreeType(string name, string color, string textureData)
        {
            Name = name;
            Color = color;
            TextureData = textureData;
            Console.WriteLine($"\n[Factory] Created new HEAVY TreeType in Memory: {Name} ({Color}) - {TextureData}");
        }

        // Extrinsic State (X, Y) মেথডের প্যারামিটার দিয়ে পাস করা হচ্ছে
        public void Draw(int x, int y)
        {
            Console.WriteLine($"Rendering a {Name} tree at ({x}, {y}) using shared 10MB texture.");
        }
    }

    // ==========================================
    // 3. Unshared Concrete Flyweight (ঐচ্ছিক)
    // ==========================================
    // সব ফ্লাইওয়েট যে শেয়ার্ড হতে হবে এমন কোনো কথা নেই। কোনো স্পেশাল ট্রি থাকলে সেটা শেয়ার্ড হবে না।
    public class SpecialBossTree : ITreeFlyweight
    {
        public void Draw(int x, int y)
        {
            Console.WriteLine($"Rendering a SPECIAL BOSS Tree at ({x}, {y}) - This object is NOT shared.");
        }
    }


    // ==========================================
    // 4. Flyweight Factory (ম্যানেজার)
    // ==========================================
    // এর কাজ হলো ডুপ্লিকেট ConcreteFlyweight তৈরি হওয়া আটকানো (Caching)
    public class TreeFactory
    {
        private static Dictionary<string, ITreeFlyweight> _flyweights = new Dictionary<string, ITreeFlyweight>();

        public static ITreeFlyweight GetTreeType(string name, string color, string texture)
        {
            // যদি এই নামের গাছ মেমোরিতে অলরেডি থাকে, তবে পুরোনোটিই রিটার্ন করো (RAM বাঁচাও!)
            if (!_flyweights.ContainsKey(name))
            {
                _flyweights[name] = new TreeType(name, color, texture);
            }
            return _flyweights[name];
        }

        public static void PrintMemoryStats()
        {
            Console.WriteLine($"\n[Memory Stats] Total unique Shared Flyweights in Memory: {_flyweights.Count}");
        }
    }


    // ==========================================
    // 5. Client (Context) Object
    // ==========================================
    // এই ক্লাসে শুধু ইউনিক ডেটা (X, Y) থাকবে। এটি খুবই হালকা ওজনের (Lightweight)।
    public class Tree
    {
        private int _x;
        private int _y;
        
        // DIP মেনে ইন্টারফেসের রেফারেন্স ধরে রাখছি, কোনো কংক্রিট ক্লাসের নয়!
        private ITreeFlyweight _flyweightType; 

        public Tree(int x, int y, ITreeFlyweight flyweightType)
        {
            _x = x;
            _y = y;
            _flyweightType = flyweightType;
        }

        public void Draw()
        {
            // ড্র করার সময় নিজের X, Y ফ্লাইওয়েটের কাছে পাস করে দিচ্ছি
            _flyweightType.Draw(_x, _y);
        }
    }


    // ==========================================
    // 6. Forest (কোটি কোটি গাছ রাখার জায়গা)
    // ==========================================
    public class Forest
    {
        private List<Tree> _trees = new List<Tree>();

        public void PlantTree(int x, int y, string name, string color, string texture)
        {
            // ১. ফ্যাক্টরি থেকে কমন ফ্লাইওয়েট নিয়ে আসো (ইন্টারফেস রিটার্ন করবে)
            ITreeFlyweight type = TreeFactory.GetTreeType(name, color, texture);
            
            // ২. একদম হালকা ওজনের একটি Tree অবজেক্ট বানাও
            Tree tree = new Tree(x, y, type);
            _trees.Add(tree);
        }

        public void PlantSpecialTree(int x, int y)
        {
            // Unshared ফ্লাইওয়েট সরাসরি 'new' করে বানাচ্ছি, কারণ এটা শেয়ার হবে না।
            ITreeFlyweight specialType = new SpecialBossTree();
            Tree tree = new Tree(x, y, specialType);
            _trees.Add(tree);
        }

        public void RenderMap()
        {
            Console.WriteLine("\n=== Rendering PUBG Map ===");
            foreach (var tree in _trees)
            {
                tree.Draw();
            }
        }
    }


    // ==========================================
    // Client Code (Main Program)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Flyweight Design Pattern (PUBG Map Rendering) ===\n");

            Forest erangelMap = new Forest();

            // আমরা ম্যাপে গাছ বসাচ্ছি
            // প্রথমবার যখন Oak বানাবো, তখন 10 MB এর ভারী অবজেক্ট তৈরি হবে।
            erangelMap.PlantTree(10, 20, "Oak", "Green", "10MB_Oak_Texture.png");
            
            // এরপর যতবার Oak বানাবো, আগেরটাই শেয়ার হবে! নতুন করে মেমোরি খাবে না।
            erangelMap.PlantTree(15, 30, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelMap.PlantTree(50, 60, "Oak", "Green", "10MB_Oak_Texture.png");

            // নতুন Pine গাছ, তাই এটি আবার একবার ভারী অবজেক্ট তৈরি করবে।
            erangelMap.PlantTree(100, 200, "Pine", "Dark Green", "8MB_Pine_Texture.png");
            erangelMap.PlantTree(110, 210, "Pine", "Dark Green", "8MB_Pine_Texture.png");

            // Unshared Flyweight (যেটি শেয়ার হবে না)
            erangelMap.PlantSpecialTree(999, 999);

            // ম্যাপ রেন্ডার করা হলো
            erangelMap.RenderMap();

            // মেমোরি স্ট্যাটাস চেক!
            TreeFactory.PrintMemoryStats();
        }
    }
}
