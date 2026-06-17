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
// ✅ SOLUTION: The Good Way (Using 4 Prototype Components)
// ==============================================================
namespace PrototypePattern.CVTemplate.Solution
{
    // ==============================================================
    // ১. Prototype Interface
    // ==============================================================
    public interface IResumePrototype
    {
        IResumePrototype Clone();
        void SetObjective(string objective);
        void Show();
    }

    // ==============================================================
    // ২. Concrete Prototype
    // ==============================================================
    public class Resume : IResumePrototype
    {
        public string Education { get; private set; }
        public string Skills { get; private set; }
        public string Experience { get; private set; }
        public string Objective { get; private set; }

        public Resume()
        {
            Console.WriteLine("\n>> [Solution] জীবনে একবারই মাস্টার CV টাইপ করা হচ্ছে... (Taking 2 Hours)");
            Thread.Sleep(500);

            Education = "B.Sc in CSE from BUET";
            Skills = "C#, .NET, Angular, SQL Server, Docker, Kubernetes";
            Experience = "3 Years as Software Engineer";
        }

        public void SetObjective(string objective)
        {
            Objective = objective;
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

    // ==============================================================
    // ৩. Client (JobApplicant)
    // ==============================================================
    public class JobApplicant
    {
        private IResumePrototype _masterResume;

        public JobApplicant(IResumePrototype masterResume)
        {
            _masterResume = masterResume;
        }

        public IResumePrototype ApplyForJob(string objective)
        {
            var clone = _masterResume.Clone();
            clone.SetObjective(objective);
            return clone;
        }
    }

    // ==============================================================
    // ৪. Main Class (SolutionRunner)
    // ==============================================================
    public class SolutionRunner
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ✅ SOLUTION RUN: Prototype Pattern (Using 4 Components) ===");
            
            // মাস্টার CV তৈরি
            var masterCV = new Resume();

            // ক্যান্ডিডেটকে (ক্লায়েন্ট) মাস্টার CV দিয়ে দিলাম
            var applicant = new JobApplicant(masterCV);

            // শুধু Objective চেঞ্জ করে ক্লোন করা হচ্ছে
            var googleCV = applicant.ApplyForJob("To work at Google");
            googleCV.Show();

            var microsoftCV = applicant.ApplyForJob("To work at Microsoft");
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
