# Decorator Design Pattern (ডেকোরেটর প্যাটার্ন)

**Decorator Pattern** এমন একটি প্যাটার্ন যা আপনাকে বিদ্যমান কোনো অবজেক্টের মূল কোড (Core structure) পরিবর্তন না করেই, রানটাইমে (প্রোগ্রাম চলাকালীন) তার মধ্যে নতুন নতুন বৈশিষ্ট্য (Behavior) বা দায়িত্ব যুক্ত করার সুযোগ দেয়। এটি এক ধরনের "র‍্যাপার" (Wrapper) বা মোড়ক হিসেবে কাজ করে।

টেকনিক্যাল কথা বাদ দিয়ে চলুন আগে একটা মজার গল্প শুনে আসি!

---

## গল্পের মাধ্যমে Decorator Pattern (The Pizza Shop)

ধরুন, আপনি ডমিনোজে (Domino's) পিৎজা খেতে গেছেন।
মেনুতে একটি সাধারণ **Basic Pizza** আছে, যার দাম ৫০০ টাকা। আপনি সেটা অর্ডার করলেন।

হঠাৎ আপনার মনে হলো, "একটু এক্সট্রা চিজ (Cheese) হলে ভালো হতো!" 
আপনি বয়কে বললেন, "ভাই, এর সাথে এক্সট্রা চিজ অ্যাড করে দিন।" 
বয় আপনাকে একটি নতুন পিৎজা বানিয়ে দিল না, বরং আপনার ওই বেসিক পিৎজার ওপরেই এক স্তর চিজ ছড়িয়ে দিলো (Wrap করে দিলো)। দাম হয়ে গেলো ৫০০ + ৫০ = ৫৫০ টাকা।

আপনার বন্ধুর আবার চিকেন খুব পছন্দ। সে বললো, "ভাই, এক্সট্রা চিকেনও অ্যাড করে দিন।"
বয় তখন আপনার ওই চিজ দেওয়া পিৎজার ওপরেই আবার এক স্তর চিকেন ছড়িয়ে দিলো! 
দাম হয়ে গেলো ৫৫০ + ১০০ = ৬৫০ টাকা।

**এখানে কী ঘটলো খেয়াল করুন:** 
আপনি কিন্তু নতুন কোনো `PizzaWithCheeseAndChicken` নামে ক্লাস বা পিৎজা তৈরি করেননি। আপনি একটি বেসিক পিৎজা নিয়েছেন, তারপর সেটার ওপর আপনার ইচ্ছামতো এক্সট্রা জিনিস **"ডেকোরেট" (Decorate)** করেছেন বা মুড়িয়ে দিয়েছেন। কালকে যদি আপনি "Extra Spicy" চান, শুধু আরেকটা লেয়ার অ্যাড করে দিলেই হবে!

---

## এবার গল্পের সাথে কম্পোনেন্টগুলো মিলিয়ে নিই!

1. **Component Interface (কমন ইন্টারফেস):**
   গল্পের **"Pizza"** হলো Component Interface। এর কাছে `GetDescription()` এবং `GetCost()` নামে দুটি কমন মেথড আছে। পিৎজা হোক বা এক্সট্রা চিজ—সবাইকেই এই নিয়ম মানতে হবে।

2. **Concrete Component (কোর অবজেক্ট):**
   গল্পের **"Basic Pizza"** হলো Concrete Component। এটি হলো মূল জিনিস! এর ডেসক্রিপশন হলো "Basic Pizza" এবং দাম ৫০০ টাকা।

3. **Decorator (র‍্যাপার / মোড়ক):**
   এটি হলো একটি স্পেশাল ক্লাস, যার ভেতরে পিৎজা ঢোকানো যায়। অর্থাৎ, সে নিজে একটি পিৎজা (Inheritance), আবার তার ভেতরেও একটি পিৎজা থাকে (Composition)! সে ক্লায়েন্টের কল রিসিভ করে এবং ভেতরের পিৎজার কাছে পাঠিয়ে দেয়।

4. **Concrete Decorator (নির্দিষ্ট ডেকোরেটর):**
   গল্পের **"Extra Cheese"** বা **"Extra Chicken"** হলো Concrete Decorator। এরা মূল পিৎজার মেথড কল করে এবং তার সাথে নিজেদের এক্সট্রা কাজ (যেমন: +৫০ টাকা যোগ করা) করে দেয়।

---

## C# কোড দিয়ে উদাহরণ

```csharp
using System;

namespace DecoratorPatternExample
{
    // ১. Component Interface (সবাইকে এই নিয়ম মানতে হবে)
    public interface IPizza
    {
        string GetDescription();
        double GetCost();
    }

    // ২. Concrete Component (আমাদের মূল বেসিক পিৎজা)
    public class BasicPizza : IPizza
    {
        public string GetDescription() => "Basic Pizza";
        public double GetCost() => 500.00;
    }

    // ৩. Decorator (Abstract Wrapper - এর কাছে Composition এবং Inheritance দুটোই আছে!)
    public abstract class PizzaDecorator : IPizza
    {
        // Composition (Has-a): এর ভেতরে একটি পিৎজা আছে
        protected IPizza _pizza;

        public PizzaDecorator(IPizza pizza)
        {
            _pizza = pizza;
        }

        // ডিফল্ট কাজ হলো ভেতরের পিৎজাকে কল করে দেওয়া (Delegation)
        public virtual string GetDescription() => _pizza.GetDescription();
        public virtual double GetCost() => _pizza.GetCost();
    }

    // ৪. Concrete Decorators (নির্দিষ্ট এক্সট্রা লেয়ারগুলো)

    // এক্সট্রা চিজ
    public class ExtraCheeseDecorator : PizzaDecorator
    {
        public ExtraCheeseDecorator(IPizza pizza) : base(pizza) { }

        public override string GetDescription()
        {
            // মূল পিৎজার ডেসক্রিপশনের সাথে নিজেরটা জুড়ে দিচ্ছে
            return base.GetDescription() + ", with Extra Cheese";
        }

        public override double GetCost()
        {
            // মূল পিৎজার দামের সাথে নিজের দাম (৫০ টাকা) যোগ করছে
            return base.GetCost() + 50.00;
        }
    }

    // এক্সট্রা চিকেন
    public class ExtraChickenDecorator : PizzaDecorator
    {
        public ExtraChickenDecorator(IPizza pizza) : base(pizza) { }

        public override string GetDescription()
        {
            return base.GetDescription() + ", with Extra Chicken";
        }

        public override double GetCost()
        {
            return base.GetCost() + 100.00;
        }
    }

    // ৫. Client
    class Program
    {
        static void Main()
        {
            // ১. কাস্টমার শুধু বেসিক পিৎজা অর্ডার করলো
            IPizza myPizza = new BasicPizza();
            Console.WriteLine($"Order 1: {myPizza.GetDescription()} | Cost: BDT {myPizza.GetCost()}");

            // ২. কাস্টমার বললো, এক্সট্রা চিজ দিন! 
            // (বেসিক পিৎজাকে চিজ ডেকোরেটরের ভেতরে পাস করা হলো / মোড়ক দেওয়া হলো)
            myPizza = new ExtraCheeseDecorator(myPizza);
            Console.WriteLine($"Order 2: {myPizza.GetDescription()} | Cost: BDT {myPizza.GetCost()}");

            // ৩. কাস্টমার বললো, এক্সট্রা চিকেনও দিন!
            // (চিজ পিৎজাকে আবার চিকেন ডেকোরেটরের ভেতরে পাস করা হলো / ডাবল মোড়ক)
            myPizza = new ExtraChickenDecorator(myPizza);
            Console.WriteLine($"Order 3: {myPizza.GetDescription()} | Cost: BDT {myPizza.GetCost()}");
        }
    }
}
```

## মূল কথা (Summary)
Decorator Pattern এর সবচেয়ে বড় সুবিধা হলো, এটি **Open/Closed Principle (OCP)** খুব দারুণভাবে মেনে চলে। 
কালকে যদি দোকানে নতুন কোনো টপিং (যেমন: Extra Mushroom) আসে, আপনাকে পুরোনো কোনো কোড চেঞ্জ করতে হবে না! শুধু `ExtraMushroomDecorator` নামে নতুন একটি ক্লাস বানাবেন এবং বেসিক পিৎজার গায়ে মুড়িয়ে (Wrap) দেবেন!
