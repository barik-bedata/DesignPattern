# Observer Design Pattern

## 📖 Overview (ওভারভিউ)
**Observer Pattern** হলো একটি Behavioral Design Pattern যা `One-to-Many` রিলেশনশিপ তৈরি করে। অর্থাৎ, যখন একটি অবজেক্টের (Subject) অবস্থায় কোনো পরিবর্তন আসে, তখন তার সাথে যুক্ত বাকি সব অবজেক্টকে (Observers) অটোমেটিকভাবে নোটিফাই করা হয়।

### 🔔 বাস্তব জীবনের উদাহরণ
১. **YouTube Channel:** আপনি কোনো চ্যানেলে সাবস্ক্রাইব করলে নতুন ভিডিও আপলোড হওয়ামাত্র আপনার কাছে নোটিফিকেশন চলে আসে।
২. **E-commerce Notification:** "Back in Stock" এ ক্লিক করে রাখলে প্রোডাক্ট স্টকে আসামাত্র ইমেইল, এসএমএস এবং অ্যাপে নোটিফিকেশন আসে।
৩. **Weather Station:** একটি ওয়েদার স্টেশন (Subject) যখন ওয়েদার আপডেট করে, তখন কানেক্টেড সকল ডিসপ্লে (Phone, TV) অটোমেটিকভাবে আপডেট পেয়ে যায়।

## 🌐 Modern Web & State Management (মডার্ন ওয়েব অ্যাপ্লিকেশনে ব্যবহার)
আপনি একদম 100% সঠিক ধরেছেন! মডার্ন ফ্রন্টএন্ড ডেভেলপমেন্ট এবং স্টেট ম্যানেজমেন্টের কোর আর্কিটেকচারই দাঁড়িয়ে আছে **Observer Pattern** এর ওপর। 

১. **RxJS (Reactive Extensions):** 
RxJS এর মূল ভিত্তিই হলো Observer Pattern। এর `Observable` ক্লাসটি হলো আমাদের `Subject`, আর `subscribe()` মেথডের ভেতরে আমরা যে লজিকগুলো পাস করি, সেগুলো হলো `Observer`। যখন ডেটা স্ট্রিম আসে, Observable তার সকল সাবস্ক্রাইবারদের লুপ চালিয়ে নোটিফাই করে।

২. **Redux / Redux Toolkit (React):** 
Redux এর `Store` হলো একটি বিশাল `Subject`। আর আমাদের React কম্পোনেন্টগুলো (যারা `useSelector` হুক বা `connect` ব্যবহার করে) হলো `Observer`। 
যখনই কোনো Action ডিসপ্যাচ করা হয় এবং Store এর State পরিবর্তন হয়, Store তখন সাথে সাথে তার সাথে কানেক্টেড থাকা সকল কম্পোনেন্টকে (Observers) নোটিফাই করে, যার ফলে কম্পোনেন্টগুলো রি-রেন্ডার হয়!

৩. **Vuex / Pinia (Vue.js):**
একইভাবে Vue.js এর স্টেট ম্যানেজমেন্ট লাইব্রেরিগুলোও ডেটার রিঅ্যাক্টিভিটি (Reactivity) মেইনটেইন করার জন্য হুবহু Observer Pattern ব্যবহার করে থাকে।

## 💻 কম্পোনেন্টগুলো
১. **Subject / Observable (`ISubject`):** 
যার দিকে সবাই তাকিয়ে থাকে। এর কাজ হলো Observer দের লিস্ট নিজের কাছে রাখা এবং কোনো ইভেন্ট ঘটলে সবাইকে লুপ চালিয়ে নোটিফাই করা (`AddObserver`, `RemoveObserver`, `NotifyObservers`)। 

২. **Observer (`IObserver`):** 
যারা নোটিফিকেশন পাওয়ার জন্য অপেক্ষা করছে। এদের একটি `Update()` মেথড থাকে, যা Subject কল করে দেয়।

৩. **Concrete Subject & Concrete Observer:** 
আসল ক্লাসগুলো যারা ইন্টারফেসগুলো ইমপ্লিমেন্ট করে (যেমন: `WeatherStation`, `PhoneDisplay`)।

## 🛡️ SOLID Principles Adherence (DIP & OCP)

- **DIP (Dependency Inversion Principle):** Subject সরাসরি কোনো Concrete Observer কে চেনে না। সে শুধু `IObserver` ইন্টারফেসকে চেনে।
- **OCP (Open-Closed Principle):** ভবিষ্যতে যদি নতুন কোনো নোটিফিকেশন সিস্টেম (যেমন: `WhatsAppNotifier`) বা নতুন কোনো ডিসপ্লে (`SmartWatchDisplay`) অ্যাড করতে হয়, তবে Subject ক্লাসে বিন্দুমাত্র পরিবর্তন করার দরকার নেই। শুধু `IObserver` ইমপ্লিমেন্ট করে নতুন ক্লাস বানিয়ে `AddObserver()` করে দিলেই হবে! এটিই Observer প্যাটার্নের সবচেয়ে বড় সুবিধা।

## 📂 এই ফোল্ডারের এক্সাম্পলগুলো
১. **[WeatherStationExample.cs](WeatherStationExample.cs):** GeeksforGeeks এর ফেমাস ওয়েদার স্টেশন এক্সাম্পল (100% SOLID & DIP Compliant)।
২. **[EcommerceNotificationExample.cs](EcommerceNotificationExample.cs):** Email, SMS, এবং Push Notification এর একটি দারুণ ই-কমার্স এক্সাম্পল, যেখানে প্রোডাক্ট স্টকে আসলে ৩ জায়গায় নোটিফিকেশন যায়।
৩. **[YouTubeSubscriptionExample.cs](YouTubeSubscriptionExample.cs):** ইউটিউব চ্যানেল সাবস্ক্রিপশন এবং ভিডিও আপলোড নোটিফিকেশন এক্সাম্পল।
