# Bridge Design Pattern (ব্রিজ প্যাটার্ন)

মিডিয়ামের ওই দারুণ আর্টিকেলটি থেকে আমরা বুঝতে পারি যে, **Bridge Pattern** এর সংজ্ঞায় থাকা "Abstraction" এবং "Implementation" শব্দগুলো অনেক সময় আমাদের কনফিউজ করে দেয়। 

সহজ কথায় (Without Bullsh*t): 
**Bridge Pattern এমন একটি ডিজাইন প্যাটার্ন যা একটি বড় ক্লাসের কাজ বা দায়িত্বকে দুটি আলাদা ক্লাস হায়ারার্কিতে (Hierarchy) ভাগ করে দেয়, যেন তারা স্বাধীনভাবে কাজ করতে পারে।** এখানে Inheritance এর বদলে **Composition** ব্যবহার করা হয়।

---

## সমস্যা (The Problem)
ধরুন আপনার বস আপনাকে একটি কাজ দিলো: সিস্টেম থেকে **User** দের ডেটা **CSV** ফাইলে এক্সপোর্ট করতে হবে। 
আপনি একটি ক্লাস বানালেন: `CsvUserExporter`। 

পরদিন বস বললো, **Company** এবং **Product** এর ডেটাও CSV তে এক্সপোর্ট করতে হবে। 
আপনি আরও দুটি ক্লাস বানালেন: `CsvCompanyExporter`, `CsvProductExporter`। 

কিছুদিন পর বস বললো, শুধু CSV নয়, এখন থেকে এই ডেটাগুলো **JSON** এবং **XML** ফরম্যাটেও এক্সপোর্ট করতে হবে!
এখন যদি আপনি ইনহেরিট্যান্স (Inheritance) ব্যবহার করেন, তবে আপনার ক্লাসের সংখ্যা জ্যামিতিক হারে বাড়বে:
`CsvUserExporter`, `JsonUserExporter`, `XmlUserExporter`, `CsvCompanyExporter`, `JsonCompanyExporter`... ইত্যাদি। 

সূত্র: `(Entity এর সংখ্যা × Format এর সংখ্যা) = ৩ × ৩ = ৯টি ক্লাস!`

## সমাধান (The Bridge Solution)
এই ক্লাস এক্সপ্লোশন (Class Explosion) কমানোর জন্যই আসে **Bridge Pattern**। আমরা এখানে দুটি আলাদা হায়ারার্কি তৈরি করবো এবং তাদের মধ্যে একটি "সেতু" (Bridge) বা কম্পোজিশন তৈরি করবো।

১. **Abstraction (কী এক্সপোর্ট করবো?):** User, Company, Product (এরা ডেটা আনবে)
২. **Implementation (কীভাবে ফরম্যাট করবো?):** CSV, JSON, XML (এরা ডেটা ফরম্যাট করবে)

সূত্র: `(Entity এর সংখ্যা + Format এর সংখ্যা) = ৩ + ৩ = ৬টি ক্লাস!`

---

## কম্পোনেন্টগুলো (Components of Bridge Pattern)
আর্টিকেল এবং থিওরি অনুযায়ী এর ৪টি মূল কম্পোনেন্ট থাকে:

1. **Abstraction:** এটি মূল কন্ট্রোলার। এটি ক্লায়েন্টের সাথে কথা বলে এবং কাজের দায়িত্ব `Implementor` কে বুঝিয়ে দেয়। (আমাদের উদাহরণে `Exporter` ক্লাস)
2. **Refined Abstraction:** এটি Abstraction এর চাইল্ড ক্লাস। (যেমন: `UserExporter`, `ProductExporter`)
3. **Implementor:** এটি সেই ইন্টারফেস যা আসল কাজটা (Implementation) কীভাবে হবে তার নিয়ম বলে দেয়। (আমাদের উদাহরণে `IFormatter`)
4. **Concrete Implementor:** এরা Implementor এর বাস্তব রূপ। (যেমন: `CsvFormatter`, `JsonFormatter`)

---

## C# কোড উদাহরণ (Code Example)

