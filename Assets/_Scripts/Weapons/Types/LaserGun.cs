using UnityEngine;

namespace Voidwalker.Weapons
{
    public class LaserGun : BaseWeapon
    {
        [Space(10)]

        [Header("Laser Gun properties")]

        [SerializeField] private float laserSpeed = 15f;
        [SerializeField] private GameObject laserPrefab;

        protected override void ExecuteShot()
        {
            GameObject shot = Instantiate(laserPrefab, transform.position, transform.rotation);

            Projectile projectile = shot.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.Initialize(damage, range, laserSpeed, transform.right);
            }
        }
    }
}
