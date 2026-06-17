using System;
using System.Collections.Generic;

namespace FlyweightPattern.PUBGGame
{
    // ==========================================
    // 1. Flyweight (The Shared Object - Intrinsic State)
    // ==========================================
    // এই ক্লাসটিতে শুধু কমন এবং ভারী ডেটা থাকবে। এটি একবারই তৈরি হবে।
    public class TreeType
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

        // Extrinsic State (X, Y) বাইরে থেকে পাস করতে হবে
        public void Draw(int x, int y)
        {
            Console.WriteLine($"Rendering a {Name} tree at ({x}, {y}) using shared 10MB texture.");
        }
    }


    // ==========================================
    // 2. Flyweight Factory (ম্যানেজার)
    // ==========================================
    // এর কাজ হলো ডুপ্লিকেট TreeType তৈরি হওয়া আটকানো (Caching)
    public class TreeFactory
    {
        private static Dictionary<string, TreeType> _treeTypes = new Dictionary<string, TreeType>();

        public static TreeType GetTreeType(string name, string color, string texture)
        {
            // যদি এই নামের গাছ মেমোরিতে অলরেডি থাকে, তবে পুরোনোটিই রিটার্ন করো (RAM বাঁচাও!)
            if (!_treeTypes.ContainsKey(name))
            {
                _treeTypes[name] = new TreeType(name, color, texture);
            }
            return _treeTypes[name];
        }

        public static void PrintMemoryStats()
        {
            Console.WriteLine($"\n[Memory Stats] Total unique Tree Types in Memory: {_treeTypes.Count}");
        }
    }


    // ==========================================
    // 3. Context / Client Object (Extrinsic State)
    // ==========================================
    // এই ক্লাসে শুধু ইউনিক ডেটা (X, Y) থাকবে। এটি খুবই হালকা ওজনের (Lightweight)।
    public class Tree
    {
        private int _x;
        private int _y;
        private TreeType _type; // রেফারেন্স (Reference) ধরে রাখছি

        public Tree(int x, int y, TreeType type)
        {
            _x = x;
            _y = y;
            _type = type;
        }

        public void Draw()
        {
            // ড্র করার সময় নিজের X, Y ফ্লাইওয়েটের কাছে পাস করে দিচ্ছি
            _type.Draw(_x, _y);
        }
    }


    // ==========================================
    // 4. Forest (যেখানে লাখ লাখ গাছ থাকবে)
    // ==========================================
    public class Forest
    {
        private List<Tree> _trees = new List<Tree>();

        public void PlantTree(int x, int y, string name, string color, string texture)
        {
            // ১. ফ্যাক্টরি থেকে কমন ফ্লাইওয়েট নিয়ে আসো (নতুন বানালে বানাবে, না হলে ক্যাশ থেকে দেবে)
            TreeType type = TreeFactory.GetTreeType(name, color, texture);
            
            // ২. একদম হালকা ওজনের একটি Tree অবজেক্ট বানাও
            Tree tree = new Tree(x, y, type);
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
    // 5. Client Code (Main Program)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Flyweight Design Pattern (PUBG Map Rendering) ===\n");

            Forest erangelMap = new Forest();

            // আমরা ম্যাপে ৫টি গাছ বসাচ্ছি
            // প্রথমবার যখন Oak বানাবো, তখন 10 MB এর ভারী অবজেক্ট তৈরি হবে।
            erangelMap.PlantTree(10, 20, "Oak", "Green", "10MB_Oak_Texture.png");
            
            // এরপর যতবার Oak বানাবো, আগেরটাই শেয়ার হবে! নতুন করে মেমোরি খাবে না।
            erangelMap.PlantTree(15, 30, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelMap.PlantTree(50, 60, "Oak", "Green", "10MB_Oak_Texture.png");

            // নতুন Pine গাছ, তাই এটি আবার একবার ভারী অবজেক্ট তৈরি করবে।
            erangelMap.PlantTree(100, 200, "Pine", "Dark Green", "8MB_Pine_Texture.png");
            erangelMap.PlantTree(110, 210, "Pine", "Dark Green", "8MB_Pine_Texture.png");

            // ম্যাপ রেন্ডার করা হলো
            erangelMap.RenderMap();

            // মেমোরি স্ট্যাটাস চেক!
            // ৫টি গাছের জন্য ৫টি ভারী মডেল তৈরি হয়নি, মাত্র ২টি (Oak এবং Pine) তৈরি হয়েছে!
            TreeFactory.PrintMemoryStats();
        }
    }
}
