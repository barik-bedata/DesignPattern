using System;

namespace BehavioralDesignPattern.Strategy.Sorting
{
    // ==========================================
    // 2. Strategy Interface
    // ==========================================
    // Defines a common interface that all concrete strategies must implement.
    // Ensures consistency so all strategies are interchangeable.
    public interface ISortingStrategy
    {
        void Sort(int[] array);
    }

    // ==========================================
    // 3. Concrete Strategies
    // ==========================================
    // Provide specific implementations of the strategy interface with different algorithms or behaviors.
    // Encapsulate the actual logic of each algorithm.
    public class BubbleSortStrategy : ISortingStrategy
    {
        public void Sort(int[] array)
        {
            // Implement Bubble Sort algorithm
            Console.WriteLine("[Bubble Sort] Sorting array: " + string.Join(", ", array));
        }
    }

    public class MergeSortStrategy : ISortingStrategy
    {
        public void Sort(int[] array)
        {
            // Implement Merge Sort algorithm
            Console.WriteLine("[Merge Sort] Sorting array: " + string.Join(", ", array));
        }
    }

    public class QuickSortStrategy : ISortingStrategy
    {
        public void Sort(int[] array)
        {
            // Implement Quick Sort algorithm
            Console.WriteLine("[Quick Sort] Sorting array: " + string.Join(", ", array));
        }
    }

    // ==========================================
    // 1. Context
    // ==========================================
    // Acts as an intermediary between the client and the strategy, delegating tasks to the selected strategy.
    // Holds a reference to a strategy object and uses it to perform operations.
    public class SortingContext
    {
        private ISortingStrategy _sortingStrategy;

        // Constructor to set initial strategy (Dependency Injection)
        public SortingContext(ISortingStrategy sortingStrategy)
        {
            _sortingStrategy = sortingStrategy;
        }

        // Setter to change strategy dynamically at runtime
        public void SetSortingStrategy(ISortingStrategy sortingStrategy)
        {
            _sortingStrategy = sortingStrategy;
        }

        public void PerformSort(int[] array)
        {
            if (_sortingStrategy == null)
            {
                Console.WriteLine("Error: Sorting strategy not configured.");
                return;
            }

            Console.WriteLine($"\n--> [Context] Delegating sort operation to {_sortingStrategy.GetType().Name}...");
            _sortingStrategy.Sort(array);
        }
    }

    // ==========================================
    // 4. Client
    // ==========================================
    // Responsible for selecting and configuring the appropriate strategy for the context.
    // Decides which strategy to use based on the problem.
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Strategy Pattern (Sorting Algorithms) ===\n");

            // Create SortingContext with BubbleSortStrategy
            SortingContext sortingContext = new SortingContext(new BubbleSortStrategy());
            int[] array1 = { 5, 2, 9, 1, 5 };
            sortingContext.PerformSort(array1); // Output: Sorting using Bubble Sort

            // Change strategy to MergeSortStrategy dynamically
            sortingContext.SetSortingStrategy(new MergeSortStrategy());
            int[] array2 = { 8, 3, 7, 4, 2 };
            sortingContext.PerformSort(array2); // Output: Sorting using Merge Sort

            // Change strategy to QuickSortStrategy dynamically
            sortingContext.SetSortingStrategy(new QuickSortStrategy());
            int[] array3 = { 6, 1, 3, 9, 5 };
            sortingContext.PerformSort(array3); // Output: Sorting using Quick Sort
        }
    }
}
