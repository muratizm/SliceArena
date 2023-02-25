using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopSystem
{

    [System.Serializable]
    public class ShopData
    {
        //public int totalFame;
        public ShopItem[] shopItems;
        
    }
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public bool isUnlocked;
        public int unlockCost;
        public int unlockedLevel;
        public WeaponInfo[] weaponLevel;
    }

    [System.Serializable]
    public class WeaponInfo
    {
        public int unlockCost;
        public int damage;

    }
}