```csharp
using System;

namespace BridgePatternExample
{
    // ==========================================
    // IMPLEMENTATION HIERARCHY (কীভাবে ফরম্যাট হবে)
    // ==========================================

    // ৩. Implementor
    public interface IFormatter
    {
        string Format(string data);
    }

    // ৪. Concrete Implementors
    public class CsvFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"[CSV Formatted] {data}";
        }
    }

    public class JsonFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"{{ \"data\": \"{data}\" }}";
        }
    }

    public class XmlFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"<data>{data}</data>";
        }
    }

    // ==========================================
    // ABSTRACTION HIERARCHY (কী এক্সপোর্ট হবে)
    // ==========================================

    // ১. Abstraction (Bridge তৈরি হচ্ছে IFormatter এর সাথে)
    public abstract class Exporter
    {
        // এটাই হলো সেই 'Bridge' বা Composition
        protected IFormatter _formatter;

        protected Exporter(IFormatter formatter)
        {
            _formatter = formatter;
        }

        // রানটাইমে ফরম্যাটার চেঞ্জ করার সুবিধা (Optional)
        public void SetFormatter(IFormatter formatter)
        {
            _formatter = formatter;
        }

        public abstract void Export();
    }

    // ২. Refined Abstraction
    public class UserExporter : Exporter
    {
        public UserExporter(IFormatter formatter) : base(formatter) { }

        public override void Export()
        {
            string rawData = "User1, User2, User3";
            string formattedData = _formatter.Format(rawData);
            Console.WriteLine($"Exporting Users -> {formattedData}");
        }
    }

    public class ProductExporter : Exporter
    {
        public ProductExporter(IFormatter formatter) : base(formatter) { }

        public override void Export()
        {
            string rawData = "Laptop, Mouse, Keyboard";
            string formattedData = _formatter.Format(rawData);
            Console.WriteLine($"Exporting Products -> {formattedData}");
        }
    }

    // ==========================================
    // CLIENT CODE
    // ==========================================
    class Program
    {
        static void Main()
        {
            // আমরা Product Export করতে চাই JSON ফরম্যাটে
            IFormatter jsonFormatter = new JsonFormatter();
            Exporter productExporter = new ProductExporter(jsonFormatter);
            productExporter.Export();

            // হঠাৎ বসের মন চাইলো User Export করবে XML ফরম্যাটে
            IFormatter xmlFormatter = new XmlFormatter();
            Exporter userExporter = new UserExporter(xmlFormatter);
            userExporter.Export();

            // একই User Exporter দিয়ে এখন CSV তে এক্সপোর্ট করবো (Bridge এর ম্যাজিক)
            userExporter.SetFormatter(new CsvFormatter());
            userExporter.Export();
        }
    }
}
```

## Inheritance বনাম Composition (এবং Interface এর ভূমিকা)

অনেকের মনে হতে পারে Bridge Pattern এ "Composition over Interface" এর কথা বলা হয়েছে। কিন্তু আসল নিয়মটি হলো **"Composition over Inheritance"** (ইনহেরিট্যান্সের চেয়ে কম্পোজিশন ভালো)। 

**১. Inheritance (Is-a Relationship):**
ইনহেরিট্যান্স হলো অনমনীয় (Rigid)। আপনি যখন `class UserCsvExporter : OldExporter` লেখেন, তখন এটি সারাজীবন শুধুমাত্র CSV নিয়েই কাজ করবে। রানটাইমে (প্রোগ্রাম চলাকালীন) একে JSON-এ বদলানো সম্ভব নয়। এর ফলেই "Class Explosion" (অনেকগুলো ক্লাস তৈরি হওয়া) ঘটে।

**২. Composition (Has-a Relationship):**
কম্পোজিশন হলো প্লাগ-অ্যান্ড-প্লে (Plug and play) এর মতো। এখানে একটি ক্লাস অন্য একটি ক্লাসকে নিজের ভেতরে ধারণ করে। আমাদের কোডে `Exporter` ক্লাসের ভেতরে `protected IFormatter _formatter;` আছে। অর্থাৎ, Exporter এর কাছে একটি Formatter **"আছে"**। সুবিধা হলো, আপনি রানটাইমে `SetFormatter(new JsonFormatter())` কল করে যেকোনো সময় CSV থেকে JSON এ সুইচ করতে পারবেন।

