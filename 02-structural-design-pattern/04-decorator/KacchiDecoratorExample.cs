using System;

namespace DecoratorPattern.Kacchi
{
    // ১. Component Interface
    public interface IKacchi
    {
        string GetItems();
        double GetPrice();
    }

    // ২. Concrete Component (রেগুলার কাচ্চি)
    public class RegularKacchi : IKacchi
    {
        public string GetItems() => "হাফ কাচ্চি (১ পিস খাসি, ১ পিস আলু)";
        public double GetPrice() => 220.00; // ২২০ টাকা
    }

    // ৩. Decorator
    public abstract class KacchiDecorator : IKacchi
    {
        protected IKacchi _kacchi;
        public KacchiDecorator(IKacchi kacchi) => _kacchi = kacchi;
        
        public virtual string GetItems() => _kacchi.GetItems();
        public virtual double GetPrice() => _kacchi.GetPrice();
    }

    // ৪. Concrete Decorator (এক্সট্রা আলু)
    public class ExtraAluDecorator : KacchiDecorator
    {
        public ExtraAluDecorator(IKacchi kacchi) : base(kacchi) { }
        
        public override string GetItems() => base.GetItems() + " + এক্সট্রা আলু";
        public override double GetPrice() => base.GetPrice() + 20.00; // ২০ টাকা
    }

    // ৪. Concrete Decorator (বোরহানি)
    public class BorhaniDecorator : KacchiDecorator
    {
        public BorhaniDecorator(IKacchi kacchi) : base(kacchi) { }
        
        public override string GetItems() => base.GetItems() + " + বোরহানি";
        public override double GetPrice() => base.GetPrice() + 50.00; // ৫০ টাকা
    }

    // Client Code (For testing)
    class Program
    {
        static void Run()
        {
            IKacchi myKacchi = new RegularKacchi();
            myKacchi = new ExtraAluDecorator(myKacchi);
            myKacchi = new BorhaniDecorator(myKacchi);
            
            Console.WriteLine($"{myKacchi.GetItems()} | Total Price: BDT {myKacchi.GetPrice()}");
        }
    }
}
