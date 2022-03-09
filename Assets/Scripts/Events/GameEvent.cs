using System;

namespace Events
{
    public class GameEvent
    {
        private bool _cancelledInternal = false;
        public bool Cancelled
        {
            get => _cancelledInternal;
            set
            {
                if (!Cancellable()) throw new Exception("Tried to cancel un-cancellable event");
                _cancelledInternal = value;
            }
        }

        protected virtual bool Cancellable()
        {
            return true;
        }
    }

    public class NonCancellableGameEvent : GameEvent
    {
        protected override bool Cancellable()
        {
            return false;
        }
    }
}