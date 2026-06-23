# 🏗️ Builder Design Pattern: The Ultimate Guide (C# Edition)

আপনি একদম ঠিক বলেছেন, Builder Pattern এর সবচেয়ে বড় সুবিধা হলো কনস্ট্রাক্টরে (Constructor) অনেকগুলো প্যারামিটার থাকার কারণে যে **Parameter Mismatch** বা **Telescoping Constructor Anti-pattern** তৈরি হয়, তা দূর করা। 

তবে প্রোডাকশন-গ্রেড অ্যাপ্লিকেশনে এর আরও গভীর ব্যবহার রয়েছে। চলুন C#-এর রিয়ে-ওয়ার্ল্ড উদাহরণের মাধ্যমে আপনার কনফিউশন একদম ক্লিয়ার করে ফেলি!

---

## 🧩 Builder Pattern-এর মূল কম্পোনেন্টগুলো কী কী?

GoF (Gang of Four) অনুযায়ী, একটি স্ট্যান্ডার্ড Builder Pattern-এ ৪টি অংশ থাকে:

1. **Product (প্রোডাক্ট):** আপনি শেষ পর্যন্ত যে অবজেক্টটি তৈরি করতে চাচ্ছেন। (যেমন: একটি Email)।
2. **Builder (ইন্টারফেস/অ্যাবস্ট্রাক্ট ক্লাস):** প্রোডাক্ট তৈরি করার জন্য কী কী ধাপ বা মেথড লাগবে, তার ব্লুপ্রিন্ট।
3. **Concrete Builder (বাস্তব বিল্ডার):** যে আসলে ইন্টারফেস মেনে প্রোডাক্টটি তিল তিল করে গড়ে তোলে। 
4. **Director (ডিরেক্টর):** এর কাজ হলো বিল্ডারকে বলে দেওয়া কোন ধাপে কোন মেথড কল করতে হবে।

---

## 🌍 Real-World Example: প্রোডাকশন C# অ্যাপ্লিকেশনে এটি কীভাবে কাজ করে?

ধরে নিই, আমরা একটি প্রোডাকশন-গ্রেড ই-কমার্স সিস্টেম বানাচ্ছি। সেখানে আমাদের ইউজারদের বিভিন্ন রকম **Email** পাঠাতে হয়। 

যদি আমরা সাধারণ ক্লাস বানাই, তাহলে অবস্থা হবে এমন:
```csharp
// ❌ Anti-pattern: Telescoping Constructor
var email = new EmailMessage("hi@test.com", "Hello", "Body", null, null, new List<string>{"file.pdf"}, true, false);
// এখানে null, null, true, false কীসের ভ্যালু, তা বোঝা অসম্ভব! প্যারামিটার মিসম্যাচ হওয়ার চান্স ১০০%।
```

### চলুন এটিকে Builder Pattern দিয়ে সাজাই:

#### Step 1: The Product (যেটি আমরা বানাতে চাই)
```csharp
using System.Collections.Generic;
using System;

// Product: যে ইমেইল অবজেক্টটি তৈরি হবে
public class EmailMessage 
{
    public string To { get; set; } = string.Empty;
    public string From { get; set; } = "noreply@company.com";
    public string Subject { get; set; } = string.Empty;
    public string TextBody { get; set; } = string.Empty;
    public string HtmlBody { get; set; }
    public List<string> Attachments { get; set; } = new List<string>();

    // এই ইমেইল সেন্ড করার ডেমো মেথড
    public void Send() 
    {
        Console.WriteLine($"Sending email to: {To}");
        Console.WriteLine($"Subject: {Subject}");
        if (Attachments.Count > 0) 
        {
            Console.WriteLine($"Attachments: {Attachments.Count} files");
        }
    }
}
```

#### Step 2: The Builder Interface & Concrete Builder
C#-এ আমরা মেথড চেইনিং (Method Chaining) ব্যবহার করার জন্য প্রতিটি মেথড থেকে `this` (Builder এর ইন্সট্যান্স) রিটার্ন করি।

```csharp
// Builder Interface
public interface IEmailBuilder 
{
    IEmailBuilder SetTo(string address);
    IEmailBuilder SetFrom(string address);
    IEmailBuilder SetSubject(string subject);
    IEmailBuilder SetBody(string text, string html = null);
    IEmailBuilder AddAttachment(string fileUrl);
    EmailMessage Build();
}

// Concrete Builder
public class EmailBuilder : IEmailBuilder 
{
    private EmailMessage _email;

    public EmailBuilder() 
    {
        _email = new EmailMessage(); // শুরুতে একটি ফাঁকা অবজেক্ট নিবে
    }

    public IEmailBuilder SetTo(string address) 
    {
        _email.To = address;
        return this; // this রিটার্ন করার ফলেই আমরা chaining করতে পারি
    }

    public IEmailBuilder SetFrom(string address) 
    {
        _email.From = address;
        return this;
    }

    public IEmailBuilder SetSubject(string subject) 
    {
        _email.Subject = subject;
        return this;
    }

    public IEmailBuilder SetBody(string text, string html = null) 
    {
        _email.TextBody = text;
        if (html != null) _email.HtmlBody = html;
        return this;
    }

    public IEmailBuilder AddAttachment(string fileUrl) 
    {
        _email.Attachments.Add(fileUrl);
        return this;
    }

    // 🚨 সবচেয়ে গুরুত্বপূর্ণ মেথড: Build()
    // এই মেথড কল না করা পর্যন্ত প্রোডাক্ট ফাইনাল হবে না
    public EmailMessage Build() 
    {
        // এখানে আমরা ভ্যালিডেশন চেক করতে পারি!
        if (string.IsNullOrEmpty(_email.To)) 
            throw new InvalidOperationException("Recipient 'To' address is mandatory!");
            
        if (string.IsNullOrEmpty(_email.Subject)) 
            throw new InvalidOperationException("Email must have a subject!");

        // প্রোডাক্ট রিটার্ন করার পর বিল্ডারকে রিসেট করে দেওয়া ভালো প্র্যাকটিস
        var finalProduct = _email;
        _email = new EmailMessage(); 
        
        return finalProduct;
    }
}
```

