using UnityEngine.InputSystem;
using UnityEngine;

namespace Voidwalker.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private bool isAttacking;
        private Vector2 movementInput;
        private Vector3 mousePosition;

        private PlayerInputActions playerInputs;

        private void Awake()
        {
            playerInputs = new PlayerInputActions();
        }

        private void OnEnable() => playerInputs.Player.Enable();

        private void OnDisable() => playerInputs.Player.Disable();

        private void Update()
        {
            ReadMoveInput();
            ReadMousePosition();
            ReadFireInput();
        }

        private void ReadFireInput()
        {
            isAttacking = playerInputs.Player.Attack.triggered;
        }

        private void ReadMoveInput()
        {
            movementInput = playerInputs.Player.Move.ReadValue<Vector2>();
        }

        private void ReadMousePosition()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public bool IsAttacking() => isAttacking;

        public Vector2 GetMovementInput() => movementInput;

        public Vector3 GetMousePosition()
        {
            return mousePosition;
        }
    }
}
