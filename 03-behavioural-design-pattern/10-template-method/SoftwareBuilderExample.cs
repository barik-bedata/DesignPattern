using System;

namespace BehavioralDesignPattern.TemplateMethod.SoftwareBuilder
{
    // ==========================================
    // 1. Abstract Class
    // ==========================================
    // এটি হলো বেস ক্লাস যা সফটওয়্যার বিল্ড প্রসেসের স্ট্রাকচার ডিফাইন করে।
    public abstract class SoftwareBuilder
    {
        // ==========================================
        // 2. Template Method
        // ==========================================
        // এটি হলো বিল্ড পাইপলাইন। লিন্ট -> কম্পাইল -> টেস্ট -> ডেপ্লয়। এই সিকোয়েন্স কেউ ভাঙতে পারবে না।
        public void BuildSoftware()
        {
            LintCode();
            CompileCode();
            RunTests();
            Deploy();
        }

        // ==========================================
        // 3. Abstract/Hook Methods
        // ==========================================
        // প্লাটফর্ম ভেদে (Android/iOS) এগুলো আলাদা হবে।
        protected abstract void CompileCode();
        protected abstract void RunTests();
        protected abstract void Deploy();

        // Common method: সব প্রজেক্টেই কোড লিন্টিং একই রকম হয় ধরে নিলাম।
        private void LintCode()
        {
            Console.WriteLine("[Common] Running Code Linter... No errors found.");
        }
    }

    // ==========================================
    // 4. Concrete Subclasses
    // ==========================================
    // Android অ্যাপ বিল্ড করার স্পেসিফিক পদ্ধতি।
    public class AndroidBuilder : SoftwareBuilder
    {
        protected override void CompileCode()
        {
            Console.WriteLine("[Android] Compiling Kotlin code and generating APK.");
        }

        protected override void RunTests()
        {
            Console.WriteLine("[Android] Running Espresso UI tests.");
        }

        protected override void Deploy()
        {
            Console.WriteLine("[Android] Deploying APK to Google Play Store.\n");
        }
    }

    // iOS অ্যাপ বিল্ড করার স্পেসিফিক পদ্ধতি।
    public class IosBuilder : SoftwareBuilder
    {
        protected override void CompileCode()
        {
            Console.WriteLine("[iOS] Compiling Swift code and generating IPA.");
        }

        protected override void RunTests()
        {
            Console.WriteLine("[iOS] Running XCTest UI tests.");
        }

        protected override void Deploy()
        {
            Console.WriteLine("[iOS] Deploying IPA to Apple App Store.\n");
        }
    }

    // ==========================================
    // Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Template Method Pattern (Software Builder Pipeline) ===\n");

            SoftwareBuilder android = new AndroidBuilder();
            android.BuildSoftware();

            SoftwareBuilder ios = new IosBuilder();
            ios.BuildSoftware();
        }
    }
}
