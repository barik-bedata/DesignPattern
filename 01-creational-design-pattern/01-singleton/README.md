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

## 🛠️ How to implement it?
To create a Singleton class, you generally need to do two things:
1. Make the **constructor private** so that no one can use the `new` keyword to create an object from outside the class.
2. Create a **static method** (e.g., `GetInstance()`) that acts as a constructor. This method checks if an instance already exists. If it does not, it creates one and stores it in a private static variable. If it does, it simply returns the existing one.

## 💻 Examples Included
Check out the [`example1.cs`](example1.cs) file in this directory to see real-world scenarios implemented in C#:
* **Example 1:** A `Logger` that shares the same log list across modules.
* **Example 2:** A `DatabaseConnection` that prevents redundant expensive connections.
* **Example 3:** An `AppConfig` that caches settings in memory for instant access.
