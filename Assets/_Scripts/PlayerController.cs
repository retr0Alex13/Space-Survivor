using UnityEngine;

namespace Voidwalker
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 50f;

        private Rigidbody2D rigidBody;
        private PlayerInput playerInput;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            rigidBody.AddForce(playerInput.GetMovementInput() * movementSpeed, ForceMode2D.Force);
        }
    }
}
