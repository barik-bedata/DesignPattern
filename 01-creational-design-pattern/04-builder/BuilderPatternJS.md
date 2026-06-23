# 🏗️ Builder Design Pattern: The Ultimate Guide (JavaScript/TypeScript Edition)

আপনি একদম ঠিক বলেছেন, Builder Pattern এর সবচেয়ে বড় সুবিধা হলো কনস্ট্রাক্টরে (Constructor) অনেকগুলো প্যারামিটার থাকার কারণে যে **Parameter Mismatch** বা **Telescoping Constructor Anti-pattern** তৈরি হয়, তা দূর করা। 

তবে প্রোডাকশন-গ্রেড অ্যাপ্লিকেশনে এর আরও গভীর ব্যবহার রয়েছে। চলুন JavaScript/TypeScript-এর রিয়েল-ওয়ার্ল্ড উদাহরণের মাধ্যমে আপনার কনফিউশন একদম ক্লিয়ার করে ফেলি!

---

## 🧩 Builder Pattern-এর মূল কম্পোনেন্টগুলো কী কী?

GoF (Gang of Four) অনুযায়ী, একটি স্ট্যান্ডার্ড Builder Pattern-এ ৪টি অংশ থাকে:

1. **Product (প্রোডাক্ট):** আপনি শেষ পর্যন্ত যে অবজেক্টটি তৈরি করতে চাচ্ছেন। (যেমন: একটি Email)।
2. **Builder (ইন্টারফেস/অ্যাবস্ট্রাক্ট ক্লাস):** প্রোডাক্ট তৈরি করার জন্য কী কী ধাপ বা মেথড লাগবে, তার ব্লুপ্রিন্ট।
3. **Concrete Builder (বাস্তব বিল্ডার):** যে আসলে ইন্টারফেস মেনে প্রোডাক্টটি তিল তিল করে গড়ে তোলে। 
4. **Director (ডিরেক্টর):** এর কাজ হলো বিল্ডারকে বলে দেওয়া কোন ধাপে কোন মেথড কল করতে হবে।

---

## 🌍 Real-World Example: প্রোডাকশন অ্যাপ্লিকেশনে এটি কীভাবে কাজ করে?

ধরে নিই, আমরা একটি প্রোডাকশন-গ্রেড ই-কমার্স সিস্টেম বানাচ্ছি। সেখানে আমাদের ইউজারদের বিভিন্ন রকম **Email** পাঠাতে হয়। 

যদি আমরা সাধারণ ক্লাস বানাই, তাহলে অবস্থা হবে এমন:
```typescript
// ❌ Anti-pattern: Telescoping Constructor
const email = new EmailMessage("hi@test.com", "Hello", "Body", null, null, ["file.pdf"], true, false);
// এখানে null, null, true, false কীসের ভ্যালু, তা বোঝা অসম্ভব! প্যারামিটার মিসম্যাচ হওয়ার চান্স ১০০%।
```

### চলুন এটিকে Builder Pattern দিয়ে সাজাই:

#### Step 1: The Product (যেটি আমরা বানাতে চাই)
```typescript
// File: EmailMessage.ts
export class EmailMessage {
  public to: string = "";
  public from: string = "noreply@company.com";
  public subject: string = "";
  public textBody: string = "";
  public htmlBody?: string;
  public attachments?: string[];

  // এই ইমেইল সেন্ড করার ডেমো মেথড
  public send(): void {
    console.log(`Sending email to: ${this.to}`);
    console.log(`Subject: ${this.subject}`);
    if (this.attachments) console.log(`Attachments: ${this.attachments.length} files`);
  }
}
```

#### Step 2: The Builder Interface & Concrete Builder
JavaScript/TypeScript-এ আমরা মেথড চেইনিং (Method Chaining) ব্যবহার করার জন্য প্রতিটি মেথড থেকে `this` (Builder এর ইন্সট্যান্স) রিটার্ন করি।

