using UnityEngine;

namespace Voidwalker.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private float damage;
        private float range;
        private Vector3 direction;
        private float speed = 20f;
        private float explosionRadius = 0f;
        private float explosionForce = 0f;
        private float lifeTime = 0f;

        public void Initialize(float dmg, float rng, Vector3 dir, float expRadius = 0f, float expForce = 0f)
        {
            damage = dmg;
            range = rng;
            direction = dir.normalized;
            explosionRadius = expRadius;
            explosionForce = expForce;
            lifeTime = range / speed;

            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (explosionRadius > 0)
            {
                // ¬ибухова збро€
                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                foreach (Collider hit in colliders)
                {
                    IDamageable target = hit.GetComponent<IDamageable>();
                    if (target != null)
                    {
                        // «меншуЇмо пошкодженн€ в залежност≥ в≥д в≥дстан≥ до еп≥центру
                        float distance = Vector3.Distance(transform.position, hit.transform.position);
                        float damageMultiplier = 1 - (distance / explosionRadius);
                        target.TakeDamage(damage * damageMultiplier);
                    }

                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                }
            }
            else
            {
                // «вичайна збро€
                IDamageable target = collision.gameObject.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }

            // ≈фект удару/вибуху можна додати тут
            Destroy(gameObject);
        }
    }
}
