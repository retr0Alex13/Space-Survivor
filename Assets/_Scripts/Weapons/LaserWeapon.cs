using UnityEngine;

namespace Voidwalker.Weapons
{
    public class LaserWeapon : WeaponBase
    {
        [SerializeField] private float spreadAngle = 3f;
        [SerializeField] private int bulletsPerShot = 1;

        protected override void SpawnProjectile(Transform firePoint)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                // Додаємо випадковий розкид для пострілів
                Vector3 spread = new Vector3(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0
                );

                Quaternion rotation = Quaternion.Euler(spread) * firePoint.rotation;

                GameObject bullet = Instantiate(projectilePrefab, firePoint.position, rotation);
                Projectile projectile = bullet.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.Initialize(Damage, Range, rotation * Vector3.forward);
                }
            }
        }
    }
}
