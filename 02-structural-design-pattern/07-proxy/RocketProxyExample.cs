using System;
using System.Threading;

namespace ProxyPattern.RealWorld.Rocket
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface IRocketStatement
    {
        void DownloadPDF();
    }

    // ==========================================
    // 2. Real Subject
    // ==========================================
    // এটি হলো অরিজিনাল স্টেটমেন্ট জেনারেটর। এটি তৈরি হতে প্রচুর সময় এবং মেমোরি লাগে।
    public class RealRocketStatement : IRocketStatement
    {
        private string _accountNumber;

        public RealRocketStatement(string accountNumber)
        {
            _accountNumber = accountNumber;
            Console.WriteLine($"[Rocket Server] ⏳ Generating 10-page heavy PDF statement for {_accountNumber}...");
            Thread.Sleep(3000); // ভারী কাজ হচ্ছে সিমুলেট করছি
            Console.WriteLine($"[Rocket Server] ✅ PDF Generation Complete.");
        }

        public void DownloadPDF()
        {
            Console.WriteLine($"[Rocket Server] 📥 Downloading Statement.pdf to your device...");
        }
    }

    // ==========================================
    // 3. Proxy (Virtual Proxy)
    // ==========================================
    // ইউজার "Statements" পেজে ঢুকলেই রকেট অ্যাপ স্টেটমেন্ট জেনারেট করে না। 
    // যখন ইউজার "Download" বাটনে ক্লিক করে, ঠিক তখনই প্রক্সি আসল অবজেক্ট তৈরি করে (Lazy Loading)।
    public class RocketAppProxy : IRocketStatement
    {
        private RealRocketStatement _realStatement;
        private string _accountNumber;

        public RocketAppProxy(string accountNumber)
        {
            _accountNumber = accountNumber;
            Console.WriteLine($"[Rocket App] Entered statement menu for {_accountNumber}. (No PDF generated yet!)");
        }

        public void DownloadPDF()
        {
            // ইউজার ডাউনলোড বাটনে ক্লিক করার পর অবজেক্ট তৈরি হচ্ছে!
            if (_realStatement == null)
            {
                _realStatement = new RealRocketStatement(_accountNumber);
            }
            
            _realStatement.DownloadPDF();
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 4. Rocket Virtual Proxy Example (Lazy Loading) ===\n");

            // ইউজার অ্যাপের মেনু ব্রাউজ করছে, অবজেক্ট শুধু ইনিশিয়ালাইজ হয়েছে, কিন্তু ভারী কাজ এখনো হয়নি
            IRocketStatement rocketApp = new RocketAppProxy("DBBL-102030");
            
            Console.WriteLine("\n[User] Reading menu... deciding whether to download...");
            Thread.Sleep(1000);

            Console.WriteLine("\n[User] Clicks 'Download Statement' button!");
            // এবার আসল ভারী কাজটা হবে
            rocketApp.DownloadPDF();
        }
    }
}
