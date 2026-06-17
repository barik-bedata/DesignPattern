using System;

namespace FacadePattern.VideoStreaming
{
    // ==========================================
    // 1. Interfaces for Subsystems (DIP মানার জন্য)
    // ==========================================
    public interface IVideoEncoder { void Encode(string file, string format); }
    public interface IVideoBuffer { void LoadToMemory(string file); }
    public interface IVideoPlayer { void Play(string file); }

    // ==========================================
    // 2. Concrete Subsystems
    // ==========================================
    public class Mp4Encoder : IVideoEncoder
    {
        public void Encode(string file, string format) => Console.WriteLine($"[Encoder] Encoding '{file}' to {format} format...");
    }

    public class CloudBuffer : IVideoBuffer
    {
        public void LoadToMemory(string file) => Console.WriteLine($"[Buffer] Loading '{file}' chunks into memory for smooth playback...");
    }

    public class Html5Player : IVideoPlayer
    {
        public void Play(string file) => Console.WriteLine($"[Player] Now playing '{file}' in full HD.\n");
    }

    // ==========================================
    // 3. The Facade (সিম্পল ইন্টারফেস)
    // ==========================================
    public class VideoStreamingFacade
    {
        // DIP Followed
        private readonly IVideoEncoder _encoder;
        private readonly IVideoBuffer _buffer;
        private readonly IVideoPlayer _player;

        public VideoStreamingFacade(IVideoEncoder encoder, IVideoBuffer buffer, IVideoPlayer player)
        {
            _encoder = encoder;
            _buffer = buffer;
            _player = player;
        }

        // ক্লায়েন্ট শুধু প্লে বাটনে চাপবে, আর ফ্যাসাড সব করে দেবে!
        public void StreamVideo(string movieName)
        {
            Console.WriteLine($"\n=== [Facade] Preparing to stream '{movieName}' ===");
            // ক্লায়েন্টের হয়ে সব ব্যাকগ্রাউন্ড কাজ Facade নিজে করছে
            _encoder.Encode(movieName, "MP4");
            _buffer.LoadToMemory(movieName);
            _player.Play(movieName);
        }
    }

    // ==========================================
    // 4. Client Code
    // ==========================================
    class Program
    {
        static void Run()
        {
            // সাব-সিস্টেম তৈরি 
            IVideoEncoder encoder = new Mp4Encoder();
            IVideoBuffer buffer = new CloudBuffer();
            IVideoPlayer player = new Html5Player();

            // Facade এ ইনজেক্ট (DI)
            VideoStreamingFacade netflixStreaming = new VideoStreamingFacade(encoder, buffer, player);

            // Client শুধু প্লে বাটনে চাপ দিচ্ছে!
            netflixStreaming.StreamVideo("The Dark Knight");
        }
    }
}
