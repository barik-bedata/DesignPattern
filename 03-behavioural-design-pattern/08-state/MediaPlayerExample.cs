using System;

namespace BehavioralDesignPattern.State.MediaPlayer
{
    // ==========================================
    // 1. State Interface/Base Class
    // ==========================================
    // Defines common methods for all states, allowing Context to work with them without knowing concrete types.
    public interface IMediaState
    {
        void PressPlay(IMediaPlayer context);
        void PressPause(IMediaPlayer context);
        void PressStop(IMediaPlayer context);
    }

    // ==========================================
    // 2. Concrete States
    // ==========================================
    // Implement the State interface, encapsulating behavior for specific states and defining Context’s actions in those states.
    public class StoppedState : IMediaState
    {
        public void PressPlay(IMediaPlayer context)
        {
            Console.WriteLine("[Stopped State] Starting to play from the beginning...");
            context.ChangeState(new PlayingState());
        }

        public void PressPause(IMediaPlayer context) => Console.WriteLine("[Stopped State] Cannot pause. Media is already stopped.");
        public void PressStop(IMediaPlayer context) => Console.WriteLine("[Stopped State] Media is already stopped.");
    }

    public class PlayingState : IMediaState
    {
        public void PressPlay(IMediaPlayer context) => Console.WriteLine("[Playing State] Already playing. Doing nothing.");

        public void PressPause(IMediaPlayer context)
        {
            Console.WriteLine("[Playing State] Pausing the media...");
            context.ChangeState(new PausedState());
        }

        public void PressStop(IMediaPlayer context)
        {
            Console.WriteLine("[Playing State] Stopping the media...");
            context.ChangeState(new StoppedState());
        }
    }

    public class PausedState : IMediaState
    {
        public void PressPlay(IMediaPlayer context)
        {
            Console.WriteLine("[Paused State] Resuming the media...");
            context.ChangeState(new PlayingState());
        }

        public void PressPause(IMediaPlayer context) => Console.WriteLine("[Paused State] Already paused.");

        public void PressStop(IMediaPlayer context)
        {
            Console.WriteLine("[Paused State] Stopping the media...");
            context.ChangeState(new StoppedState());
        }
    }

    // ==========================================
    // 3. Context
    // ==========================================
    // Maintains a reference to the current state, delegates behavior to it, and provides an interface for clients.
    
    // DIP মানার জন্য IMediaPlayer ইন্টারফেস (Context Interface) তৈরি করা হলো
    public interface IMediaPlayer
    {
        void ChangeState(IMediaState newState);
    }

    public class MediaPlayer : IMediaPlayer
    {
        private IMediaState _currentState;

        public MediaPlayer()
        {
            _currentState = new StoppedState();
        }

        public void ChangeState(IMediaState newState)
        {
            _currentState = newState;
        }

        // Context নিজে কোনো কাজ করে না। সে তার বর্তমান State (অবজেক্ট)-এর কাছে কাজটি পাঠিয়ে দেয় (Delegate করে)।
        public void PressPlay() 
        {
            Console.WriteLine($"--> [Context] Delegating PressPlay() to {_currentState.GetType().Name}");
            _currentState.PressPlay(this);
        }

        public void PressPause() 
        {
            Console.WriteLine($"--> [Context] Delegating PressPause() to {_currentState.GetType().Name}");
            _currentState.PressPause(this);
        }

        public void PressStop() 
        {
            Console.WriteLine($"--> [Context] Delegating PressStop() to {_currentState.GetType().Name}");
            _currentState.PressStop(this);
        }
    }

    // ==========================================
    // Client Usage
    // ==========================================
    class Program
    {
        static void Run()
        {
            Console.WriteLine("=== State Pattern (Media Player) ===\n");
            
            MediaPlayer player = new MediaPlayer();

            player.PressPlay();   // Stopped -> Playing
            player.PressPlay();   // Playing -> Playing (Ignored)
            player.PressPause();  // Playing -> Paused
            player.PressStop();   // Paused -> Stopped
            player.PressPause();  // Stopped -> Error message
        }
    }
}
