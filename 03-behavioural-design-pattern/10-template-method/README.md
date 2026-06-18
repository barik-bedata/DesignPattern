# Template Method Design Pattern

## 📖 Overview
The **Template Method Design Pattern** is a behavioral design pattern that defines the overall structure (skeleton) of an algorithm in a base class. It allows subclasses to redefine or customize specific steps of the algorithm without changing its core structure.

## 🤔 Is this even a Design Pattern?
**"This just looks like basic Inheritance and Method Overriding!"**

Yes, you are absolutely right! Today, what we consider "basic OOP knowledge" was documented as the *Template Method Pattern* in the 1990s by the Gang of Four (GoF). Before modern OOP became a standard, organizing code with a fixed skeleton and variable steps was a major architectural breakthrough.

### 🎬 The Hollywood Principle
The true magic of this pattern lies in the **"Hollywood Principle": *Don't call us, we'll call you.***
Instead of the subclass calling the parent class, the Parent (Base) class calls the Subclass at the exact right moment. 

This is the exact mechanism that powers almost all modern **Frameworks** (like ASP.NET, React, Android SDK). The framework writes the giant "Template Method" and calls your custom overridden methods (like `OnInit`, `Page_Load`, `onCreate`) when needed!

---

## 🧱 Components (4 Key Elements)

1. **Abstract Class** 
   - Defines the template method (algorithm skeleton) with some steps implemented and others left abstract or as hooks for customization.
2. **Template Method** 
   - Outlines the algorithm’s fixed structure by calling steps in order. 
   - In C#, this is kept non-virtual (equivalent to `final` in Java) to prevent changes to the skeleton sequence.
3. **Abstract/Hook Methods**
   - Placeholder methods in the abstract class that subclasses *must* implement or *optionally* override.
4. **Concrete Subclasses**
   - Provide implementations for abstract methods, customizing specific steps while preserving the overall algorithm.

---

## 💻 6 Practical Examples Included

We have fully implemented **6 Real-Life Examples** using C# with 100% SOLID Principles. Every example contains detailed Bengali comments explaining the 4 components.

1. **`BeverageMakerExample.cs`**
   - **Scenario**: Making Tea and Coffee.
   - **Template Sequence**: Boil Water -> Brew -> Pour -> Add Condiments.
2. **`DataMinerExample.cs`**
   - **Scenario**: Parsing data from PDF and CSV files.
   - **Template Sequence**: Open File -> Extract -> Parse -> Save DB -> Close File.
3. **`SoftwareBuilderExample.cs`**
   - **Scenario**: CI/CD Pipeline for Android and iOS apps.
   - **Template Sequence**: Lint Code -> Compile -> Run Tests -> Deploy.
4. **`GameAIExample.cs`**
   - **Scenario**: Turn-based strategy game AI (Orcs vs Elves).
   - **Template Sequence**: Collect Resources -> Build Structures -> Build Units -> Attack.
5. **`ReportGeneratorExample.cs`**
   - **Scenario**: Generating HTML and PDF reports.
   - **Template Sequence**: Format Header -> Format Body -> Format Footer.
6. **`DatabaseQueryRunnerExample.cs`**
   - **Scenario**: Executing queries against SQL Server and MongoDB.
   - **Template Sequence**: Connect -> Run Command -> Disconnect.
