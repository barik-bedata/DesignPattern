using System;

namespace DecoratorPattern.WeddingStage
{
    // ১. Component Interface
    public interface IWeddingStage
    {
        string GetDecorationDetails();
        int GetTotalCost();
    }

    // ২. Concrete Component (বেসিক স্টেজ)
    public class BasicStage : IWeddingStage
    {
        public string GetDecorationDetails() => "সাধারণ স্টেজ (কিছু কাপড় ও সাধারণ চেয়ার)";
        public int GetTotalCost() => 15000; // ১৫,০০০ টাকা
    }

    // ৩. Decorator
    public abstract class StageDecorator : IWeddingStage
    {
        protected IWeddingStage _stage;
        public StageDecorator(IWeddingStage stage) => _stage = stage;
        
        public virtual string GetDecorationDetails() => _stage.GetDecorationDetails();
        public virtual int GetTotalCost() => _stage.GetTotalCost();
    }

    // ৪. Concrete Decorator (কাঁচা ফুলের সাজ)
    public class FreshFlowerDecorator : StageDecorator
    {
        public FreshFlowerDecorator(IWeddingStage stage) : base(stage) { }
        
        public override string GetDecorationDetails() => base.GetDecorationDetails() + " + কাঁচা ফুলের ডেকোরেশন";
        public override int GetTotalCost() => base.GetTotalCost() + 10000; 
    }

    // ৪. Concrete Decorator (এলইডি লাইটিং)
    public class LedLightingDecorator : StageDecorator
    {
        public LedLightingDecorator(IWeddingStage stage) : base(stage) { }
        
        public override string GetDecorationDetails() => base.GetDecorationDetails() + " + প্রিমিয়াম LED লাইটিং";
        public override int GetTotalCost() => base.GetTotalCost() + 5000; 
    }

    // Client Code (For testing)
    class Program
    {
        static void Run()
        {
            IWeddingStage stage = new BasicStage();
            stage = new FreshFlowerDecorator(stage);
            stage = new LedLightingDecorator(stage);
            
            Console.WriteLine($"{stage.GetDecorationDetails()} | Total Cost: BDT {stage.GetTotalCost()}");
        }
    }
}
