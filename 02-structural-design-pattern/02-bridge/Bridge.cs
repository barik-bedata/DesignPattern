using System;

namespace BridgePatternExample
{
    // ==========================================
    // IMPLEMENTATION HIERARCHY (কীভাবে ফরম্যাট হবে)
    // ==========================================

    // ৩. Implementor
    public interface IFormatter
    {
        string Format(string data);
    }

    // ৪. Concrete Implementors
    public class CsvFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"[CSV Formatted] {data}";
        }
    }

    public class JsonFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"{{ \"data\": \"{data}\" }}";
        }
    }

    public class XmlFormatter : IFormatter
    {
        public string Format(string data)
        {
            return $"<data>{data}</data>";
        }
    }

    // ==========================================
    // ABSTRACTION HIERARCHY (কী এক্সপোর্ট হবে)
    // ==========================================

    // ১. Abstraction (Bridge তৈরি হচ্ছে IFormatter এর সাথে)
    public abstract class Exporter
    {
        // এটাই হলো সেই 'Bridge' বা Composition
        protected IFormatter _formatter;

        protected Exporter(IFormatter formatter)
        {
            _formatter = formatter;
        }

        // রানটাইমে ফরম্যাটার চেঞ্জ করার সুবিধা (Optional)
        public void SetFormatter(IFormatter formatter)
        {
            _formatter = formatter;
        }

        public abstract void Export();
    }

    // ২. Refined Abstraction
    public class UserExporter : Exporter
    {
        public UserExporter(IFormatter formatter) : base(formatter) { }

        public override void Export()
        {
            string rawData = "User1, User2, User3";
            string formattedData = _formatter.Format(rawData);
            Console.WriteLine($"Exporting Users -> {formattedData}");
        }
    }

    public class ProductExporter : Exporter
    {
        public ProductExporter(IFormatter formatter) : base(formatter) { }

        public override void Export()
        {
            string rawData = "Laptop, Mouse, Keyboard";
            string formattedData = _formatter.Format(rawData);
            Console.WriteLine($"Exporting Products -> {formattedData}");
        }
    }

    // ==========================================
    // CLIENT CODE
    // ==========================================
    class Program
    {
        static void Main()
        {
            // আমরা Product Export করতে চাই JSON ফরম্যাটে
            IFormatter jsonFormatter = new JsonFormatter();
            Exporter productExporter = new ProductExporter(jsonFormatter);
            productExporter.Export();

            // হঠাৎ বসের মন চাইলো User Export করবে XML ফরম্যাটে
            IFormatter xmlFormatter = new XmlFormatter();
            Exporter userExporter = new UserExporter(xmlFormatter);
            userExporter.Export();

            // একই User Exporter দিয়ে এখন CSV তে এক্সপোর্ট করবো (Bridge এর ম্যাজিক)
            userExporter.SetFormatter(new CsvFormatter());
            userExporter.Export();
        }
    }
}

/*
======================================================================
WITHOUT BRIDGE PATTERN (The Problem / ক্লাসের বিস্ফোরণ)
======================================================================
ব্রিজ প্যাটার্ন ব্যবহার না করলে আমাদের Inheritance ব্যবহার করতে হতো। 
ফলে প্রতিটা Entity (User, Product) এবং Format (CSV, JSON, XML) এর 
কম্বিনেশনের জন্য আলাদা আলাদা ক্লাস তৈরি করতে হতো। 

নিচে এর উদাহরণ দেওয়া হলো (কোডটি কমেন্ট করে রাখা হয়েছে):

    public abstract class OldExporter
    {
        public abstract void Export();
    }

    // --- User Exporters ---
    public class UserCsvExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Users -> [CSV Formatted] User1, User2");
        }
    }

    public class UserJsonExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Users -> { \"data\": \"User1, User2\" }");
        }
    }

    public class UserXmlExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Users -> <data>User1, User2</data>");
        }
    }

    // --- Product Exporters ---
    public class ProductCsvExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Products -> [CSV Formatted] Laptop, Mouse");
        }
    }

    public class ProductJsonExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Products -> { \"data\": \"Laptop, Mouse\" }");
        }
    }

    public class ProductXmlExporter : OldExporter
    {
        public override void Export()
        {
            Console.WriteLine("Exporting Products -> <data>Laptop, Mouse</data>");
        }
    }

    // সমস্যাটা কোথায়? 
    // এখন যদি নতুন একটি 'Company' এক্সপোর্ট করতে হয়, তবে আমাদের আরও ৩টি নতুন ক্লাস বানাতে হবে!
    // CompanyCsvExporter, CompanyJsonExporter, CompanyXmlExporter.
    // এবং নতুন কোনো ফরম্যাট আসলে (যেমন: PDF), তখন সব Entity এর জন্য আবার PDF ক্লাস বানাতে হবে।
    // একেই বলা হয় Class Explosion! যা Bridge Pattern এর মাধ্যমে সমাধান করা হয়েছে।
======================================================================
*/