```typescript
// File: EmailBuilder.ts
import { EmailMessage } from "./EmailMessage";

// Concrete Builder
export class EmailBuilder {
  private email: EmailMessage;

  constructor() {
    this.email = new EmailMessage(); // শুরুতে একটি ফাঁকা অবজেক্ট নিবে
  }

  public setTo(address: string): this {
    this.email.to = address;
    return this; // this রিটার্ন করার ফলেই আমরা chaining করতে পারি
  }

  public setFrom(address: string): this {
    this.email.from = address;
    return this;
  }

  public setSubject(subject: string): this {
    this.email.subject = subject;
    return this;
  }

  public setBody(text: string, html?: string): this {
    this.email.textBody = text;
    if (html) this.email.htmlBody = html;
    return this;
  }

  public addAttachment(fileUrl: string): this {
    if (!this.email.attachments) this.email.attachments = [];
    this.email.attachments.push(fileUrl);
    return this;
  }

  // 🚨 সবচেয়ে গুরুত্বপূর্ণ মেথড: Build()
  // এই মেথড কল না করা পর্যন্ত প্রোডাক্ট ফাইনাল হবে মহাশয়!
  public build(): EmailMessage {
    // এখানে আমরা ভ্যালিডেশন চেক করতে পারি!
    if (!this.email.to) throw new Error("Recipient 'to' address is mandatory!");
    if (!this.email.subject) throw new Error("Email must have a subject!");

    // প্রোডাক্ট রিটার্ন করার পর বিল্ডারকে রিসেট করে দেওয়া ভালো প্র্যাকটিস
    const finalProduct = this.email;
    this.email = new EmailMessage(); 
    
    return finalProduct;
  }
}
```

---

## 🎬 Step 3: The Director Component (ডিরেক্টর)

প্রোডাকশনে অনেক সময় একই ধরণের অবজেক্ট বারবার বানাতে হয়। যেমন: *Welcome Email*, *Password Reset Email* ইত্যাদি। 

Client-কে যেন বারবার একই মেথড চেইন লিখতে না হয়, সেজন্য আমরা একজন **Director** বা ম্যানেজার নিয়োগ করতে পারি। ডিরেক্টর জানে *কীভাবে* বানাতে হয়, সে শুধু বিল্ডারকে হুকুম দেয়!

```typescript
// File: EmailDirector.ts
import { EmailBuilder } from "./EmailBuilder";

// The Director
export class EmailDirector {
  
  // ডিরেক্টরের কাছে শুধু বিল্ডার দেওয়া থাকবে
  public constructWelcomeEmail(builder: EmailBuilder, userEmail: string): void {
    builder
      .setTo(userEmail)
      .setSubject("Welcome to our awesome platform!")
      .setBody("Hi there! We are so glad to have you.");
  }

  public constructPasswordResetEmail(builder: EmailBuilder, userEmail: string, resetLink: string): void {
    builder
      .setTo(userEmail)
      .setSubject("Password Reset Request")
      .setBody(`Click here to reset: ${resetLink}`, `<a href="${resetLink}">Reset Password</a>`);
  }
}
```

---

## 🚀 Step 4: Client Code (যেভাবে আমরা ব্যবহার করব)

এখন দেখুন আমাদের প্রোডাকশন কোড কতটা সুন্দর এবং রিডেবল (Readable) হয়ে গেছে!

```typescript
// File: index.ts
import { EmailBuilder } from "./EmailBuilder";
import { EmailDirector } from "./EmailDirector";

// --- Example 1: Director ছাড়া (Custom Email) ---
const customBuilder = new EmailBuilder();
const customEmail = customBuilder
  .setTo("client@business.com")
  .setSubject("Your Monthly Invoice")
  .setBody("Please find your invoice attached.")
  .addAttachment("s3://bucket/invoice_jan.pdf")
  .build(); 
  
customEmail.send();


// --- Example 2: Director ব্যবহার করে (Template Email) ---
const builder = new EmailBuilder();
const director = new EmailDirector();

// ডিরেক্টরকে বললাম "আমাকে একটা ওয়েলকাম ইমেইল বানিয়ে দাও"
director.constructWelcomeEmail(builder, "newuser@example.com");

// বিল্ডারের কাছ থেকে ফাইনাল প্রোডাক্ট নিয়ে নিলাম
const welcomeEmail = builder.build();

welcomeEmail.send();
```

---

