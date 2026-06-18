# Observer Design Pattern

## 📖 Overview (ওভারভিউ)
**Observer Pattern** হলো একটি Behavioral Design Pattern যা `One-to-Many` রিলেশনশিপ তৈরি করে। অর্থাৎ, যখন একটি অবজেক্টের (Subject) অবস্থায় কোনো পরিবর্তন আসে, তখন তার সাথে যুক্ত বাকি সব অবজেক্টকে (Observers) অটোমেটিকভাবে নোটিফাই করা হয়।

### 🔔 বাস্তব জীবনের উদাহরণ
১. **YouTube Channel:** আপনি কোনো চ্যানেলে সাবস্ক্রাইব করলে নতুন ভিডিও আপলোড হওয়ামাত্র আপনার কাছে নোটিফিকেশন চলে আসে।
২. **E-commerce Notification:** "Back in Stock" এ ক্লিক করে রাখলে প্রোডাক্ট স্টকে আসামাত্র ইমেইল, এসএমএস এবং অ্যাপে নোটিফিকেশন আসে।

## 💻 কম্পোনেন্টগুলো

১. **Subject / Observable (`ISubject`):** 
যার দিকে সবাই তাকিয়ে থাকে। এর কাজ হলো Observer দের লিস্ট নিজের কাছে রাখা এবং কোনো ইভেন্ট ঘটলে সবাইকে লুপ চালিয়ে নোটিফাই করা (`Attach`, `Detach`, `Notify`)। 

২. **Observer (`IObserver`):** 
যারা নোটিফিকেশন পাওয়ার জন্য অপেক্ষা করছে। এদের একটি `Update()` বা `ReceiveNotification()` মেথড থাকে, যা Subject কল করে দেয়।

৩. **Concrete Subject & Concrete Observer:** 
আসল ক্লাসগুলো যারা ইন্টারফেসগুলো ইমপ্লিমেন্ট করে (যেমন: `Product`, `EmailNotifier`, `YouTubeChannel`)।

## 🛡️ SOLID Principles Adherence (DIP & OCP)

- **DIP (Dependency Inversion Principle):** Subject সরাসরি কোনো Concrete Observer কে চেনে না। সে শুধু `IObserver` ইন্টারফেসকে চেনে।
- **OCP (Open-Closed Principle):** ভবিষ্যতে যদি নতুন কোনো নোটিফিকেশন সিস্টেম (যেমন: `WhatsAppNotifier`) অ্যাড করতে হয়, তবে Subject ক্লাসে বিন্দুমাত্র পরিবর্তন করার দরকার নেই। শুধু `IObserver` ইমপ্লিমেন্ট করে নতুন ক্লাস বানিয়ে `Attach()` করে দিলেই হবে! এটিই Observer প্যাটার্নের সবচেয়ে বড় সুবিধা।

## 📂 এই ফোল্ডারের এক্সাম্পলগুলো
১. **[EcommerceNotificationExample.cs](EcommerceNotificationExample.cs):** Email, SMS, এবং Push Notification এর একটি দারুণ ই-কমার্স এক্সাম্পল, যেখানে প্রোডাক্ট স্টকে আসলে ৩ জায়গায় নোটিফিকেশন যায়।
২. **[YouTubeSubscriptionExample.cs](YouTubeSubscriptionExample.cs):** ইউটিউব চ্যানেল সাবস্ক্রিপশন এবং ভিডিও আপলোড নোটিফিকেশন এক্সাম্পল।
