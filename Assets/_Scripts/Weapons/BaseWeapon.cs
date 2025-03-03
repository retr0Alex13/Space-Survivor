using System.Collections;
using UnityEngine;

namespace Voidwalker.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [Header("Base Weapon Properties")]

        [SerializeField] protected float damage;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float range;
        [SerializeField] protected float ammoCapacity;
        [SerializeField] protected float reloadTime;

        protected float nextFireTime;
        protected float currentAmmo;


        protected virtual void Update()
        {
            if (currentAmmo <= 0)
            {
                Reload();
            }
        }

        public virtual bool CanFire()
        {
            return Time.time >= nextFireTime && currentAmmo > 0;
        }

        public virtual void Fire()
        {
            if (!CanFire()) return;

            float coolDown = 1 / fireRate;
            nextFireTime = Time.time + coolDown;
            currentAmmo--;

            ExecuteShot();
        }

        protected abstract void ExecuteShot();

        public virtual void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(reloadTime);
            currentAmmo = ammoCapacity;
        }
    }
}
