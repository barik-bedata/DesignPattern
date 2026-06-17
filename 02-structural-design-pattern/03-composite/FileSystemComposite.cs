using System;
using System.Collections.Generic;

namespace CompositePatternExample.FileSystem
{
    // ১. Component (সবার জন্য কমন)
    public interface IFileSystemNode
    {
        string GetName();
        int GetSize(); // কিলোবাইটে সাইজ রিটার্ন করবে
        void PrintStructure(string indent = "");
    }

    // ২. Composite Interface (শুধু ফোল্ডারের জন্য, কারণ ফোল্ডারেই কেবল নতুন কিছু অ্যাড করা যায়)
    public interface IFolder : IFileSystemNode
    {
        void Add(IFileSystemNode node);
    }

    // ৩. Leaf (ফাইল - এর ভেতরে আর কিছু ঢোকানো যায় না)
    public class FileNode : IFileSystemNode
    {
        private string _name;
        private int _size;

        public FileNode(string name, int size)
        {
            _name = name;
            _size = size;
        }

        public string GetName() => _name;
        public int GetSize() => _size;

        public void PrintStructure(string indent = "")
        {
            Console.WriteLine($"{indent}- File: {_name} ({_size} KB)");
        }
    }

    // ৪. Composite (ফোল্ডার - এর ভেতরে ফাইল বা আরও ফোল্ডার থাকতে পারে)
    public class FolderNode : IFolder
    {
        private string _name;
        private List<IFileSystemNode> _children = new List<IFileSystemNode>();

        public FolderNode(string name)
        {
            _name = name;
        }

        public void Add(IFileSystemNode node)
        {
            _children.Add(node);
        }

        public string GetName() => _name;

        // নিজের ভেতরের সব ফাইল ও সাব-ফোল্ডারের সাইজ যোগ করে রিটার্ন করবে
        public int GetSize()
        {
            int totalSize = 0;
            foreach (var child in _children)
            {
                totalSize += child.GetSize();
            }
            return totalSize;
        }

        public void PrintStructure(string indent = "")
        {
            Console.WriteLine($"{indent}+ Folder: {_name} (Total Size: {GetSize()} KB)");
            foreach (var child in _children)
            {
                child.PrintStructure(indent + "  "); // ইনডেন্ট বাড়িয়ে সুন্দর করে প্রিন্ট করবে
            }
        }
    }

    // ৫. Client
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== File System Example (Composite Pattern) ===\n");

            // ফাইল তৈরি (Leaf)
            IFileSystemNode file1 = new FileNode("resume.pdf", 500);
            IFileSystemNode file2 = new FileNode("design_pattern.docx", 1200);
            IFileSystemNode file3 = new FileNode("photo.jpg", 2500);
            IFileSystemNode file4 = new FileNode("video.mp4", 150000);

            // ফোল্ডার তৈরি (Composite)
            IFolder docsFolder = new FolderNode("Documents");
            docsFolder.Add(file1);
            docsFolder.Add(file2);

            IFolder mediaFolder = new FolderNode("Media");
            mediaFolder.Add(file3);
            mediaFolder.Add(file4);

            // মেইন রুট ফোল্ডার (এর ভেতরে অন্য ফোল্ডারগুলো রাখবো)
            IFolder rootFolder = new FolderNode("Root_Drive");
            rootFolder.Add(docsFolder);
            rootFolder.Add(mediaFolder);

            // Client শুধু root কে কল করবে, বাকি সব সে নিজে ট্রাভার্স (Traverse) করবে!
            rootFolder.PrintStructure();
        }
    }
}
