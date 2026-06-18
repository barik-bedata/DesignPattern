# Visitor Design Pattern

## 📖 Overview
The **Visitor Design Pattern** is a behavioral design pattern that allows you to add new operations to existing classes without modifying them. It separates the algorithm from the objects on which it operates, heavily utilizing a technique called **Double Dispatch**.

## 🏘️ গল্পে গল্পে Visitor Pattern (The Story)
ধরুন একটি পাড়ায় ৩টি ভিন্ন ভিন্ন বাড়ি আছে: **কাঠের বাড়ি, ইটের বাড়ি, এবং মাটির বাড়ি**।
এখন একদিন ওই পাড়ায় একজন **ইন্স্যুরেন্স এজেন্ট (Visitor)** আসলো। তার কাজ হলো প্রতিটি বাড়ির জন্য ইন্স্যুরেন্স প্রিমিয়াম হিসাব করা। 

এখন, "ইন্স্যুরেন্স কীভাবে হিসাব করতে হবে"—এই লজিক কি বাড়ির ভেতরে লেখা থাকা উচিত? কখনোই না! কারণ বাড়ির কাজ হলো শুধু মানুষকে আশ্রয় দেওয়া (Single Responsibility Principle)। 

তাই Visitor Pattern বলে: 
- বাড়ির দরজায় শুধু একটি মেথড রাখো: `Accept(Visitor)`. 
- ইন্স্যুরেন্স এজেন্ট (Visitor) যখন দরজায় কড়া নাড়বে, বাড়িওয়ালা শুধু দরজা খুলে বলবে, "Welcome! ভেতরে আসুন!" (`visitor.Visit(this)`)।
- এরপর এজেন্ট ঘরের ভেতরে ঢুকে দেখবে এটা কাঠের বাড়ি নাকি ইটের বাড়ি, এবং সেই অনুযায়ী সে নিজের খাতায় হিসাব করবে।

ভবিষ্যতে যদি ইন্স্যুরেন্স এজেন্টের বদলে একজন **Tax Collector (ট্যাক্স অফিসার)** আসে, তবে বাড়ির কোনো কোড চেঞ্জ করতে হবে না! ট্যাক্স অফিসারও একই দরজা দিয়ে ঢুকবে এবং নিজের মতো ট্যাক্স হিসাব করবে।

## 🧱 5 Key Components

1. **Visitor Interface (`IVisitor`)**: ডিক্লেয়ার করে যে ভিজিটর কোন কোন এলিমেন্টের কাছে যেতে পারবে (যেমন: `Visit(WoodenHouse)`, `Visit(BrickHouse)`)।
2. **Concrete Visitor (`TaxVisitor`, `InsuranceVisitor`)**: এরা হলো সত্যিকারের ভিজিটর যারা ওই ইন্টারফেস ইমপ্লিমেন্ট করে এবং আসল হিসাব-নিকাশ বা লজিক এক্সিকিউট করে।
3. **Element Interface (`IElement`)**: যেসব এলিমেন্টের কাছে ভিজিটর যাবে, তাদের একটি কমন ইন্টারফেস। এর একটাই কাজ—ভিজিটরকে এক্সেপ্ট করা (`Accept(IVisitor)`).
4. **Concrete Elements (`WoodenHouse`, `BrickHouse`)**: রিয়েল অবজেক্ট। এরা `Accept` মেথড কল হলে নিজেদেরকে ভিজিটরের কাছে তুলে দেয় (`visitor.Visit(this)`).
5. **Object Structure / Client (`List<IElement>`)**: এটি হলো সেই পাড়া বা কালেকশন যেখানে ভিজিটর ঘুরে বেড়াবে এবং সবাইকে ভিজিট করবে।

## 💡 Important Note on GFG Article Bug
The GeeksforGeeks article had a severe SOLID violation. It hardcoded the properties of the shapes (like `radiusOfCircle = 5`) inside the `AreaCalculator` Visitor class! 
**Why is this wrong?** Because a Circle's radius belongs to the Circle, not the Visitor! If we have two different circles with radii 5 and 10, the GFG code would fail entirely. 
In our `ShapeAreaExample.cs`, we have fixed this. The Shapes hold their own properties, and the Visitor accesses them properly.

## 💻 4 Practical Examples Included
We have implemented 4 completely SOLID-compliant examples in C# to demonstrate this pattern:

1. **`ShapeAreaExample.cs`**
   - **Elements**: `Circle`, `Square`, `Triangle`.
   - **Visitor**: `AreaCalculatorVisitor`.
   - **Purpose**: Calculates the area of different shapes without modifying the shape classes. Fixes the GFG hardcoding bug.

2. **`SupermarketCheckoutExample.cs`**
   - **Elements**: `Laptop`, `Fruit`, `Book`.
   - **Visitors**: `TaxVisitor`, `DiscountVisitor`.
   - **Purpose**: Calculates dynamic tax rates and discounts based on the type of product at checkout.

3. **`DocumentExporterExample.cs`**
   - **Elements**: `Paragraph`, `Image`, `Table`.
   - **Visitors**: `HtmlExportVisitor`, `PdfExportVisitor`.
   - **Purpose**: Converts an internal document structure into different formats (HTML, PDF) cleanly.

4. **`OrganizationSalaryExample.cs`**
   - **Elements**: `RegularEmployee`, `Manager`, `Director`.
   - **Visitors**: `BonusCalculatorVisitor`, `MedicalAllowanceVisitor`.
   - **Purpose**: Calculates annual bonuses and assigns medical coverage tiers based on employee roles.

## ⚠️ Visitor Pattern-এ State Management (`+=` vs `=`)
Visitor-এর ভেতরে ডেটা কীভাবে রাখা হচ্ছে, তার ওপর আপনার অ্যাপ্লিকেশনের রেজাল্ট পুরোপুরি নির্ভর করে:
- **Accumulating State (`+=`)**: আমাদের `ShapeAreaExample`-এ আমরা `TotalArea += area` ব্যবহার করেছি। এর ফলে একটি লিস্টের সবগুলো শেপের এরিয়া একসাথে যোগ হয়ে **Total Sum** বের হয়েছে।
- **Single Assignment (`=`)**: যদি আমরা `+=` না করে সরাসরি `Area = area` লিখতাম, তবে এটি আগের ডেটা মুছে ফেলতো এবং শুধু সর্বশেষ যে শেপটি ভিজিট করা হয়েছে, তার এরিয়া দেখাতো (অর্থাৎ **Single Area**)। আপনার রিকোয়ারমেন্ট যদি হয় একেকটি শেপের আলাদা আলাদা এরিয়া বের করা (লিস্টের সামেশন নয়), তবে `+=` এর বদলে শুধু `=` ব্যবহার করতে হবে।


