using System;
using System.Threading;

// ==============================================================
// ❌ VIOLATION: The Bad Way (প্রতিটি চাকরির জন্য নতুন CV বানানো)
// ==============================================================

namespace PrototypePattern.CVTemplate.Violation
{
    public class Resume
    {
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }
        public string Objective { get; set; }

        public Resume(string objective)
        {
            Console.WriteLine(">> [Violation] পুরো CV নতুন করে টাইপ করা হচ্ছে... (Taking 2 Hours)");
            Thread.Sleep(500);

            Education = "B.Sc in CSE from BUET";
            Skills = "C#, .NET, Angular, SQL Server, Docker, Kubernetes";
            Experience = "3 Years as Software Engineer";
            Objective = objective;
        }

        public void Show()
        {
            Console.WriteLine($"[CV] Objective: {Objective}\n[Skills] {Skills}\n");
        }
    }

    public class ViolationRunner
    {
        public static void Run()
        {
            Console.WriteLine("=== ❌ VIOLATION RUN: প্রতিটি চাকরির অ্যাপ্লিকেশনে নতুন CV টাইপ করা ===");
            var googleCV = new Resume("To work at Google");
            googleCV.Show();

            var microsoftCV = new Resume("To work at Microsoft");
            microsoftCV.Show();
        }
    }
}

// ==============================================================
// ✅ SOLUTION: The Good Way (মাস্টার CV কে ক্লোন করা)
// ==============================================================

namespace PrototypePattern.CVTemplate.Solution
{
    public interface IResumePrototype
    {
        IResumePrototype Clone();
    }

    public class Resume : IResumePrototype
    {
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }
        public string Objective { get; set; }

        public Resume()
        {
            Console.WriteLine("\n>> [Solution] জীবনে একবারই মাস্টার CV টাইপ করা হচ্ছে... (Taking 2 Hours)");
            Thread.Sleep(500);

            Education = "B.Sc in CSE from BUET";
            Skills = "C#, .NET, Angular, SQL Server, Docker, Kubernetes";
            Experience = "3 Years as Software Engineer";
        }

        public IResumePrototype Clone()
        {
            return (IResumePrototype)this.MemberwiseClone();
        }

        public void Show()
        {
            Console.WriteLine($"[CV] Objective: {Objective}\n[Skills] {Skills}\n");
        }
    }

    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: মাস্টার CV ক্লোন করে শুধু Objective চেঞ্জ করা ===");
            
            var masterCV = new Resume();

            var googleCV = (Resume)masterCV.Clone();
            googleCV.Objective = "To work at Google";
            googleCV.Show();

            var microsoftCV = (Resume)masterCV.Clone();
            microsoftCV.Objective = "To work at Microsoft";
            microsoftCV.Show();
        }
    }
}

class Program
{
    static void Main()
    {
        PrototypePattern.CVTemplate.Violation.ViolationRunner.Run();
        PrototypePattern.CVTemplate.Solution.SolutionRunner.Run();
    }
}
