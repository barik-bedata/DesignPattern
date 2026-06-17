using System;

namespace DecoratorPattern.Fuchka
{
    // ১. Component Interface (সবাইকে এই নিয়ম মানতে হবে)
    public interface IFuchka
    {
        string GetDescription();
        double GetPrice();
    }

    // ২. Concrete Component (মূল ফুচকা)
    public class RegularFuchka : IFuchka
    {
        public string GetDescription() => "রেগুলার ফুচকা (আলু-মটর ও টক দিয়ে)";
        public double GetPrice() => 50.00; // ৫০ টাকা
    }

    // ৩. Decorator (অ্যাবস্ট্রাক্ট র‍্যাপার)
    public abstract class FuchkaDecorator : IFuchka
    {
        protected IFuchka _fuchka;
        public FuchkaDecorator(IFuchka fuchka) => _fuchka = fuchka;
        
        public virtual string GetDescription() => _fuchka.GetDescription();
        public virtual double GetPrice() => _fuchka.GetPrice();
    }

    // ৪. Concrete Decorator (ডিম ফুচকা)
    public class DimFuchkaDecorator : FuchkaDecorator
    {
        public DimFuchkaDecorator(IFuchka fuchka) : base(fuchka) { }
        
        public override string GetDescription() => base.GetDescription() + " + গ্রেট করা ডিম";
        public override double GetPrice() => base.GetPrice() + 10.00; // ডিমের দাম ১০ টাকা
    }

    // ৪. Concrete Decorator (দই ফুচকা)
    public class DoiFuchkaDecorator : FuchkaDecorator
    {
        public DoiFuchkaDecorator(IFuchka fuchka) : base(fuchka) { }
        
        public override string GetDescription() => base.GetDescription() + " + মিষ্টি দই";
        public override double GetPrice() => base.GetPrice() + 20.00; // দইয়ের দাম ২০ টাকা
    }

    // Client Code (For testing)
    class Program
    {
        static void Run()
        {
            IFuchka myFuchka = new RegularFuchka();
            myFuchka = new DimFuchkaDecorator(myFuchka); // ডিম অ্যাড করলাম
            myFuchka = new DoiFuchkaDecorator(myFuchka); // দই অ্যাড করলাম
            
            Console.WriteLine($"{myFuchka.GetDescription()} | Total: BDT {myFuchka.GetPrice()}");
        }
    }
}