---

## 🎬 Step 3: The Director Component (ডিরেক্টর)

প্রোডাকশনে অনেক সময় একই ধরণের অবজেক্ট বারবার বানাতে হয়। যেমন: *Welcome Email*, *Password Reset Email* ইত্যাদি। 

Client-কে যেন বারবার একই মেথড চেইন লিখতে না হয়, সেজন্য আমরা একজন **Director** বা ম্যানেজার নিয়োগ করতে পারি। ডিরেক্টর জানে *কীভাবে* বানাতে হয়, সে শুধু বিল্ডারকে হুকুম দেয়!

```csharp
// The Director
public class EmailDirector 
{
    // ডিরেক্টরের কাছে শুধু বিল্ডার দেওয়া থাকবে
    public void ConstructWelcomeEmail(IEmailBuilder builder, string userEmail) 
    {
        builder
            .SetTo(userEmail)
            .SetSubject("Welcome to our awesome C# platform!")
            .SetBody("Hi there! We are so glad to have you.");
    }

    public void ConstructPasswordResetEmail(IEmailBuilder builder, string userEmail, string resetLink) 
    {
        builder
            .SetTo(userEmail)
            .SetSubject("Password Reset Request")
            .SetBody($"Click here to reset: {resetLink}", $"<a href='{resetLink}'>Reset Password</a>");
    }
}
```

---

## 🚀 Step 4: Client Code (যেভাবে আমরা ব্যবহার করব)

এখন দেখুন আমাদের প্রোডাকশন কোড কতটা সুন্দর এবং রিডেবল (Readable) হয়ে গেছে!

```csharp
class Program
{
    static void Main(string[] args)
    {
        // --- Example 1: Director ছাড়া (Custom Email) ---
        var customBuilder = new EmailBuilder();
        var customEmail = customBuilder
            .SetTo("client@business.com")
            .SetSubject("Your Monthly Invoice")
            .SetBody("Please find your invoice attached.")
            .AddAttachment("s3://bucket/invoice_jan.pdf")
            .Build();
            
        customEmail.Send();


        // --- Example 2: Director ব্যবহার করে (Template Email) ---
        IEmailBuilder builder = new EmailBuilder();
        var director = new EmailDirector();

        // ডিরেক্টরকে বললাম "আমাকে একটা ওয়েলকাম ইমেইল বানিয়ে দাও"
        director.ConstructWelcomeEmail(builder, "newuser@example.com");

        // বিল্ডারের কাছ থেকে ফাইনাল প্রোডাক্ট নিয়ে নিলাম
        var welcomeEmail = builder.Build();
        
        welcomeEmail.Send();
    }
}
```

---

## 🎯 প্রোডাকশনে Builder Pattern কেন এত জনপ্রিয়? (Why we love it)

1. **Step-by-Step Creation:** একটি অবজেক্টকে একবারে তৈরি না করে, ধাপে ধাপে তৈরি করা যায়।
2. **Immutability (অপরিবর্তনযোগ্যতা):** `Build()` মেথডের মাধ্যমে আমরা চাইলে একটি Immutable (যাকে আর চেঞ্জ করা যায় না) অবজেক্ট রিটার্ন করতে পারি।
3. **Validation at the end:** অবজেক্টের সব ডেটা দেওয়া শেষ হলে `Build()` মেথডের ভেতরে একবারে কড়া ভ্যালিডেশন করা যায়।
4. **Different Representations:** একই ডিরেক্টর ব্যবহার করে আপনি চাইলে `EmailBuilder`-এর বদলে `SmsBuilder` পাস করতে পারেন, যদি তারা একই ইন্টারফেস ইমপ্লিমেন্ট করে থাকে।

### 💡 C# / .NET প্রোডাকশনে রিয়েল লাইফ ব্যবহার:
- **`HostBuilder` / `WebApplicationBuilder`:** ASP.NET Core-এ `builder.Services.AddControllers()` এভাবেই কাজ করে।
- **Entity Framework Core (EF Core):** ডাটাবেস মডেল বানানোর জন্য `ModelBuilder` প্রচণ্ড ব্যবহৃত হয়।
- **Unit Testing:** xUnit বা NUnit-এ ফেক ডেটা বানানোর জন্য `TestDataBuilder` প্রোডাকশনে খুব বেশি ব্যবহৃত হয়।
