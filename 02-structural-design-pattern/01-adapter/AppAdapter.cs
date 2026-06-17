using System;
using System.Collections.Generic;

namespace AdapterPattern.RealWorldExamples
{
    // ১. Target Interface (সিস্টেম এই ইন্টারফেস চেনে, যেমন Android এর RecyclerView)
    public interface IUIListElement
    {
        int GetTotalItems();
        void DrawItemOnScreen(int position);
    }

    // ২. Adaptee (আমাদের সাধারণ ডেটা, যা সরাসরি UI তে দেখানো যায় না)
    public class UserDataModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }

    // ৩. Adapter (সাধারণ List ডেটাকে IUIListElement এ রূপান্তর করবে)
    public class AppAdapter : IUIListElement
    {
        // Adaptee (যে ডেটাকে কনভার্ট করতে হবে)
        private List<UserDataModel> _dataList;

        public AppAdapter(List<UserDataModel> dataList)
        {
            _dataList = dataList;
        }

        // Target Interface এর মেথডগুলো ইমপ্লিমেন্ট করা হচ্ছে
        public int GetTotalItems()
        {
            return _dataList.Count;
        }

        public void DrawItemOnScreen(int position)
        {
            // ডেটাকে নিয়ে UI বা স্ক্রিনে আঁকার (Draw) ব্যবস্থা করা হচ্ছে
            var user = _dataList[position];
            Console.WriteLine($"[UI Component] Drawing User {position + 1}: {user.Name} ({user.Role})");
        }
    }

    // Client Code (যেমন Android এর RecyclerView বা আমাদের মোবাইল অ্যাপ)
    public class MobileAppList
    {
        public static void RenderListOnScreen(IUIListElement adapter)
        {
            Console.WriteLine("--- Rendering List on Screen ---");
            int count = adapter.GetTotalItems();
            for (int i = 0; i < count; i++)
            {
                adapter.DrawItemOnScreen(i);
            }
        }

        public static void Main()
        {
            // সাধারণ ডেটাবেস বা API থেকে আসা ডেটা (Adaptee)
            List<UserDataModel> myUsers = new List<UserDataModel>
            {
                new UserDataModel { Name = "Abdul Barik", Role = "Developer" },
                new UserDataModel { Name = "John Doe", Role = "Manager" },
                new UserDataModel { Name = "Alice", Role = "Designer" }
            };

            // সরাসরি List কে MobileAppList.RenderListOnScreen এ পাঠানো যাবে না!
            // তাই আমরা AppAdapter এর সাহায্য নিচ্ছি।
            IUIListElement myAdapter = new AppAdapter(myUsers);

            // এখন সিস্টেম সহজেই ডেটাগুলো স্ক্রিনে দেখাতে পারবে
            RenderListOnScreen(myAdapter);
        }
    }
}
