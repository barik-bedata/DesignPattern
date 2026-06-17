using System;

namespace DecoratorPattern.Enterprise
{
    // ==========================================
    // 1. Strategies (Algorithms) - The Ultimate Solution!
    // ==========================================
    
    // এনক্রিপশনের জন্য ইন্টারফেস (Strategy Pattern)
    public interface IEncryptionStrategy
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }

    // AES এনক্রিপশন অ্যালগরিদম
    public class AesEncryptionStrategy : IEncryptionStrategy
    {
        public string Encrypt(string data) => $"***AES_ENCRYPTED({data})***";
        public string Decrypt(string data) => data.Replace("***AES_ENCRYPTED(", "").Replace(")***", "");
    }

    // কম্প্রেশনের জন্য ইন্টারফেস (Strategy Pattern)
    public interface ICompressionStrategy
    {
        string Compress(string data);
        string Decompress(string data);
    }

    // ZIP কম্প্রেশন অ্যালগরিদম
    public class ZipCompressionStrategy : ICompressionStrategy
    {
        public string Compress(string data) => $"[ZIP_COMPRESSED]{data}[/ZIP_COMPRESSED]";
        public string Decompress(string data) => data.Replace("[ZIP_COMPRESSED]", "").Replace("[/ZIP_COMPRESSED]", "");
    }


    // ==========================================
    // 2. Decorator Pattern Components (Pipeline)
    // ==========================================

    // ১. Component Interface (Core Data Source Service)
    public interface IDataSource
    {
        void WriteData(string data);
        string ReadData();
    }

    // ২. Concrete Component (বেসিক ফাইল রাইটার / রিডার)
    public class FileDataSource : IDataSource
    {
        private string _filename;
        private string _inMemoryStorage; 

        public FileDataSource(string filename)
        {
            _filename = filename;
        }

        public void WriteData(string data)
        {
            Console.WriteLine($"[FileDataSource] Writing raw data to {_filename}");
            _inMemoryStorage = data; 
        }

        public string ReadData()
        {
            Console.WriteLine($"[FileDataSource] Reading raw data from {_filename}");
            return _inMemoryStorage;
        }
    }

    // ৩. Decorator (Base Wrapper)
    public abstract class DataSourceDecorator : IDataSource
    {
        protected IDataSource _wrappee;

        public DataSourceDecorator(IDataSource source)
        {
            _wrappee = source;
        }

        public virtual void WriteData(string data) => _wrappee.WriteData(data);
        public virtual string ReadData() => _wrappee.ReadData();
    }

    // ৪. Concrete Decorator (Encryption)
    public class EncryptionDecorator : DataSourceDecorator
    {
        private readonly IEncryptionStrategy _encryptionStrategy;

        // Dependency Injection এর মাধ্যমে Strategy ইনজেক্ট করা হচ্ছে
        public EncryptionDecorator(IDataSource source, IEncryptionStrategy strategy) : base(source) 
        { 
            _encryptionStrategy = strategy;
        }

        public override void WriteData(string data)
        {
            Console.WriteLine($"[EncryptionDecorator] Encrypting data using {_encryptionStrategy.GetType().Name}...");
            string encryptedData = _encryptionStrategy.Encrypt(data);
            base.WriteData(encryptedData);
        }

        public override string ReadData()
        {
            string encryptedData = base.ReadData();
            Console.WriteLine($"[EncryptionDecorator] Decrypting data using {_encryptionStrategy.GetType().Name}...");
            return _encryptionStrategy.Decrypt(encryptedData);
        }
    }

    // ৪. Concrete Decorator (Compression)
    public class CompressionDecorator : DataSourceDecorator
    {
        private readonly ICompressionStrategy _compressionStrategy;

        // Dependency Injection এর মাধ্যমে Strategy ইনজেক্ট করা হচ্ছে
        public CompressionDecorator(IDataSource source, ICompressionStrategy strategy) : base(source) 
        { 
            _compressionStrategy = strategy;
        }

        public override void WriteData(string data)
        {
            Console.WriteLine($"[CompressionDecorator] Compressing data using {_compressionStrategy.GetType().Name}...");
            string compressedData = _compressionStrategy.Compress(data);
            base.WriteData(compressedData);
        }

        public override string ReadData()
        {
            string compressedData = base.ReadData();
            Console.WriteLine($"[CompressionDecorator] Decompressing data using {_compressionStrategy.GetType().Name}...");
            return _compressionStrategy.Decompress(compressedData);
        }
    }

    // ৫. Client Code (ইন্ডাস্ট্রিতে যেভাবে ইউজ হয়)
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Industry Standard Decorator + Strategy Pattern ===\n");

            string salaryRecords = "User: Abdul Barik, Salary: $5000";

            // Decorator প্যাটার্ন দিয়ে পাইপলাইন তৈরি করছি, আর Strategy প্যাটার্ন দিয়ে অ্যালগরিদম ইনজেক্ট করছি!
            // এখন কোড ১০০% লুজলি কাপলড (Loosely Coupled) এবং Open/Closed Principle মেনে চলছে।
            
            IDataSource secureStorage = new CompressionDecorator(
                                            new EncryptionDecorator(
                                                new FileDataSource("salary_records.dat"),
                                                new AesEncryptionStrategy() // Injecting Encryption Strategy
                                            ),
                                            new ZipCompressionStrategy() // Injecting Compression Strategy
                                        );

            Console.WriteLine("--- WRITING PROCESS ---");
            secureStorage.WriteData(salaryRecords);

            Console.WriteLine("\n--- READING PROCESS ---");
            string loadedData = secureStorage.ReadData();
            
            Console.WriteLine($"\n[Final Output shown to Client]: {loadedData}");
        }
    }
}
