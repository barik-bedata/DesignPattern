using System;
using System.Collections.Generic;

namespace BehavioralDesignPattern.Visitor.DocumentExporter
{
    // ==========================================
    // 1. Visitor Interface
    // ==========================================
    public interface IDocumentVisitor
    {
        void Visit(Paragraph paragraph);
        void Visit(Image image);
        void Visit(Table table);
    }

    // ==========================================
    // 3. Element Interface
    // ==========================================
    public interface IDocumentElement
    {
        void Accept(IDocumentVisitor visitor);
    }

    // ==========================================
    // 4. Concrete Elements
    // ==========================================
    public class Paragraph : IDocumentElement
    {
        public string Text { get; }

        public Paragraph(string text)
        {
            Text = text;
        }

        public void Accept(IDocumentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Image : IDocumentElement
    {
        public string SourcePath { get; }

        public Image(string sourcePath)
        {
            SourcePath = sourcePath;
        }

        public void Accept(IDocumentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Table : IDocumentElement
    {
        public int Rows { get; }
        public int Columns { get; }

        public Table(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public void Accept(IDocumentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // ==========================================
    // 2. Concrete Visitors
    // ==========================================
    // HTML-এ কনভার্ট করার ভিজিটর
    public class HtmlExportVisitor : IDocumentVisitor
    {
        public void Visit(Paragraph paragraph)
        {
            Console.WriteLine($"[HTML Exporter] <p>{paragraph.Text}</p>");
        }

        public void Visit(Image image)
        {
            Console.WriteLine($"[HTML Exporter] <img src='{image.SourcePath}' />");
        }

        public void Visit(Table table)
        {
            Console.WriteLine($"[HTML Exporter] <table> Rendered {table.Rows}x{table.Columns} cells </table>");
        }
    }

    // PDF-এ কনভার্ট করার ভিজিটর
    public class PdfExportVisitor : IDocumentVisitor
    {
        public void Visit(Paragraph paragraph)
        {
            Console.WriteLine($"[PDF Exporter] Drawing text block: '{paragraph.Text}' into PDF structure.");
        }

        public void Visit(Image image)
        {
            Console.WriteLine($"[PDF Exporter] Embedding binary image from {image.SourcePath} into PDF.");
        }

        public void Visit(Table table)
        {
            Console.WriteLine($"[PDF Exporter] Drawing PDF grid lines for {table.Rows}x{table.Columns} table.");
        }
    }

    // ==========================================
    // 5. Object Structure / Client
    // ==========================================
    public class Program
    {
        public static void Run()
        {
            Console.WriteLine("=== Visitor Pattern (Document Exporter) ===\n");

            // একটি ডকুমেন্টের ভেতরে থাকা এলিমেন্টগুলো
            List<IDocumentElement> document = new List<IDocumentElement>
            {
                new Paragraph("Visitor Design Pattern is awesome."),
                new Image("/assets/visitor.png"),
                new Table(3, 4)
            };

            Console.WriteLine("--- Exporting to HTML ---");
            HtmlExportVisitor htmlVisitor = new HtmlExportVisitor();
            foreach (var element in document)
            {
                element.Accept(htmlVisitor);
            }

            Console.WriteLine("\n--- Exporting to PDF ---");
            PdfExportVisitor pdfVisitor = new PdfExportVisitor();
            foreach (var element in document)
            {
                element.Accept(pdfVisitor);
            }
        }
    }
}
