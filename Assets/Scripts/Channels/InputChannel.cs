using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Channels
{
    [CreateAssetMenu(fileName = "Input Channel", menuName = "Game/Input Channel")]
    public class InputChannel : ScriptableObject
    {
        public event UnityAction<float> OnMovePerformed;
        public UnityEvent<InputAction.CallbackContext, DataContainingEvent<float>> preMovePerformed;
        
        public event UnityAction OnMoveCancelled;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preMoveCancelled;
        
        public event UnityAction OnFallPerformed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preFallPerformed;
        
        public event UnityAction OnFallCancelled;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preFallCancelled;
        
        public event UnityAction OnDashPressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preDashPerformed;
        
        public event UnityAction OnJumpPressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preJumpPressed;

        public event UnityAction OnAttackPressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preAttackPressed;
        
        public event UnityAction OnSecondaryAttackPressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preSecondaryAttackPressed;
        
        public event UnityAction<Vector2> OnMouseMoved;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preMouseMoved;

        public event UnityAction OnInteractionPressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preInteractionPressed;
        
        public event UnityAction OnInteractionReleased;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preInteractionReleased;
        
        public event UnityAction OnEscapePressed;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preEscapePressed;
        
        public event UnityAction OnEscapeHeld;
        public UnityEvent<InputAction.CallbackContext, GameEvent> preEscapeHeld;

        /// <summary>
        /// Mouse position on screen position
        /// </summary>
        public Vector2 MousePosition { get; private set; }
        /// <summary>
        /// If interaction key is just pressed
        /// </summary>
        public bool Interacted { get; private set; }

        public void FireOnMovePerformed(InputAction.CallbackContext context)
        {
            var @event = new DataContainingEvent<float>(context);
            preMovePerformed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnMovePerformed?.Invoke(@event.Data);
                return;   
            }
            Debug.Log("Canceled OnMovePerformed Event");
        }

        public void FireOnMoveCancelled(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preMoveCancelled?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnMoveCancelled?.Invoke();
                return;
            }
            Debug.Log("Canceled OnMoveCancelled Event");
        }
        
        public void FireOnDashPerformed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preDashPerformed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnDashPressed?.Invoke();
                return;   
            }
            Debug.Log("Canceled OnDashPerformed Event");
        }

        public void FireOnFallPerformed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preFallPerformed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnFallPerformed?.Invoke();
                return;
            }
            Debug.Log("Canceled OnFallPerformed Event");
        }
        
        public void FireOnFallCancelled(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preFallCancelled?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnFallCancelled?.Invoke();
                return;
            }
            Debug.Log("Canceled OnFallCancelled Event");
        }
        
        public void FireOnJumpPressed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preJumpPressed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnJumpPressed?.Invoke();
                return;
            }
            Debug.Log("Cancelled OnJumpPressed Event");
        }
        
        public void FireOnAttackPressed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preAttackPressed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnAttackPressed?.Invoke();
                return;
            }
            Debug.Log("Cancelled OnAttackPressed Event");
        }
        
        public void FireOnSecondaryAttackPressed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preSecondaryAttackPressed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnSecondaryAttackPressed?.Invoke();
                return;
            }
            Debug.Log("Cancelled OnSecondaryAttackPressed Event");
        }

        public void FireOnInteractionPressed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preInteractionPressed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnInteractionPressed?.Invoke();
                Interacted = !Interacted;
                return;
            }
            Debug.Log("Cancelled OnInteractionPressed Event");
        }

        public void FireOnInteractionReleased(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preInteractionReleased?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnInteractionReleased?.Invoke();
                Interacted = !Interacted;
                return;
            }
            Debug.Log("Cancelled OnInteractionReleased Event");
        }
        
        public void FireOnEscapePressed(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preEscapePressed?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnEscapePressed?.Invoke();
                return;
            }
            Debug.Log("Cancelled OnInteractionReleased Event");
        }
        
        public void FireOnEscapeHeld(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preEscapeHeld?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                OnEscapeHeld?.Invoke();
                return;
            }
            Debug.Log("Cancelled OnInteractionReleased Event");
        }

        public void FireOnMouseMoved(InputAction.CallbackContext context)
        {
            var @event = new GameEvent();
            preMouseMoved?.Invoke(context, @event);
            if (!@event.Cancelled)
            {
                var pos = context.ReadValue<Vector2>();
                OnMouseMoved?.Invoke(pos);
                MousePosition = pos;
                return;
            }
            Debug.Log("Cancelled OnMouseMoved Event");
        }
    }
}