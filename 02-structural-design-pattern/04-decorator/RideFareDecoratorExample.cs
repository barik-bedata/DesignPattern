using System;

namespace DecoratorPattern.RideFare
{
    // ১. Component Interface
    public interface IRide
    {
        string GetRideDetails();
        double GetFare();
    }

    // ২. Concrete Component (বেসিক উবার রাইড)
    public class UberCarRide : IRide
    {
        public string GetRideDetails() => "বেসিক উবার কার রাইড (ধানমন্ডি টু গুলশান)";
        public double GetFare() => 300.00; // ৩০০ টাকা
    }

    // ৩. Decorator
    public abstract class RideDecorator : IRide
    {
        protected IRide _ride;
        public RideDecorator(IRide ride) => _ride = ride;
        
        public virtual string GetRideDetails() => _ride.GetRideDetails();
        public virtual double GetFare() => _ride.GetFare();
    }

    // ৪. Concrete Decorator (Peak Hour বা বৃষ্টির সময় সারচার্জ)
    public class PeakHourSurchargeDecorator : RideDecorator
    {
        public PeakHourSurchargeDecorator(IRide ride) : base(ride) { }
        
        public override string GetRideDetails() => base.GetRideDetails() + " + পিক আওয়ার সারচার্জ (বৃষ্টির কারণে)";
        public override double GetFare() => base.GetFare() + 100.00; // ১০০ টাকা এক্সট্রা
    }

    // ৪. Concrete Decorator (ফ্লাইওভার টোল)
    public class TollTaxDecorator : RideDecorator
    {
        public TollTaxDecorator(IRide ride) : base(ride) { }
        
        public override string GetRideDetails() => base.GetRideDetails() + " + হানিফ ফ্লাইওভার টোল";
        public override double GetFare() => base.GetFare() + 50.00; // ৫০ টাকা এক্সট্রা
    }

    // Client Code (For testing)
    class Program
    {
        static void Run()
        {
            IRide myRide = new UberCarRide();
            myRide = new PeakHourSurchargeDecorator(myRide);
            myRide = new TollTaxDecorator(myRide);
            
            Console.WriteLine($"{myRide.GetRideDetails()} | Final Fare: BDT {myRide.GetFare()}");
        }
    }
}
