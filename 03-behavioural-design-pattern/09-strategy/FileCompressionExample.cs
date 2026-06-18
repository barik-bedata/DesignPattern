using System;

namespace BehavioralDesignPattern.Strategy.FileCompression
{
    // ==========================================
    // 2. Strategy Interface
    // ==========================================
    // Defines a common interface that all concrete strategies must implement.
    public interface ICompressionStrategy
    {
        void CompressFiles(string[] files);
    }

    // ==========================================
    // 3. Concrete Strategies
    // ==========================================
    // Provide specific implementations of the strategy interface with different algorithms or behaviors.
    public class ZipCompressionStrategy : ICompressionStrategy
    {
        public void CompressFiles(string[] files)
        {
            Console.WriteLine($"[ZIP Compression] Compressing {files.Length} files into 'archive.zip' using ZIP algorithm.");
        }
    }

    public class RarCompressionStrategy : ICompressionStrategy
    {
        public void CompressFiles(string[] files)
        {
            Console.WriteLine($"[RAR Compression] Compressing {files.Length} files into 'archive.rar' using RAR algorithm.");
        }
    }

    public class TarGzCompressionStrategy : ICompressionStrategy
    {
        public void CompressFiles(string[] files)
        {
            Console.WriteLine($"[TAR.GZ Compression] Compressing {files.Length} files into 'archive.tar.gz' using GZip algorithm.");
        }
    }

    // ==========================================
    // 1. Context
    // ==========================================
    // Acts as an intermediary between the client and the strategy, delegating tasks to the selected strategy.
    public class CompressionContext
    {
        private ICompressionStrategy _strategy;

        public void SetCompressionStrategy(ICompressionStrategy strategy)
        {
            _strategy = strategy;
        }

        public void CreateArchive(string[] files)
        {
            if (_strategy == null)
            {
                Console.WriteLine("Error: Compression strategy not selected.");
                return;
            }

            Console.WriteLine($"\n--> [Context] Delegating file compression task to {_strategy.GetType().Name}...");
            _strategy.CompressFiles(files);
        }
    }

    // ==========================================
    // 4. Client
    // ==========================================
    // Responsible for selecting and configuring the appropriate strategy for the context.
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Strategy Pattern (File Compression Tool) ===\n");

            CompressionContext context = new CompressionContext();
            string[] filesToCompress = { "report.pdf", "photo.png", "video.mp4" };

            // Client selects ZIP format strategy
            context.SetCompressionStrategy(new ZipCompressionStrategy());
            context.CreateArchive(filesToCompress);

            // Client changes mind and selects RAR format strategy
            context.SetCompressionStrategy(new RarCompressionStrategy());
            context.CreateArchive(filesToCompress);
        }
    }
}
