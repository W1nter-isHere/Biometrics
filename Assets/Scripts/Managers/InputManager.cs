using Channels;
using Input;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputChannel inputChannel;
        private InputMaster _inputMaster;

        public void Awake()
        {
            _inputMaster = new InputMaster();
        }

        public void OnEnable()
        {
            _inputMaster.Enable();

            _inputMaster.UI.EscapeHeld.performed += inputChannel.FireOnEscapeHeld;
            _inputMaster.UI.EscapePressed.performed += inputChannel.FireOnEscapePressed;
            _inputMaster.Player.Movement.performed += inputChannel.FireOnMovePerformed;
            _inputMaster.Player.Movement.canceled += inputChannel.FireOnMoveCancelled;
            _inputMaster.Player.Fall.performed += inputChannel.FireOnFallPerformed;
            _inputMaster.Player.Fall.canceled += inputChannel.FireOnFallCancelled;
            _inputMaster.Player.Dash.performed += inputChannel.FireOnDashPerformed;
            _inputMaster.Player.Jump.performed += inputChannel.FireOnJumpPressed;
            _inputMaster.Player.MousePosition.performed += inputChannel.FireOnMouseMoved;
            _inputMaster.Player.Attack.performed += inputChannel.FireOnAttackPressed;
            _inputMaster.Player.SecondaryAttack.performed += inputChannel.FireOnSecondaryAttackPressed;
        }

        public void OnDisable()
        {
            _inputMaster.Disable();
            
            _inputMaster.UI.EscapeHeld.performed -= inputChannel.FireOnEscapeHeld;
            _inputMaster.UI.EscapePressed.performed -= inputChannel.FireOnEscapePressed;
            _inputMaster.Player.Movement.performed -= inputChannel.FireOnMovePerformed;
            _inputMaster.Player.Movement.canceled -=inputChannel.FireOnMoveCancelled;
            _inputMaster.Player.Fall.performed -= inputChannel.FireOnFallPerformed;
            _inputMaster.Player.Fall.canceled -= inputChannel.FireOnFallCancelled;
            _inputMaster.Player.Dash.performed -= inputChannel.FireOnDashPerformed;
            _inputMaster.Player.Jump.performed -= inputChannel.FireOnJumpPressed;
            _inputMaster.Player.MousePosition.performed -= inputChannel.FireOnMouseMoved;
            _inputMaster.Player.Attack.performed -= inputChannel.FireOnAttackPressed;
            _inputMaster.Player.SecondaryAttack.performed -= inputChannel.FireOnSecondaryAttackPressed;
        }
    }
}