**Interface এর ভূমিকা:**
কম্পোজিশন করার সময় আমরা কোনো ফিক্সড ক্লাসকে (যেমন শুধু `CsvFormatter`) ভেতরে রাখি না। আমরা ভেতরে রাখি একটি **Interface** (`IFormatter`) কে। এর মানে হলো— "আমার ভেতরে এমন যেকোনো কিছু বসানো যাবে, যা `IFormatter` এর নিয়ম মেনে চলে।" এটিই হলো ব্রিজ প্যাটার্নের আসল ম্যাজিক!

---

## Composition এবং Dependency Injection (DI) এর সম্পর্ক

ব্রিজ প্যাটার্নে Composition-কে সফলভাবে কাজ করানোর পেছনের আসল হিরো হলো **Dependency Injection (DI)**। একটি ছাড়া অন্যটির আসল ক্ষমতা বোঝাই যায় না।

**১. Composition (কী বানাবো?):** 
Composition মানে হলো একটি ক্লাস অন্য একটি ক্লাসকে নিজের ভেতরে ধারণ করা (Has-a relationship)। 
কিন্তু Composition এটা বলে না যে, অবজেক্টটা কোথা থেকে আসবে বা কীভাবে আসবে! 

**২. Dependency Injection (কীভাবে বানাবো?):** 
DI হলো সেই মেকানিজম বা পদ্ধতি—যার মাধ্যমে আমরা ভেতরের অবজেক্টটাকে **বাইরে থেকে** ভেতরে ঢুকিয়ে দিই (Inject করি)।

### কোড উদাহরণ দিয়ে পার্থক্য:

**DI ছাড়া Composition (Bad Practice):**
এখানে Composition হয়েছে ঠিকই, কিন্তু `Car` ক্লাসটি `ToyotaEngine` এর সাথে শক্তভাবে আটকে (Tightly coupled) গেছে।
```csharp
class Car {
    private ToyotaEngine _engine; // Concrete class (DIP Violated!)
    
    public Car() {
        _engine = new ToyotaEngine(); // হার্ডকোড করা হয়েছে (DI নেই)
    }
}
```

**DI সহ Composition (Good Practice / DIP):**
এখানে আমরা কনস্ট্রাক্টরের মাধ্যমে বাইরে থেকে ইঞ্জিন Inject করছি, এবং সেটি কোনো Concrete Class নয়, বরং একটি **Interface (`IEngine`)**! এটিই হলো **DIP (Dependency Inversion Principle)** এর পারফেক্ট উদাহরণ।
```csharp
public interface IEngine {
    void Start();
}

class Car {
    private IEngine _engine; // Composition with Interface

    // বাইরে থেকে ইন্টারফেস Inject করা হচ্ছে (Dependency Injection)
    public Car(IEngine engine) {
        _engine = engine; 
    }
}
```

**সংক্ষিপ্ত কথা:**
Composition হলো একটি ডিজাইন স্ট্রাকচার, আর Dependency Injection হলো সেই স্ট্রাকচারকে ফ্লেক্সিবল করার টুল বা মাধ্যম। DI হলো Composition-কে সবচেয়ে স্মার্ট উপায়ে ব্যবহার করার উপায়! 

---

## মূল কথা (Conclusion)
Bridge Pattern এর মূল মন্ত্র হলো: **Inheritance এর চেয়ে Composition ভালো (Prefer Composition over Inheritance)।** 
যখন দেখবেন আপনার ক্লাসে একাধিক ডাইমেনশনে পরিবর্তন আসার সম্ভাবনা আছে (যেমন Entity এবং Format), তখন সেগুলোকে আলাদা করে ইন্টারফেস ও কম্পোজিশন ব্যবহার করে তাদের মধ্যে Bridge তৈরি করে দেওয়াটাই সবচেয়ে স্মার্ট কাজ।
