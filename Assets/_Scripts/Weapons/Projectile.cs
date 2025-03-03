using UnityEngine;

namespace Voidwalker
{
    public class Projectile : MonoBehaviour
    {
        private float damage;
        private float speed;
        private float range;

        private Vector2 direction;
        private Vector2 startPosition;

        private Rigidbody2D rigidBody;

        private void OnEnable()
        {
            startPosition = transform.position;
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float damage, float range, float speed, Vector2 direction)
        {
            this.damage = damage;
            this.range = range;
            this.speed = speed;
            this.direction = direction.normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void FixedUpdate()
        {
            MoveProjectile();
        }

        private void MoveProjectile()
        {
            rigidBody.linearVelocity = direction * speed;
        }

        private void Update()
        {
            CheckDistanceTraveled();
        }

        private void CheckDistanceTraveled()
        {
            float distanceTraveled = Vector2.Distance(startPosition, transform.position);
            if (distanceTraveled >= range)
            {
                if (gameObject.activeInHierarchy)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable hittable = collision.GetComponent<IDamageable>();

            if (hittable != null)
            {
                //hittable.TakeDamage(damage);
            }

            if (gameObject.activeInHierarchy)
            {
                //Destroy(gameObject);
            }
        }
    }
}
