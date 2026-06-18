using System;

namespace BehavioralDesignPattern.Mediator.UIDialog
{
    // 1. Mediator Interface (ডায়ালগের নিয়ম)
    public interface IDialogController
    {
        void Notify(IUIComponent sender, string eventType);
    }

    // 2. Colleague Interface (UI কম্পোনেন্টগুলোর কমন চেহারা)
    public interface IUIComponent
    {
        void Changed();
    }

    // 3. Concrete Mediator (আসল ডায়ালগ বা ফর্ম কন্ট্রোলার)
    public class RegistrationForm : IDialogController
    {
        public Checkbox TermsCheckbox { get; set; }
        public SubmitButton RegisterButton { get; set; }

        public void Notify(IUIComponent sender, string eventType)
        {
            // চেকবক্সে ক্লিক হলে সাবমিট বাটন এনাবল/ডিজেবল হবে।
            // বাটন এবং চেকবক্স একে অপরকে চেনে না, ফর্ম কন্ট্রোলার সব ম্যানেজ করছে।
            if (sender == TermsCheckbox && eventType == "CheckChanged")
            {
                if (TermsCheckbox.IsChecked)
                {
                    Console.WriteLine("[Dialog Controller] Terms accepted. Enabling Submit button.");
                    RegisterButton.Enable();
                }
                else
                {
                    Console.WriteLine("[Dialog Controller] Terms rejected. Disabling Submit button.");
                    RegisterButton.Disable();
                }
            }
        }
    }

    // 4. Concrete Colleagues (আসল UI কম্পোনেন্ট)
    public class Checkbox : IUIComponent
    {
        private readonly IDialogController _dialog;
        public bool IsChecked { get; private set; }

        public Checkbox(IDialogController dialog) { _dialog = dialog; }

        public void Toggle()
        {
            IsChecked = !IsChecked;
            Changed();
        }

        public void Changed() => _dialog.Notify(this, "CheckChanged");
    }

    public class SubmitButton : IUIComponent
    {
        private readonly IDialogController _dialog;
        public bool IsEnabled { get; private set; }

        public SubmitButton(IDialogController dialog) { _dialog = dialog; }

        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
        public void Changed() { }
    }

    // ==========================================
    // 5. Client (মেইন অ্যাপ্লিকেশন)
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== Mediator Pattern (UI Dialog) ===\n");

            RegistrationForm form = new RegistrationForm();
            
            Checkbox terms = new Checkbox(form);
            SubmitButton submit = new SubmitButton(form);

            form.TermsCheckbox = terms;
            form.RegisterButton = submit;

            // ইউজার চেকবক্স ক্লিক করলো, ফর্ম কন্ট্রোলার সাবমিট বাটন এনাবল করে দেবে
            terms.Toggle(); 
        }
    }
}
