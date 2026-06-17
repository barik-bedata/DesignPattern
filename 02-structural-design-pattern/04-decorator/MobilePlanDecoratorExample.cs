using System;

namespace DecoratorPattern.MobilePlan
{
    // ১. Component Interface
    public interface IMobilePlan
    {
        string GetPlanDetails();
        double GetCost();
    }

    // ২. Concrete Component (বেসিক প্যাকেজ)
    public class BasicTalktimePlan : IMobilePlan
    {
        public string GetPlanDetails() => "বেসিক টকটাইম প্ল্যান (100 Min)";
        public double GetCost() => 80.00; // ৮০ টাকা
    }

    // ৩. Decorator
    public abstract class MobilePlanDecorator : IMobilePlan
    {
        protected IMobilePlan _plan;
        public MobilePlanDecorator(IMobilePlan plan) => _plan = plan;
        
        public virtual string GetPlanDetails() => _plan.GetPlanDetails();
        public virtual double GetCost() => _plan.GetCost();
    }

    // ৪. Concrete Decorator (ইন্টারনেট প্যাক)
    public class InternetPackDecorator : MobilePlanDecorator
    {
        public InternetPackDecorator(IMobilePlan plan) : base(plan) { }
        
        public override string GetPlanDetails() => base.GetPlanDetails() + " + 1GB Data";
        public override double GetCost() => base.GetCost() + 40.00; 
    }

    // ৪. Concrete Decorator (এসএমএস প্যাক)
    public class SmsPackDecorator : MobilePlanDecorator
    {
        public SmsPackDecorator(IMobilePlan plan) : base(plan) { }
        
        public override string GetPlanDetails() => base.GetPlanDetails() + " + 100 SMS";
        public override double GetCost() => base.GetCost() + 10.00; 
    }

    // Client Code (For testing)
    class Program
    {
        static void Run()
        {
            IMobilePlan myPlan = new BasicTalktimePlan();
            myPlan = new InternetPackDecorator(myPlan);
            myPlan = new SmsPackDecorator(myPlan);
            
            Console.WriteLine($"{myPlan.GetPlanDetails()} | Total: BDT {myPlan.GetCost()}");
        }
    }
}
