using System;
using System.Collections.Generic;

namespace FlyweightPattern.PUBGGame
{
    // ==========================================
    // 1. Flyweight Interface (DIP)
    // ==========================================
    // এটি সেই ইন্টারফেস যার মাধ্যমে ফ্লাইওয়েটগুলো বাহিরের (Extrinsic) ইউনিক পজিশন রিসিভ করে।
    public interface ITreeFlyweight
    {
        void Render(int positionX, int positionY);
    }

    // ==========================================
    // 2. Concrete Flyweight (Shared Object - Intrinsic State)
    // ==========================================
    public class SharedTreeModel : ITreeFlyweight
    {
        public string TreeName { get; private set; }
        public string LeafColor { get; private set; }
        public string HeavyTextureFile { get; private set; } 

        public SharedTreeModel(string treeName, string leafColor, string heavyTextureFile)
        {
            TreeName = treeName;
            LeafColor = leafColor;
            HeavyTextureFile = heavyTextureFile;
            Console.WriteLine($"\n[Factory] Created new HEAVY SharedTreeModel in Memory: {TreeName} ({LeafColor}) - {HeavyTextureFile}");
        }

        public void Render(int positionX, int positionY)
        {
            Console.WriteLine($"Rendering a '{TreeName}' tree at coordinates ({positionX}, {positionY}) using shared 10MB texture.");
        }
    }

    // ==========================================
    // 3. Unshared Concrete Flyweight (Optional)
    // ==========================================
    public class UniqueBossTree : ITreeFlyweight
    {
        public void Render(int positionX, int positionY)
        {
            Console.WriteLine($"Rendering a SPECIAL BOSS Tree at coordinates ({positionX}, {positionY}) - This object is NOT shared.");
        }
    }


    // ==========================================
    // 4. Flyweight Factory & Interface
    // ==========================================
    public interface IFlyweightTreeFactory
    {
        ITreeFlyweight GetSharedTreeModel(string treeName, string leafColor, string textureFile);
        void PrintMemoryStats();
    }

    public class FlyweightTreeFactory : IFlyweightTreeFactory
    {
        // ক্যাশ (Cache) মেমোরি, যেখানে শেয়ার্ড মডেলগুলো রাখা হবে
        private Dictionary<string, ITreeFlyweight> _sharedTreeModels = new Dictionary<string, ITreeFlyweight>();

        public ITreeFlyweight GetSharedTreeModel(string treeName, string leafColor, string textureFile)
        {
            if (!_sharedTreeModels.ContainsKey(treeName))
            {
                _sharedTreeModels[treeName] = new SharedTreeModel(treeName, leafColor, textureFile);
            }
            return _sharedTreeModels[treeName];
        }

        public void PrintMemoryStats()
        {
            Console.WriteLine($"\n[Memory Stats] Total unique Shared Flyweights in Memory: {_sharedTreeModels.Count}");
        }
    }


    // ==========================================
    // 5. Context / Client Object & Interface
    // ==========================================
    public interface IGameObject
    {
        void RenderOnMap();
    }

    // এটি হলো গাছের মূল ইন্সট্যান্স, যা ম্যাপে বসবে। 
    // এটি শুধু নিজের পজিশন (X, Y) জানে এবং একটি শেয়ার্ড মডেলের রেফারেন্স ধরে রাখে।
    public class TreeInstance : IGameObject
    {
        private int _positionX;
        private int _positionY;
        private ITreeFlyweight _sharedFlyweightModel; 

        public TreeInstance(int positionX, int positionY, ITreeFlyweight sharedFlyweightModel)
        {
            _positionX = positionX;
            _positionY = positionY;
            _sharedFlyweightModel = sharedFlyweightModel;
        }

        public void RenderOnMap()
        {
            _sharedFlyweightModel.Render(_positionX, _positionY);
        }
    }


    // ==========================================
    // 6. Map / Forest (The high-level manager)
    // ==========================================
    public interface IForest
    {
        void PlantTree(int positionX, int positionY, string treeName, string leafColor, string textureFile);
        void PlantSpecialBossTree(int positionX, int positionY);
        void RenderEntireForest();
    }

    public class PubgForestMap : IForest
    {
        private List<IGameObject> _plantedTrees = new List<IGameObject>();
        private IFlyweightTreeFactory _treeFactory; 

        public PubgForestMap(IFlyweightTreeFactory treeFactory)
        {
            _treeFactory = treeFactory;
        }

        public void PlantTree(int positionX, int positionY, string treeName, string leafColor, string textureFile)
        {
            // ১. ফ্যাক্টরি থেকে শেয়ার্ড মডেল নিয়ে আসো
            ITreeFlyweight sharedModel = _treeFactory.GetSharedTreeModel(treeName, leafColor, textureFile);
            
            // ২. নতুন একটি TreeInstance বানাও
            IGameObject newTreeInstance = new TreeInstance(positionX, positionY, sharedModel);
            _plantedTrees.Add(newTreeInstance);
        }

        public void PlantSpecialBossTree(int positionX, int positionY)
        {
            ITreeFlyweight uniqueBossTreeModel = new UniqueBossTree();
            IGameObject newTreeInstance = new TreeInstance(positionX, positionY, uniqueBossTreeModel);
            _plantedTrees.Add(newTreeInstance);
        }

        public void RenderEntireForest()
        {
            Console.WriteLine("\n=== Rendering PUBG Erangel Map Forest ===");
            foreach (var tree in _plantedTrees)
            {
                tree.RenderOnMap();
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
            Console.WriteLine("=== Flyweight Design Pattern (Clean & Readable) ===\n");

            // ১০০% DIP Followed
            IFlyweightTreeFactory treeFactory = new FlyweightTreeFactory();
            IForest erangelForestMap = new PubgForestMap(treeFactory);

            // ম্যাপে Oak গাছ বসাচ্ছি (প্রথমবার 10MB মডেল তৈরি হবে, পরেরগুলো শেয়ার হবে)
            erangelForestMap.PlantTree(10, 20, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelForestMap.PlantTree(15, 30, "Oak", "Green", "10MB_Oak_Texture.png");
            erangelForestMap.PlantTree(50, 60, "Oak", "Green", "10MB_Oak_Texture.png");

            // ম্যাপে Pine গাছ বসাচ্ছি (প্রথমবার 8MB মডেল তৈরি হবে, পরেরগুলো শেয়ার হবে)
            erangelForestMap.PlantTree(100, 200, "Pine", "Dark Green", "8MB_Pine_Texture.png");
            erangelForestMap.PlantTree(110, 210, "Pine", "Dark Green", "8MB_Pine_Texture.png");

            // একটি স্পেশাল বস-ট্রি বসাচ্ছি (এটি শেয়ার হবে না)
            erangelForestMap.PlantSpecialBossTree(999, 999);

            // ম্যাপ রেন্ডার
            erangelForestMap.RenderEntireForest();

            // মেমোরি স্ট্যাটাস চেক!
            treeFactory.PrintMemoryStats();
        }
    }
}
