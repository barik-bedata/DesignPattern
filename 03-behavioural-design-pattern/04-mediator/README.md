# Mediator Design Pattern

## 📖 Overview (ওভারভিউ)
**Mediator Pattern** হলো একটি Behavioral Design Pattern, যা একাধিক অবজেক্টের (Objects) মধ্যে সরাসরি যোগাযোগ (Direct Communication) বন্ধ করে একটি "মধ্যস্থতাকারী" বা "Mediator" অবজেক্টের মাধ্যমে যোগাযোগের ব্যবস্থা করে। 

সহজ কথায়: এটি একটি ক্লাসের "Many-to-Many" (জট পাকানো) রিলেশনশিপকে ভেঙে "One-to-Many" রিলেশনশিপে পরিণত করে।

### ✈️ বাস্তব জীবনের উদাহরণ (Air Traffic Control)
আকাশে অনেক প্লেন উড়ছে। প্লেনগুলো যদি নিজেদের মধ্যে সরাসরি কথা বলতে যায়, তবে চরম বিশৃঙ্খলা তৈরি হবে। এর বদলে তারা **ATC (Air Traffic Control) Tower** এর সাথে কথা বলে। টাওয়ার সবার অবস্থা জেনে প্লেনগুলোকে ইনস্ট্রাকশন দেয়। এখানে ATC টাওয়ার হলো `Mediator`, আর প্লেনগুলো হলো `Colleague`। 

## 💻 কম্পোনেন্টগুলো (৪টি অংশ)

১. **Mediator (Interface):** কলিগদের সাথে যোগাযোগের নিয়মকানুন ডিফাইন করে।
২. **Colleague (Interface/Abstract):** সেই অবজেক্টগুলো যারা একে অপরের সাথে কথা বলতে চায়। এরা শুধু `Mediator`-এর ইন্টারফেসকে চেনে, একে অপরকে চেনে না।
৩. **Concrete Mediator:** আসল মধ্যস্থতাকারী (যেমন: ATC টাওয়ার)। সে সব কলিগের লিস্ট রাখে এবং কার মেসেজ কার কাছে যাবে তা কন্ট্রোল করে।
৪. **Concrete Colleague:** আসল অবজেক্টগুলো (যেমন: বোয়িং ৭৭৭, এয়ারবাস)। এরা শুধু টাওয়ারকে মেসেজ পাঠায়।

## 🛡️ SOLID Principles Adherence

আমাদের C# কোডটি সম্পূর্ণ SOLID প্রিন্সিপাল মেনে তৈরি করা হয়েছে, বিশেষ করে আপনার রিকোয়ারমেন্ট অনুযায়ী **DIP (Dependency Inversion Principle)** এর ওপর সর্বোচ্চ জোর দেওয়া হয়েছে:

- **Single Responsibility (SRP):** `AtcTower` শুধু মেসেজ রাউটিং এর কাজ করে। `Boeing777` শুধু নিজের মেসেজ পাঠানো আর রিসিভ করার কাজ করে।
- **Open/Closed (OCP):** আগামীকাল নতুন `FighterJet` প্লেন তৈরি করলে টাওয়ারের কোডে কোনো পরিবর্তন করতে হবে না। শুধু `IAircraft` ইমপ্লিমেন্ট করলেই টাওয়ার তাকে চিনে নেবে।
- **Dependency Inversion (DIP):** 
  - `AtcTower` (Concrete Mediator) কোনো নির্দিষ্ট প্লেনকে চেনে না, সে শুধু `IAircraft` (Interface) এর ওপর নির্ভর করে। 
  - `AircraftBase` (Colleague) সরাসরি `AtcTower` ক্লাসকে চেনে না, সে শুধু `IAirTrafficControl` (Interface) এর ওপর নির্ভর করে। 

## 🤔 আরও কিছু রিয়েল-ওয়ার্ল্ড উদাহরণ (কোড ফাইলে দেওয়া আছে)

আমাদের গিটহাব ফোল্ডারে ATC ছাড়াও আরও ৫টি প্র্যাকটিক্যাল উদাহরণ দেওয়া আছে:

১. **[ChatRoomExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-mediator/ChatRoomExample.cs):** 
   একটি চ্যাট অ্যাপ্লিকেশনে ইউজাররা একে অপরের সাথে সরাসরি কানেক্টেড থাকে না। তারা `ChatServer` (Mediator) কে মেসেজ দেয়, সার্ভার বাকিদের কাছে সেটি ব্রডকাস্ট করে।

২. **[SmartHomeExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-mediator/SmartHomeExample.cs):** 
   মোশন সেন্সর, লাইট এবং এসি একে অপরকে চেনে না। সেন্সর শুধু `HomeHub` (Mediator) কে জানায় যে মোশন ডিটেক্ট হয়েছে। হাব তখন একা একাই লাইট এবং এসি অন করে দেয়।

৩. **[UiDialogExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-mediator/UiDialogExample.cs):** 
   একটি ফর্মে Checkbox এ ক্লিক করলে Submit Button এনাবল হবে। এই লজিকটি সরাসরি চেকবক্সের ভেতর না লিখে `RegistrationForm` (Mediator) এর ভেতর লেখা হয়।

৪. **[StockExchangeExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-mediator/StockExchangeExample.cs):** 
   স্টক মার্কেটে বায়ার এবং সেলার একে অপরের সাথে সরাসরি ট্রেড করে না। তারা `StockBroker` (Mediator) এর মাধ্যমে অর্ডার সাবমিট করে, ব্রোকার সেটি ম্যাচ করে এক্সিকিউট করে।

৫. **[GameLobbyExample.cs](file:///Users/bedata/Desktop/Learning/DesignPattern/03-behavioural-design-pattern/04-mediator/GameLobbyExample.cs):** 
   মাল্টিপ্লেয়ার গেমে প্লেয়াররা একে অপরের স্ট্যাটাস চেক করে না। তারা শুধু `LobbyManager` (Mediator) কে বলে যে তারা রেডি। যখন সবাই রেডি হয়, লবি একা একাই গেম স্টার্ট করে দেয়।
