using System;

namespace BehavioralDesignPattern.Strategy.NavigationApp
{
    // ==========================================
    // 2. Strategy Interface
    // ==========================================
    // Defines a common interface that all concrete strategies must implement.
    public interface IRouteStrategy
    {
        void BuildRoute(string source, string destination);
    }

    // ==========================================
    // 3. Concrete Strategies
    // ==========================================
    // Provide specific implementations of the strategy interface with different algorithms or behaviors.
    public class DrivingRouteStrategy : IRouteStrategy
    {
        public void BuildRoute(string source, string destination)
        {
            Console.WriteLine($"[Driving Route] Calculating fastest route for cars from '{source}' to '{destination}' avoiding heavy traffic.");
        }
    }

    public class WalkingRouteStrategy : IRouteStrategy
    {
        public void BuildRoute(string source, string destination)
        {
            Console.WriteLine($"[Walking Route] Calculating shortest pedestrian path from '{source}' to '{destination}'.");
        }
    }

    public class PublicTransitRouteStrategy : IRouteStrategy
    {
        public void BuildRoute(string source, string destination)
        {
            Console.WriteLine($"[Transit Route] Finding available buses and trains from '{source}' to '{destination}'.");
        }
    }

    // ==========================================
    // 1. Context
    // ==========================================
    // Acts as an intermediary between the client and the strategy, delegating tasks to the selected strategy.
    public class Navigator
    {
        private IRouteStrategy _routeStrategy;

        public void SetStrategy(IRouteStrategy strategy)
        {
            _routeStrategy = strategy;
        }

        public void CalculateRoute(string source, string destination)
        {
            if (_routeStrategy == null)
            {
                Console.WriteLine("Error: Route strategy not set.");
                return;
            }

            Console.WriteLine($"\n--> [Context] Delegating route calculation to {_routeStrategy.GetType().Name}...");
            _routeStrategy.BuildRoute(source, destination);
        }
    }

    // ==========================================
    // 4. Client
    // ==========================================
    // Responsible for selecting and configuring the appropriate strategy for the context.
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Strategy Pattern (Navigation App) ===\n");

            Navigator navigator = new Navigator();
            string source = "Gulshan-1";
            string destination = "Dhanmondi-27";

            // Client chooses driving strategy
            navigator.SetStrategy(new DrivingRouteStrategy());
            navigator.CalculateRoute(source, destination);

            // Client changes mind and chooses walking strategy
            navigator.SetStrategy(new WalkingRouteStrategy());
            navigator.CalculateRoute(source, destination);

            // Client chooses public transit strategy
            navigator.SetStrategy(new PublicTransitRouteStrategy());
            navigator.CalculateRoute(source, destination);
        }
    }
}
