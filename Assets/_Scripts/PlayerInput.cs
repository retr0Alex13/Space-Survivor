using UnityEngine.InputSystem;
using UnityEngine;

namespace Voidwalker
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 movementInput;

        private PlayerInputActions playerInputs;

        private void Awake()
        {
            playerInputs = new PlayerInputActions();
        }

        private void OnEnable() => playerInputs.Player.Enable();

        private void OnDisable() => playerInputs.Player.Disable();

        private void Update()
        {
            movementInput = playerInputs.Player.Move.ReadValue<Vector2>();
        }
    }
}
