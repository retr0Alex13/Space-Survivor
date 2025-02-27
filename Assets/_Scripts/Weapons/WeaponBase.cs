using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidwalker.Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField] protected string weaponName;
        [SerializeField] protected float baseDamage;
        [SerializeField] protected float baseFireRate;
        [SerializeField] protected float baseRange;
        [SerializeField] protected float baseHeatGeneration;
        [SerializeField] protected int ammoPerShot;
        [SerializeField] protected int maxAmmo;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected AudioClip fireSound;
        [SerializeField] protected ParticleSystem muzzleFlash;

        // Налаштування для мультистволової стрільби
        [SerializeField] protected bool useAllBarrelsAtOnce = true;
        [SerializeField] protected float barrelFireDelay = 0.05f;
        [SerializeField] protected FirePattern barrelFirePattern = FirePattern.Sequential;

        protected int currentAmmo;
        protected float lastFireTime;
        protected int lastBarrelIndex = 0;
        protected float damageModifier = 1f;
        protected float fireRateModifier = 1f;
        protected float rangeModifier = 1f;
        protected float heatModifier = 1f;

        public enum FirePattern
        {
            Sequential,     // По черзі
            Random,         // Випадково
            Simultaneous,   // Одночасно
            AlternatingPairs, // Чергуючи пари
            OutsideIn,      // Від країв до центру
            InsideOut       // Від центру до країв
        }

        public string WeaponName => weaponName;
        public float Damage => baseDamage * damageModifier;
        public float FireRate => baseFireRate * fireRateModifier;
        public float Range => baseRange * rangeModifier;
        public float HeatGeneration => baseHeatGeneration * heatModifier;
        public int AmmoPerShot => ammoPerShot;

        protected AudioSource audioSource;

        protected virtual void Awake()
        {
            currentAmmo = maxAmmo;
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public virtual void Fire(Transform firePoint)
        {
            if (currentAmmo < ammoPerShot) return;
            if (Time.time - lastFireTime < 1f / FireRate) return;

            lastFireTime = Time.time;
            currentAmmo -= ammoPerShot;

            PlayFireEffects(firePoint);
            SpawnProjectile(firePoint);
        }

        public virtual void FireFromMultiplePoints(List<Transform> firePoints)
        {
            if (firePoints == null || firePoints.Count == 0) return;
            if (currentAmmo < ammoPerShot * (useAllBarrelsAtOnce ? firePoints.Count : 1)) return;
            if (Time.time - lastFireTime < 1f / FireRate) return;

            lastFireTime = Time.time;

            if (useAllBarrelsAtOnce)
            {
                // Використовуємо всі стволи одразу
                currentAmmo -= ammoPerShot * firePoints.Count;

                if (barrelFirePattern == FirePattern.Simultaneous)
                {
                    // Стріляємо з усіх стволів одночасно
                    foreach (Transform firePoint in firePoints)
                    {
                        PlayFireEffects(firePoint);
                        SpawnProjectile(firePoint);
                    }
                }
                else
                {
                    // Послідовно стріляємо з усіх стволів із затримкою
                    StartCoroutine(FirePatternSequence(firePoints));
                }
            }
            else
            {
                // Використовуємо один ствол за раз
                currentAmmo -= ammoPerShot;

                // Вибираємо ствол відповідно до шаблону вогню
                Transform selectedFirePoint = SelectNextFirePoint(firePoints);

                PlayFireEffects(selectedFirePoint);
                SpawnProjectile(selectedFirePoint);
            }
        }

        protected Transform SelectNextFirePoint(List<Transform> firePoints)
        {
            int count = firePoints.Count;

            switch (barrelFirePattern)
            {
                case FirePattern.Sequential:
                    lastBarrelIndex = (lastBarrelIndex + 1) % count;
                    return firePoints[lastBarrelIndex];

                case FirePattern.Random:
                    return firePoints[Random.Range(0, count)];

                case FirePattern.Simultaneous:
                    // Не має значення, який ми повернемо, оскільки цей режим обробляється окремо
                    return firePoints[0];

                case FirePattern.AlternatingPairs:
                    lastBarrelIndex = (lastBarrelIndex + 2) % count;
                    return firePoints[lastBarrelIndex];

                case FirePattern.OutsideIn:
                    // Першими стріляють крайні стволи, потім більш центральні
                    if (lastBarrelIndex < count / 2)
                    {
                        lastBarrelIndex++;
                        return firePoints[lastBarrelIndex - 1];
                    }
                    else
                    {
                        lastBarrelIndex = 0;
                        return firePoints[count - 1];
                    }

                case FirePattern.InsideOut:
                    // Першими стріляють центральні стволи, потім крайні
                    if (lastBarrelIndex == 0)
                    {
                        lastBarrelIndex = count / 2;
                        return firePoints[lastBarrelIndex];
                    }
                    else if (lastBarrelIndex < count - 1)
                    {
                        int offset = lastBarrelIndex - count / 2 + 1;
                        if (offset > 0)
                        {
                            lastBarrelIndex = count / 2 - offset;
                        }
                        else
                        {
                            lastBarrelIndex = count / 2 - offset;
                        }
                        return firePoints[lastBarrelIndex];
                    }
                    else
                    {
                        lastBarrelIndex = 0;
                        return firePoints[0];
                    }

                default:
                    return firePoints[0];
            }
        }

        protected IEnumerator FirePatternSequence(List<Transform> firePoints)
        {
            List<int> firingOrder = new List<int>();
            int count = firePoints.Count;

            // Визначаємо порядок стрільби відповідно до вибраного шаблону
            switch (barrelFirePattern)
            {
                case FirePattern.Sequential:
                    for (int i = 0; i < count; i++)
                    {
                        firingOrder.Add((lastBarrelIndex + i + 1) % count);
                    }
                    lastBarrelIndex = firingOrder[count - 1];
                    break;

                case FirePattern.Random:
                    List<int> indices = new List<int>();
                    for (int i = 0; i < count; i++) indices.Add(i);

                    while (indices.Count > 0)
                    {
                        int randomIndex = Random.Range(0, indices.Count);
                        firingOrder.Add(indices[randomIndex]);
                        indices.RemoveAt(randomIndex);
                    }
                    break;

                case FirePattern.AlternatingPairs:
                    for (int i = 0; i < count; i += 2)
                    {
                        firingOrder.Add(i % count);
                        if (i + 1 < count)
                            firingOrder.Add((i + 1) % count);
                    }
                    break;

                case FirePattern.OutsideIn:
                    for (int i = 0; i < count / 2; i++)
                    {
                        firingOrder.Add(i);
                        firingOrder.Add(count - 1 - i);
                    }
                    if (count % 2 == 1)
                        firingOrder.Add(count / 2);
                    break;

                case FirePattern.InsideOut:
                    firingOrder.Add(count / 2);
                    if (count % 2 == 0)
                        firingOrder.Add(count / 2 - 1);

                    for (int i = 1; i <= count / 2; i++)
                    {
                        if (count / 2 + i < count)
                            firingOrder.Add(count / 2 + i);
                        if (count / 2 - i >= 0 && count % 2 == 1)
                            firingOrder.Add(count / 2 - i);
                        else if (count / 2 - 1 - i >= 0 && count % 2 == 0)
                            firingOrder.Add(count / 2 - 1 - i);
                    }
                    break;

                default:
                    for (int i = 0; i < count; i++)
                    {
                        firingOrder.Add(i);
                    }
                    break;
            }

            // Стріляємо з кожного ствола з затримкою
            for (int i = 0; i < firingOrder.Count; i++)
            {
                Transform firePoint = firePoints[firingOrder[i]];
                PlayFireEffects(firePoint);
                SpawnProjectile(firePoint);

                if (i < firingOrder.Count - 1)
                    yield return new WaitForSeconds(barrelFireDelay);
            }
        }
        protected void PlayFireEffects(Transform firePoint)
        {
            if (muzzleFlash != null)
            {
                ParticleSystem muzzleInstance = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation, firePoint);
                muzzleInstance.Play();
                Destroy(muzzleInstance.gameObject, muzzleInstance.main.duration);
            }

            if (fireSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(fireSound);
            }
        }

        protected abstract void SpawnProjectile(Transform firePoint);

        public virtual void Reload()
        {
            currentAmmo = maxAmmo;
        }

        public virtual void Upgrade(WeaponUpgrade upgrade)
        {
            damageModifier += upgrade.DamageIncrease;
            fireRateModifier += upgrade.FireRateIncrease;
            rangeModifier += upgrade.RangeIncrease;
            heatModifier -= upgrade.HeatReduction;
            maxAmmo += upgrade.ExtraAmmoCapacity;
        }
    }
}
