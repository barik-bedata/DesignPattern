using System;
using System.Collections.Generic;

namespace FlyweightPattern.PUBGGame
{
    // ==========================================
    // 1. Flyweight Interface (DIP)
    // ==========================================
    // এটি সেই ইন্টারফেস যার মাধ্যমে ফ্লাইওয়েটগুলো বাহিরের (Extrinsic) ডেটা রিসিভ করে।
    public interface ITreeFlyweight
    {
        void Draw(int x, int y);
    }

    // ==========================================
    // 2. Concrete Flyweight (Shared Object - Intrinsic State)
    // ==========================================
    public class TreeType : ITreeFlyweight
    {
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string TextureData { get; private set; } 

        public TreeType(string name, string color, string textureData)
        {
            Name = name;
            Color = color;
            TextureData = textureData;
            Console.WriteLine($"\n[Factory] Created new HEAVY TreeType in Memory: {Name} ({Color}) - {TextureData}");
        }

        public void Draw(int x, int y)
        {
            Console.WriteLine($"Rendering a {Name} tree at ({x}, {y}) using shared 10MB texture.");
        }
    }

    // ==========================================
    // 3. Unshared Concrete Flyweight (Optional)
    // ==========================================
    public class SpecialBossTree : ITreeFlyweight
    {
        public void Draw(int x, int y)
        {
            Console.WriteLine($"Rendering a SPECIAL BOSS Tree at ({x}, {y}) - This object is NOT shared.");
        }
    }


    // ==========================================
    // 4. Flyweight Factory & Interface
    // ==========================================
    // ফ্যাক্টরির জন্যও ইন্টারফেস (DIP)
    public interface ITreeFactory
    {
        ITreeFlyweight GetTreeType(string name, string color, string texture);
        void PrintMemoryStats();
    }

    public class TreeFactory : ITreeFactory
    {
        // Static ফিল্ড সরিয়ে Instance ফিল্ড ব্যবহার করছি যেন টেস্টেবিলিটি বাড়ে
        private Dictionary<string, ITreeFlyweight> _flyweights = new Dictionary<string, ITreeFlyweight>();

        public ITreeFlyweight GetTreeType(string name, string color, string texture)
        {
            if (!_flyweights.ContainsKey(name))
            {
                _flyweights[name] = new TreeType(name, color, texture);
            }
            return _flyweights[name];
        }

        public void PrintMemoryStats()
        {
            Console.WriteLine($"\n[Memory Stats] Total unique Shared Flyweights in Memory: {_flyweights.Count}");
        }
    }


    // ==========================================
    // 5. Context / Client Object & Interface
    // ==========================================
    // Tree ক্লাসের জন্যও ইন্টারফেস (DIP)
    public interface IGameObject
    {
        void Draw();
    }

    public class Tree : IGameObject
    {
        private int _x;
        private int _y;
        private ITreeFlyweight _flyweightType; 

        public Tree(int x, int y, ITreeFlyweight flyweightType)
        {
            _x = x;
            _y = y;
            _flyweightType = flyweightType;
        }

        public void Draw()
        {
            _flyweightType.Draw(_x, _y);
        }
    }


    // ==========================================
    // 6. Map / Forest (The high-level manager)
    // ==========================================
    // Forest ক্লাসের জন্যও ইন্টারফেস (DIP)
    public interface IForest
    {
        void PlantTree(int x, int y, string name, string color, string texture);
        void PlantSpecialTree(int x, int y);
        void RenderMap();
    }

    public class Forest : IForest
    {
        private List<IGameObject> _trees = new List<IGameObject>();
        private ITreeFactory _factory; // Dependency Injection

        // Constructor Injection for Factory!
        public Forest(ITreeFactory factory)
        {
            _factory = factory;
        }

        public void PlantTree(int x, int y, string name, string color, string texture)
        {
            ITreeFlyweight type = _factory.GetTreeType(name, color, texture);
            IGameObject tree = new Tree(x, y, type);
            _trees.Add(tree);
        }

        public void PlantSpecialTree(int x, int y)
        {
            ITreeFlyweight specialType = new SpecialBossTree();
            IGameObject tree = new Tree(x, y, specialType);
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
            Console.WriteLine("=== Flyweight Design Pattern (With 100% DIP) ===\n");

            // Client এখন কোনো কংক্রিট ক্লাসের ওপর নির্ভর করছে না। 
            // সবকিছুই ইন্টারফেসের মাধ্যমে হচ্ছে!
            ITreeFactory factory = new TreeFactory();
            IForest erangelMap = new Forest(factory);

            // ম্যাপে গাছ বসাচ্ছি
            erangelMap.PlantTree(10, 20, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelMap.PlantTree(15, 30, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelMap.PlantTree(50, 60, "Oak", "Green", "10MB_Oak_Texture.png");

            erangelMap.PlantTree(100, 200, "Pine", "Dark Green", "8MB_Pine_Texture.png");
            erangelMap.PlantTree(110, 210, "Pine", "Dark Green", "8MB_Pine_Texture.png");

            // Unshared Flyweight
            erangelMap.PlantSpecialTree(999, 999);

            // ম্যাপ রেন্ডার
            erangelMap.RenderMap();

            // মেমোরি স্ট্যাটাস চেক!
            factory.PrintMemoryStats();
        }
    }
}
