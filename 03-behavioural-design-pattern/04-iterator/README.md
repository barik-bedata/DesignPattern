# Iterator Design Pattern

## 📖 Overview (ওভারভিউ)
**Iterator Pattern** হলো একটি Behavioral Design Pattern যা কোনো কালেকশনের (যেমন: List, Array, Tree, Graph) ভেতরের ডেটা স্ট্রাকচার এক্সপোজ না করেই তার ইলিমেন্টগুলোকে এক এক করে ট্রাভার্স (Traverse/Loop) করার সুবিধা দেয়।

সহজ কথায়: আপনার কাছে একটি কালেকশন আছে। ক্লায়েন্টকে সেই কালেকশনের ডেটাগুলো কীভাবে সেভ করা আছে (যেমন `for(int i=0)` বা `node.Next`) তা বুঝতে হবে না। ক্লায়েন্টকে শুধু একটি `Iterator` (রিমোট) দেওয়া হবে, যা দিয়ে সে `Next()` কল করে ডেটা দেখতে পারবে।

### 📺 বাস্তব জীবনের উদাহরণ (TV Remote Control)
আপনার টিভিতে চ্যানেলগুলো কীভাবে সেভ করা আছে (Array নাকি List) তা আপনি জানেন না। আপনার হাতে একটি **রিমোট কন্ট্রোল (Iterator)** আছে। আপনি শুধু `Next` বাটনে চাপ দেন আর চ্যানেল চেঞ্জ হয়। এখানে টিভি হলো `Aggregate` (Collection) আর রিমোট হলো `Iterator`। 

## 💻 কম্পোনেন্টগুলো (৪টি অংশ)

১. **Iterator Interface (`IChannelIterator`):** রিমোট কন্ট্রোলের বাটনগুলো (`First()`, `Next()`, `IsDone()`, `CurrentItem()`) ডিফাইন করে।
২. **Concrete Iterator (`TvRemoteControl`):** আসল রিমোট। এটি মনে রাখে যে আপনি বর্তমানে কত নম্বর চ্যানেলে আছেন (Current Position track করে) এবং `Next()` কল করলে পরের চ্যানেল এনে দেয়।
৩. **Aggregate Interface (`ITvChannelCollection`):** কালেকশনের নিয়মকানুন। এটি বলে দেয় যে কালেকশনের কাছে একটি `CreateIterator()` মেথড থাকতে হবে।
৪. **Concrete Aggregate (`TvChannelCollection`):** আসল টিভি বা ডেটা কালেকশন। এর ভেতরে ডেটা (চ্যানেল লিস্ট) স্টোর করা থাকে।

## 🛡️ SOLID Principles Adherence (DIP)

আমাদের কোডটি সম্পূর্ণ **Dependency Inversion Principle (DIP)** মেনে তৈরি করা হয়েছে:
- `TvRemoteControl` (Iterator) সরাসরি `TvChannelCollection` (Concrete Class) এর ওপর নির্ভর করে না। এটি `ITvChannelCollection` (Interface) এর ওপর নির্ভর করে।
- Client (`Program`) সরাসরি `TvRemoteControl` কে কল করে না। সে `IChannelIterator` ইন্টারফেস দিয়ে কাজ করে।

## 🤔 আরও একটি রিয়েল-ওয়ার্ল্ড উদাহরণ (Pagination)

আমাদের ফোল্ডারে **[NewsFeedIteratorExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-iterator/NewsFeedIteratorExample.cs)** নামে একটি চমৎকার ফাইল আছে। 

ধরুন, একটি সোশ্যাল মিডিয়া ফিডে ১ লক্ষ পোস্ট আছে। সবগুলো পোস্ট একবারে লোড করলে অ্যাপ ক্র্যাশ করবে। তাই আমরা `GetPostsFromApi(pageNumber, pageSize)` ব্যবহার করে একসাথে ৫টি বা ১০টি করে পোস্ট আনি (যাকে Pagination বলে)।

**Iterator Pattern এর ম্যাজিক:**
ক্লায়েন্ট (যেমন: মোবাইল অ্যাপের UI) এই পেজিনেশন সম্পর্কে কিছুই জানে না! ক্লায়েন্ট শুধু `while(scroller.HasNext())` ব্যবহার করে স্ক্রল করে যায়। ব্যাকগ্রাউন্ডে `PaginationIterator` একা একাই হিসেব রাখে যে কখন কারেন্ট পেজের পোস্ট শেষ হলো এবং কখন চুপিচুপি ডাটাবেজ থেকে পরের পেজের পোস্টগুলো নিয়ে আসতে হবে!

অর্থাৎ, Iterator Pattern ক্লায়েন্টের কাছ থেকে অত্যন্ত জটিল Data Fetching Logic কে পুরোপুরি লুকিয়ে রাখে (Abstraction)।

## 🤔 কখন ব্যবহার করবেন? (Real World Use Cases)

১. **C# `foreach` loop:** সি-শার্পের `IEnumerable` এবং `IEnumerator` হলো Iterator প্যাটার্নের সবচেয়ে বড় উদাহরণ। আপনি যখন `foreach` লুপ চালান, তখন এটি ব্যাকগ্রাউন্ডে Iterator তৈরি করে কাজ করে।
২. **Database Result Set:** ডাটাবেজ থেকে লক্ষ লক্ষ ডেটা আসলে তা একবারে মেমোরিতে না এনে `DbDataReader` বা `ResultSet` এর মাধ্যমে এক এক করে রিড করা হয় (যেমন `reader.Read()`)। এটিও Iterator প্যাটার্ন।
৩. **Social Media Feed:** ফেইসবুক বা ইনস্টাগ্রামের নিউজফিড। আপনি স্ক্রল করেন আর ডেটা লোড হতে থাকে। ক্লায়েন্ট জানে না সার্ভার কীভাবে ডেটা স্ট্রাকচার সাজিয়েছে, ক্লায়েন্ট শুধু `GetNextBatch()` কল করে।
