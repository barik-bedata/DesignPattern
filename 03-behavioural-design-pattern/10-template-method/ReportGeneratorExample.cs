using System;

namespace BehavioralDesignPattern.TemplateMethod.ReportGenerator
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // রিপোর্ট জেনারেট করার বেস ক্লাস।
    public abstract class ReportGenerator
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // একটি রিপোর্টের হেডার, বডি এবং ফুটার থাকে। এই সিকোয়েন্স ফিক্সড।
        public void GenerateReport()
        {
            FormatHeader();
            FormatBody();
            FormatFooter();
            Console.WriteLine("Report Generation Complete!\n");
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // HTML নাকি PDF হবে, তার ওপর ভিত্তি করে ফরম্যাটিং লজিক চেঞ্জ হবে।
        protected abstract void FormatHeader();
        protected abstract void FormatBody();
        protected abstract void FormatFooter();
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // HTML রিপোর্ট বানানোর লজিক
    public class HtmlReportGenerator : ReportGenerator
    {
        protected override void FormatHeader()
        {
            Console.WriteLine("[HTML] <header> Report Title </header>");
        }

        protected override void FormatBody()
        {
            Console.WriteLine("[HTML] <body> Report Content Data </body>");
        }

        protected override void FormatFooter()
        {
            Console.WriteLine("[HTML] <footer> Page 1 </footer>");
        }
    }

    // PDF রিপোর্ট বানানোর লজিক
    public class PdfReportGenerator : ReportGenerator
    {
        protected override void FormatHeader()
        {
            Console.WriteLine("[PDF] Rendering PDF Header Text");
        }

        protected override void FormatBody()
        {
            Console.WriteLine("[PDF] Rendering PDF Body Tables");
        }

        protected override void FormatFooter()
        {
            Console.WriteLine("[PDF] Rendering PDF Footer Page Number");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Report Generator) ===\n");

            ReportGenerator htmlReport = new HtmlReportGenerator();
            htmlReport.GenerateReport();

            ReportGenerator pdfReport = new PdfReportGenerator();
            pdfReport.GenerateReport();
        }
    }
}
