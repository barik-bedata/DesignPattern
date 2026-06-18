using System;

namespace BehavioralDesignPattern.State.DocumentWorkflow
{
    // ==========================================
    // 1. State Interface/Base Class
    // ==========================================
    // Defines common methods for all states, allowing Context to work with them without knowing concrete types.
    public interface IDocumentState
    {
        void Publish(IDocument context);
        void Reject(IDocument context);
    }

    // ==========================================
    // 2. Concrete States
    // ==========================================
    // Implement the State interface, encapsulating behavior for specific states and defining Context’s actions in those states.
    public class DraftState : IDocumentState
    {
        public void Publish(IDocument context)
        {
            Console.WriteLine("[Draft] Document sent for moderation review.");
            context.ChangeState(new ModerationState());
        }

        public void Reject(IDocument context)
        {
            Console.WriteLine("[Draft] Cannot reject a draft. It is still being written.");
        }
    }

    public class ModerationState : IDocumentState
    {
        public void Publish(IDocument context)
        {
            Console.WriteLine("[Moderation] Review successful. Document is now Published publicly.");
            context.ChangeState(new PublishedState());
        }

        public void Reject(IDocument context)
        {
            Console.WriteLine("[Moderation] Review failed. Sending back to Draft.");
            context.ChangeState(new DraftState());
        }
    }

    public class PublishedState : IDocumentState
    {
        public void Publish(IDocument context)
        {
            Console.WriteLine("[Published] Document is already published.");
        }

        public void Reject(IDocument context)
        {
            Console.WriteLine("[Published] Document has been un-published and sent back to Draft.");
            context.ChangeState(new DraftState());
        }
    }

    // ==========================================
    // 3. Context
    // ==========================================
    // Maintains a reference to the current state, delegates behavior to it, and provides an interface for clients.
    
    // DIP মানার জন্য IDocument ইন্টারফেস (Context Interface) তৈরি করা হলো
    public interface IDocument
    {
        void ChangeState(IDocumentState newState);
    }

    public class Document : IDocument
    {
        private IDocumentState _currentState;
        public string Title { get; }

        public Document(string title)
        {
            Title = title;
            _currentState = new DraftState();
        }

        public void ChangeState(IDocumentState newState)
        {
            _currentState = newState;
        }

        public void Publish()
        {
            Console.WriteLine($"\n--> [Context] Delegating Publish() to {_currentState.GetType().Name}");
            Console.Write($"Action [Publish]: ");
            _currentState.Publish(this);
        }

        public void Reject()
        {
            Console.WriteLine($"\n--> [Context] Delegating Reject() to {_currentState.GetType().Name}");
            Console.Write($"Action [Reject]: ");
            _currentState.Reject(this);
        }
    }

    // ==========================================
    // Client Usage
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== State Pattern (Document Workflow/CMS) ===\n");
            
            Document blogPost = new Document("Learn SOLID Principles");

            blogPost.Publish(); // Draft -> Moderation
            blogPost.Reject();  // Moderation -> Draft (Review failed)
            
            Console.WriteLine("--- Writer updates document ---");
            blogPost.Publish(); // Draft -> Moderation
            blogPost.Publish(); // Moderation -> Published (Review passed)
            
            blogPost.Publish(); // Published -> Ignored
        }
    }
}
