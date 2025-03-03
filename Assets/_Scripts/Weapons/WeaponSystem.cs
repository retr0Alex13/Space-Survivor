using System.Collections.Generic;
using UnityEngine;

namespace Voidwalker.Weapons
{
    public class WeaponSystem : MonoBehaviour
    {
        [Header("Weapon Management")]
        [SerializeField] private List<BaseWeapon> availableWeapons;
        
        private int currentWeaponIndex;

        private BaseWeapon currentWeapon => 
            availableWeapons.Count > 0 ? availableWeapons[currentWeaponIndex] : null;

        private void Start()
        {
            if (currentWeapon != null)
            {
                currentWeapon.Reload();
            }
            else
            {
                Debug.LogWarning("No weapon assigned to the player.");
            }
        }

        public void FireFromWeapon()
        {
            if (currentWeapon != null)
            {
                currentWeapon.Fire();
            }
            else
            {
                Debug.LogWarning("No weapon assigned to the player.");
            }
        }
    }
}
