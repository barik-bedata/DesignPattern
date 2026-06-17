using System;

namespace ProxyPattern.VirtualProxy
{
    // ==========================================
    // 1. Subject Interface
    // ==========================================
    public interface IVideo
    {
        void Play();
    }

    // ==========================================
    // 2. Real Subject (Heavy Object)
    // ==========================================
    // এই অবজেক্টটি তৈরি হতে অনেক সময় লাগে (যেমন 1GB সাইজের ভিডিও ডিস্ক থেকে লোড করা)।
    public class RealHeavyVideo : IVideo
    {
        private string _fileName;

        public RealHeavyVideo(string fileName)
        {
            _fileName = fileName;
            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            Console.WriteLine($"[RealVideo] ⏳ Loading extremely heavy video '{_fileName}' from disk... (This blocks UI for 5 seconds)");
        }

        public void Play()
        {
            Console.WriteLine($"[RealVideo] ▶️ Playing video: '{_fileName}'");
        }
    }

    // ==========================================
    // 3. Virtual Proxy
    // ==========================================
    // প্রক্সি ক্লাসটি শুরুতেই ভিডিও লোড করে না (Lazy Initialization)। 
    // যখন ইউজার 'Play' বাটনে ক্লিক করে, ঠিক তখনই সে আসল ভিডিওটি লোড করে মেমোরি বাঁচায়।
    public class ProxyVideo : IVideo
    {
        private RealHeavyVideo _realVideo;
        private string _fileName;

        public ProxyVideo(string fileName)
        {
            _fileName = fileName;
            Console.WriteLine($"[Proxy] Created lightweight proxy for '{_fileName}'. Real video NOT loaded yet.");
        }

        public void Play()
        {
            // যদি রিয়েল ভিডিও অবজেক্ট তৈরি না হয়ে থাকে, তবেই তৈরি করো
            if (_realVideo == null)
            {
                _realVideo = new RealHeavyVideo(_fileName);
            }
            // আসল ভিডিওর কাছে রিকোয়েস্ট পাঠিয়ে দাও
            _realVideo.Play();
        }
    }

    // ==========================================
    // Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== 1. Virtual Proxy Example (Lazy Video Loading) ===\n");

            // ক্লায়েন্ট শুধু একটি ভিডিওর লিস্ট দেখতে চাচ্ছে। 
            // যদি আমরা RealHeavyVideo বানাতাম, তবে গ্যালারিতে ঢোকার আগেই ফোন হ্যাং হয়ে যেতো!
            IVideo video1 = new ProxyVideo("Avatar_1080p.mp4");
            IVideo video2 = new ProxyVideo("Inception_4k.mkv");

            Console.WriteLine("\n[Client] User is scrolling the gallery. No videos are loaded into RAM yet.");
            
            Console.WriteLine("\n[Client] User clicked PLAY on video 1.");
            // ঠিক এই মুহূর্তে ভিডিওটি লোড হবে এবং প্লে হবে
            video1.Play();

            Console.WriteLine("\n[Client] User clicked PLAY on video 1 again.");
            // এবার আর লোড হবে না, কারণ আগে থেকেই মেমোরিতে আছে!
            video1.Play();
        }
    }
}
