using System.Collections.Generic;
using UnityEngine;

namespace Voidwalker.Weapons
{
    public interface IWeapon
    {
        string WeaponName { get; }
        float Damage { get; }
        float FireRate { get; }
        float Range { get; }
        float HeatGeneration { get; }
        int AmmoPerShot { get; }

        void Fire(Transform firePoint);
        void FireFromMultiplePoints(List<Transform> firePoints);
        void Reload();
        void Upgrade(WeaponUpgrade upgrade);
    }
}
