using System;

namespace AdapterPattern.RealWorldExamples
{
    // ১. Target Interface (আমাদের ওয়েবসাইটের নিজস্ব লগইন সিস্টেম)
    public interface ILoginSystem
    {
        void Login(string email, string password);
    }

    public class EmailLogin : ILoginSystem
    {
        public void Login(string email, string password)
        {
            Console.WriteLine($"Logged in using Email: {email}");
        }
    }

    // ২. Adaptee (Google-এর নিজস্ব লগইন API)
    public class GoogleAuthApi
    {
        public void LoginWithGoogleToken(string token)
        {
            Console.WriteLine($"Google Auth successful! Token Validated: {token}");
        }
    }

    // ৩. Adapter (যাতে Google লগইন আমাদের পুরনো ILoginSystem সাপোর্ট করে)
    public class GoogleAuthAdapter : ILoginSystem
    {
        private GoogleAuthApi _googleApi;
        private string _googleToken;

        public GoogleAuthAdapter(GoogleAuthApi googleApi, string googleToken)
        {
            _googleApi = googleApi;
            _googleToken = googleToken;
        }

        // ILoginSystem এর Login মেথড
        public void Login(string email, string password)
        {
            Console.WriteLine("Adapter: Ignored email and password. Using Google Token instead...");
            // ভেতরে Google-এর API কল করা হচ্ছে
            _googleApi.LoginWithGoogleToken(_googleToken);
        }
    }

    // Client Code
    public class AppLogin
    {
        public static void Main()
        {
            Console.WriteLine("--- App Login ---");

            // সাধারণ ইমেইল লগইন
            ILoginSystem normalLogin = new EmailLogin();
            normalLogin.Login("user@example.com", "password123");

            // Google দিয়ে লগইন (Adapter এর মাধ্যমে)
            GoogleAuthApi googleApi = new GoogleAuthApi();
            ILoginSystem googleLogin = new GoogleAuthAdapter(googleApi, "ABCDEF123456TOKEN");
            
            // আমাদের সিস্টেম তার চেনা Login মেথড কল করছে, কিন্তু ভেতরে Google কাজ করছে
            googleLogin.Login("", ""); 
        }
    }
}
