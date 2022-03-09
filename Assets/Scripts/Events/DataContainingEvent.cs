using UnityEngine.InputSystem;

namespace Events
{
    public class DataContainingEvent<T> : GameEvent where T : struct
    {
        public T Data;

        public DataContainingEvent(InputAction.CallbackContext ctx)
        {
            Data = ctx.ReadValue<T>();
        }
    }
}