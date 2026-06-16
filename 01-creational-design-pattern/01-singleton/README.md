# Singleton Design Pattern

## 📖 What is Singleton?
**Singleton** is a creational design pattern that lets you ensure that a class has only one instance, while providing a global access point to this instance.

## 🤔 Why do we need it?
Normally, we create new objects whenever we need them using `new ClassName()`. However, for some specific objects, creating multiple instances can cause serious problems:

1. **Data Inconsistency (e.g., Logger)**
   If different parts of an application create their own `Logger` instances, the log data will be scattered across multiple lists instead of being in one centralized place.

2. **Resource & Performance Overhead (e.g., Database Connection)**
   Creating a database connection is an expensive operation (takes time and memory). If we create a new connection for every single user request, the database server might overload and crash. A Singleton ensures we create the connection only once and reuse it.

3. **State Mismatch & Slowness (e.g., App Configuration)**
   Reading settings from a disk is slow. If every module reads the config file separately, it wastes time. Moreover, if one module updates a setting (like changing theme to "Dark"), other modules with their own instances won't see the update.

## 🚀 Advanced: Multithreading & `volatile`
In a multithreaded application, if two threads try to access the Singleton at the exact same time, they might bypass the `if (instance == null)` check simultaneously and create **two** instances. 

To prevent this, we use a `lock`. However, simply locking isn't enough due to **CPU Caching and Instruction Reordering**. 
By adding the `volatile` keyword to our instance variable, we instruct the processor to:
1. NEVER cache this variable in a CPU register.
2. ALWAYS read and write its value directly from the **Main Memory (RAM)**.
This ensures that if Thread-A creates the instance, Thread-B instantly sees it from the RAM, avoiding subtle, hard-to-reproduce bugs.

## 🪄 The Magic of `Lazy<T>` (Deferred Instantiation)
In modern C#, `Lazy<T>` is the best way to implement a Singleton. But how does it work internally?

1. **Delegate Storage:** When you write `new Lazy<T>(() => new Object())`, it doesn't create the object. It just "remembers" the function (delegate) on how to create it.
2. **The Internal Flag:** It maintains an internal boolean flag (e.g., `IsValueCreated = false`).
3. **The First Call:** When someone calls `.Value` for the very first time, `Lazy` checks the flag, uses an internal thread-safe lock, runs the function to create the object, saves it in memory, and turns the flag to `true`.
4. **Subsequent Calls:** Next time `.Value` is called, it sees the flag is `true` and instantly returns the cached object without locking.

**Why is it so powerful?**
If your application has a huge 5GB Database Connection object, but the user never clicks the "Show Database" button during their session, that 5GB object is **never created**. It guarantees zero wasted memory and lightning-fast app startup times!

## 🛠️ How to implement it?
To create a Singleton class, you generally need to do two things:
1. Make the **constructor private** so that no one can use the `new` keyword to create an object from outside the class.
2. Create a **static method or property** that returns the instance (using Double-Check Locking or `Lazy<T>`).

## 💻 Examples Included
Check out the examples in this directory:
* [`example1.cs`](example1.cs): Basic implementation (Logger, DB Connection, Config).
* [`example2-thread-safe.cs`](example2-thread-safe.cs): Multithreading implementation (Double-Check Locking with `volatile` and `Lazy<T>`).
* [`example3-lazy-singleton.cs`](example3-lazy-singleton.cs): Deep dive into how `Lazy<T>` delays expensive object creation until needed.
