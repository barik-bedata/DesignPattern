# State Design Pattern

## 📖 Overview (ওভারভিউ)
**State Pattern** হলো একটি Behavioral Design Pattern, যা কোনো অবজেক্টের ভেতরের অবস্থা (Internal State) পরিবর্তন হওয়ার সাথে সাথে তার আচরণ (Behavior) পরিবর্তন করার সুবিধা দেয়। 

সাধারণত আমরা এই কাজের জন্য বিশাল বড় `if-else` বা `switch-case` স্টেটমেন্ট ব্যবহার করি। কিন্তু State Pattern এই লজিকগুলোকে আলাদা আলাদা ക്ലാসে (State Classes) ভাগ করে দেয়, ফলে মেইন ক্লাসে (Context) কোনো `if-else` লিখতে হয় না!

## 💡 কেন State Pattern ব্যবহার করবেন?
১. **OCP (Open-Closed Principle):** নতুন কোনো স্টেট অ্যাড করতে হলে মেইন ক্লাসের `if-else` মডিফাই করতে হয় না, শুধু নতুন একটি স্টেট ক্লাস বানালেই হয়।
২. **Clean Code:** বিশাল বড় কন্ডিশনাল ব্লক থেকে মুক্তি দেয়।
৩. **State-Specific Behavior:** প্রতিটি স্টেটের নিজস্ব দায়িত্ব নির্দিষ্ট থাকে।

## 📂 এই ফোল্ডারের রিয়েল-লাইফ এক্সাম্পলগুলো (100% SOLID & DIP Compliant)

আমরা এই ফোল্ডারে ৪টি চমৎকার রিয়েল-লাইফ এক্সাম্পল রেখেছি। সবগুলো এক্সাম্পলে **DIP (Dependency Inversion Principle)** মানার জন্য State ক্লাসগুলো সরাসরি Concrete Context কে চেনে না, বরং একটি ইন্টারফেস (যেমন `IMediaPlayerStateMachine`) ব্যবহার করে। 

### ১. [Media Player (MediaPlayerExample.cs)](MediaPlayerExample.cs)
একটি মিউজিক প্লেয়ার যেখানে `Playing`, `Paused`, এবং `Stopped` স্টেট আছে। 
- Stopped অবস্থায় Play দিলে গান শুরু হয়। 
- Playing অবস্থায় Play দিলে কিছুই হয় না।

### ২. [Document Workflow / CMS (DocumentWorkflowExample.cs)](DocumentWorkflowExample.cs)
একটি নিউজপেপার বা ব্লগের ডকুমেন্ট এডিটিং ফ্লো।
- **States:** `Draft` ➡️ `Moderation` ➡️ `Published`
- Draft অবস্থায় Publish করলে মডারেশনে যায়, মডারেশন থেকে Publish করলে পাবলিক হয়।

### ৩. [Vending Machine (VendingMachineExample.cs)](VendingMachineExample.cs)
একটি ক্লাসিক ভেন্ডিং মেশিন যেখানে টাকা ঢোকালে স্টেট চেঞ্জ হয়।
- **States:** `NoMoney`, `HasMoney`, `Dispensing`
- টাকা ছাড়া প্রোডাক্ট সিলেক্ট করলে এরর দেয়।

### ৪. [ATM Machine (AtmMachineExample.cs)](AtmMachineExample.cs)
একটি এটিএম বুথ যেখানে কার্ড ঢোকানো, পিন দেওয়া এবং টাকা তোলার ফ্লো কন্ট্রোল করা হয়।
- **States:** `NoCard`, `HasCard`, `PinEntered`
- পিন ছাড়া টাকা তোলা যায় না এবং ভুল পিন দিলে কার্ড বের করে দেয়।
