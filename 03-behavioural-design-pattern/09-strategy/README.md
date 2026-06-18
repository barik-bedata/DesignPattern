# Strategy Design Pattern

## 📖 Overview
The Strategy Design Pattern is a behavioral design pattern that lets you define a family of algorithms, put each of them into a separate class, and make their objects interchangeable. It allows the algorithm to vary independently from clients that use it.

## 🧱 Components (4 Key Elements)

1. **Context**
   - Acts as an intermediary between the client and the strategy, delegating tasks to the selected strategy.
   - Holds a reference to a strategy object and uses it to perform operations.
   - Allows switching strategies without changing its own code.

2. **Strategy Interface**
   - Defines a common interface that all concrete strategies must implement.
   - Ensures consistency so all strategies are interchangeable.
   - Promotes flexibility by decoupling context from implementations.

3. **Concrete Strategies**
   - Provide specific implementations of the strategy interface with different algorithms or behaviors.
   - Encapsulate the actual logic of each algorithm.
   - Can be selected and replaced based on requirements.

4. **Client**
   - Responsible for selecting and configuring the appropriate strategy for the context.
   - Decides which strategy to use based on the problem.
   - Passes the chosen strategy to the context for execution.

## 🤔 When to use it?
- When you have a lot of similar classes that only differ in the way they execute some behavior.
- To avoid massive `if-else` or `switch` statements for selecting an algorithm.
- When you need different variations of an algorithm (e.g., Sorting, Payment Methods, Route Calculation).

## 💻 Examples Included
1. **`SortingExample.cs`**: Context is `SortingContext`. Strategies are `BubbleSort`, `MergeSort`, `QuickSort`.
2. **`PaymentSystemExample.cs`**: Context is `ShoppingCart`. Strategies are `CreditCard`, `PayPal`, `bKash`.
3. **`NavigationAppExample.cs`**: Context is `Navigator`. Strategies are `Driving`, `Walking`, `Transit`.
4. **`FileCompressionExample.cs`**: Context is `CompressionContext`. Strategies are `Zip`, `Rar`, `TarGz`.
