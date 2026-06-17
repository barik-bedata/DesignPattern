using System;
using System.Collections.Generic;

namespace FlyweightPattern.SimpleShapes
{
    // ==========================================
    // ১. Flyweight Interface
    // ==========================================
    // Extrinsic State (x, y, radius) গুলো মেথডের প্যারামিটার হিসেবে আসবে।
    public interface IShape
    {
        void Draw(int x, int y, int radius);
    }


    // ==========================================
    // ২. Concrete Flyweight (Shared Object)
    // ==========================================
    // Intrinsic State (Color) এর ভেতরে সেভ থাকবে। এটি মেমোরিতে একবারই তৈরি হবে।
    public class Circle : IShape
    {
        private string _color; // Intrinsic State (Shared)

        public Circle(string color)
        {
            this._color = color;
            Console.WriteLine($"\n[Factory] Created a new {_color} Circle object in memory.");
        }

        public void Draw(int x, int y, int radius)
        {
            Console.WriteLine($"Drawing a {_color} Circle at ({x}, {y}) with radius {radius}");
        }
    }


    // ==========================================
    // ৩. Flyweight Factory (ম্যানেজার)
    // ==========================================
    // ডুপ্লিকেট অবজেক্ট তৈরি হওয়া আটকানোর জন্য (Caching)
    public class ShapeFactory
    {
        private Dictionary<string, IShape> _circleMap = new Dictionary<string, IShape>();

        public IShape GetCircle(string color)
        {
            // যদি এই রঙের সার্কেল মেমোরিতে অলরেডি থাকে, তবে পুরোনোটিই রিটার্ন করো
            if (!_circleMap.ContainsKey(color))
            {
                _circleMap[color] = new Circle(color);
            }
            return _circleMap[color];
        }

        public void PrintMemoryStats()
        {
            Console.WriteLine($"\n[Memory Stats] Total unique Circle objects created in RAM: {_circleMap.Count}");
        }
    }


    // ==========================================
    // ৪. Client Code (Main Program)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Flyweight Design Pattern (Simplest Example) ===\n");

            ShapeFactory factory = new ShapeFactory();

            // লাল রঙের সার্কেল চাচ্ছি। ফ্যাক্টরি নতুন অবজেক্ট বানাবে।
            IShape redCircle1 = factory.GetCircle("Red");
            redCircle1.Draw(10, 10, 50);

            // আবার লাল রঙের সার্কেল চাচ্ছি। ফ্যাক্টরি মেমোরি থেকে আগেরটাই দেবে! (কোনো নতুন অবজেক্ট তৈরি হবে না)
            IShape redCircle2 = factory.GetCircle("Red");
            redCircle2.Draw(20, 30, 40);

            // তৃতীয়বার লাল রঙের সার্কেল। সেই আগেরটাই রিটার্ন করবে!
            IShape redCircle3 = factory.GetCircle("Red");
            redCircle3.Draw(100, 150, 10);

            // নীল রঙের সার্কেল। ফ্যাক্টরি এবার নতুন অবজেক্ট বানাবে।
            IShape blueCircle1 = factory.GetCircle("Blue");
            blueCircle1.Draw(5, 5, 20);

            // আবার নীল রঙের সার্কেল। ফ্যাক্টরি আগেরটাই দেবে!
            IShape blueCircle2 = factory.GetCircle("Blue");
            blueCircle2.Draw(50, 50, 100);

            // রেজাল্ট: আমরা ৫ বার সার্কেল ড্র করেছি, কিন্তু মেমোরিতে অবজেক্ট তৈরি হয়েছে মাত্র ২টি (Red এবং Blue)!
            factory.PrintMemoryStats();
        }
    }
}