## 🎯 প্রোডাকশনে Builder Pattern কেন এত জনপ্রিয়? (Why we love it)

1. **Step-by-Step Creation:** একটি অবজেক্টকে একবারে তৈরি না করে, ধাপে ধাপে তৈরি করা যায়।
2. **Immutability (অপরিবর্তনযোগ্যতা):** `build()` মেথডের মাধ্যমে আমরা চাইলে একটি Immutable (যাকে আর চেঞ্জ করা যায় না) অবজেক্ট রিটার্ন করতে পারি।
3. **Validation at the end:** অবজেক্টের সব ডেটা দেওয়া শেষ হলে `build()` মেথডের ভেতরে একবারে কড়া ভ্যালিডেশন করা যায়।
4. **Different Representations:** একই ডিরেক্টর ব্যবহার করে আপনি চাইলে `EmailBuilder`-এর বদলে `SmsBuilder` পাস করতে পারেন, যদি তারা একই ইন্টারফেস ইমপ্লিমেন্ট করে থাকে।

### 💡 JavaScript / Node.js প্রোডাকশনে রিয়েল লাইফ ব্যবহার:
- **Knex.js / Prisma / TypeORM:** এরা সবাই ডাটাবেস কুয়েরি বানানোর জন্য Builder pattern ব্যবহার করে (`db.select('*').from('users').where('id', 1)`).
- **DOM Builders:** ফ্রন্টএন্ডে অনেক সময় HTML DOM ট্রি বানানোর জন্য এটি ব্যবহৃত হয়।
- **Testing:** Unit Testing-এর সময় `MockDataBuilder` বা `UserFactoryBuilder` প্রোডাকশনে প্রচণ্ড ব্যবহৃত হয় ফেক ডেটা বানানোর জন্য।

---

## 🛠️ Bonus: Query Builder (Prisma / Knex.js) এর রিয়েল এক্সাম্পল

আপনি যেহেতু Prisma বা ডাটাবেসের কথা জিজ্ঞেস করেছেন, তাই এখানে দেখানো হলো ঠিক কীভাবে ডাটাবেস লাইব্রেরিগুলো Builder Pattern ব্যবহার করে SQL কুয়েরি জেনারেট করে:

```typescript
// File: SqlQueryBuilder.ts
// Query Builder (The Builder)
export class SqlQueryBuilder {
  private query: string = "";

  public select(fields: string): this {
    this.query += `SELECT ${fields} `;
    return this; // Method Chaining
  }

  public from(table: string): this {
    this.query += `FROM ${table} `;
    return this;
  }

  public where(condition: string): this {
    this.query += `WHERE ${condition} `;
    return this;
  }

  public limit(count: number): this {
    this.query += `LIMIT ${count} `;
    return this;
  }

  // 🚨 Build Method
  public build(): string {
    return this.query.trim() + ";";
  }
}
```

```typescript
// File: SqlQueryDirector.ts
import { SqlQueryBuilder } from "./SqlQueryBuilder";

// The Director (for SqlQueryBuilder)
export class SqlQueryDirector {
  public constructActiveUsersQuery(builder: SqlQueryBuilder): void {
    builder
      .select("id, name, email")
      .from("users")
      .where("status = 'ACTIVE'")
      .limit(10);
  }
}
```

```typescript
// File: index.ts
import { SqlQueryBuilder } from "./SqlQueryBuilder";
import { SqlQueryDirector } from "./SqlQueryDirector";

// --- Client Code (কীভাবে আমরা প্রোডাকশনে ব্যবহার করি) ---
const queryBuilder = new SqlQueryBuilder();
const queryDirector = new SqlQueryDirector();

// ডিরেক্টরকে বললাম active users কুয়েরি বানাতে
queryDirector.constructActiveUsersQuery(queryBuilder);

const finalSQL = queryBuilder.build();

console.log(finalSQL); 
// আউটপুট: SELECT id, name, email FROM users WHERE status = 'ACTIVE' LIMIT 10;
```
এই কারণেই ডাটাবেস ORM (যেমন Prisma, TypeORM) ব্যবহার করা এত সহজ! ভেতরে ভেতরে তারা সবাই এই **Builder Pattern** মেনেই কাজ করে।
