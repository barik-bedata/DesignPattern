using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Visitor.ShapeArea
{
    // ==========================================
    // 1. Visitor Interface
    // ==========================================
    // ভিজিটরের কাজ কী হবে তা এখানে ডিফাইন করা হয়। সে কোন কোন ক্লাসে (Circle, Square) ভিজিট করতে পারবে, তার লিস্ট এখানে থাকে।
    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Square square);
        void Visit(Triangle triangle);
    }

    // ==========================================
    // 3. Element Interface
    // ==========================================
    // যাদের ভিজিট করা হবে, তাদের ইন্টারফেস। এদের কাজ শুধু ভিজিটরকে "Welcome" (Accept) করা।
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }

    // ==========================================
    // 4. Concrete Elements
    // ==========================================
    // এরা হলো রিয়েল অবজেক্ট। 
    // GFG এর কোডে একটি বড় ভুল ছিল—তারা Circle এর Radius ভিজিটরের ভেতরে হার্ডকোড করেছিল! 
    // SOLID (SRP) অনুযায়ী Circle তার নিজের Radius নিজে ধারণ করবে।
    public class Circle : IShape
    {
        public double Radius { get; } // Object's own property

        public Circle(double radius)
        {
            Radius = radius;
        }

        // Double Dispatch: ভিজিটর আসলে সে নিজেকে (this) ভিজিটরের হাতে তুলে দেয়।
        public void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this); 
        }
    }

    public class Square : IShape
    {
        public double Side { get; }

        public Square(double side)
        {
            Side = side;
        }

        public void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Triangle : IShape
    {
        public double BaseLength { get; }
        public double Height { get; }

        public Triangle(double baseLength, double height)
        {
            BaseLength = baseLength;
            Height = height;
        }

        public void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // ==========================================
    // 2. Concrete Visitor
    // ==========================================
    // এরা হলো সত্যিকারের ভিজিটর, যারা লজিক নিয়ে কাজ করে।
    // AreaCalculatorVisitor শুধু Area হিসাব করবে। অন্য ক্লাসে হাত না দিয়ে সে নিজের ভেতরেই সব হিসাব করে।
    public class AreaCalculatorVisitor : IShapeVisitor
    {
        public double TotalArea { get; private set; } = 0;

        // Best Practice: Reset method to clear state before reusing the visitor
        public void Reset()
        {
            TotalArea = 0;
            Console.WriteLine("[Visitor] State Reset. Total Area is now 0.");
        }

        public void Visit(Circle circle)
        {
            // Calculate area of circle using circle's actual property
            double area = Math.PI * Math.Pow(circle.Radius, 2);
            Console.WriteLine($"[Visitor] Calculated Area for Circle: {area:F2}");
            
            // Note: If we used `TotalArea = area` instead of `+=`, it would override the previous value
            // and we would only get the single area of the last visited shape.
            // Since we want the sum of all shapes in the list, we use `+=`.
            TotalArea += area;
        }

        public void Visit(Square square)
        {
            // Calculate area of square
            double area = Math.Pow(square.Side, 2);
            Console.WriteLine($"[Visitor] Calculated Area for Square: {area:F2}");
            TotalArea += area;
        }

        public void Visit(Triangle triangle)
        {
            // Calculate area of triangle
            double area = (triangle.BaseLength * triangle.Height) / 2;
            Console.WriteLine($"[Visitor] Calculated Area for Triangle: {area:F2}");
            TotalArea += area;
        }
    }

    // ==========================================
    // 5. Object Structure / Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Visitor Pattern (Shape Area Calculator) ===\n");

            // 1st Collection
            List<IShape> shapesList1 = new List<IShape>
            {
                new Circle(5),
                new Square(4)
            };

            // 2nd Collection
            List<IShape> shapesList2 = new List<IShape>
            {
                new Triangle(3, 6)
            };

            // Create the Visitor
            AreaCalculatorVisitor areaCalculator = new AreaCalculatorVisitor();

            Console.WriteLine("--- Processing List 1 ---");
            foreach (var shape in shapesList1)
            {
                shape.Accept(areaCalculator);
            }
            Console.WriteLine($"Total Area for List 1: {areaCalculator.TotalArea:F2}\n");

            Console.WriteLine("--- Processing List 2 (Using Reset) ---");
            // BEST PRACTICE: Reset state before reusing the visitor on a new collection!
            areaCalculator.Reset(); 
            
            foreach (var shape in shapesList2)
            {
                shape.Accept(areaCalculator);
            }
            Console.WriteLine($"Total Area for List 2: {areaCalculator.TotalArea:F2}");
        }
    }
}
