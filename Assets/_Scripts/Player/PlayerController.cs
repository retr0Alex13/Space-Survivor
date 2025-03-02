using UnityEngine;

namespace Voidwalker.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float movementSpeed = 50f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float angleOffset = 0f;

        private Rigidbody2D rigidBody;
        private PlayerInput playerInput;

        private Quaternion targetRotation;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            rigidBody.AddForce(playerInput.GetMovementInput() * movementSpeed, ForceMode2D.Force);
        }

        private void Update()
        {
            RotatePlayer();
            if (playerInput.IsAttacking())
            {

            }
        }

        private void RotatePlayer()
        {
            CalculateTargetRotation();

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void CalculateTargetRotation()
        {
            Vector3 mousePosition = playerInput.GetMousePosition();

            mousePosition.z = transform.position.z;

            Vector3 direction = mousePosition - transform.position;

            // Skip rotation if there's no meaningful direction (prevents jitter)
            if (direction.magnitude < 0.1f)
                return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += angleOffset;

            targetRotation = Quaternion.Euler(0, 0, angle);
        }

        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
    }
}
