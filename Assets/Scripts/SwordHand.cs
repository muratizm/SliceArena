using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHand : MonoBehaviour
{
    private MeshFilter meshFilter;
    void Start(){
        ShopSystem.ShopUI.weaponName = PlayerPrefs.GetString("WeaponName");

        meshFilter = GetComponent<MeshFilter>();
        if(ShopSystem.ShopUI.weaponName != null){
            meshFilter.sharedMesh = Resources.Load<GameObject>(ShopSystem.ShopUI.weaponName).GetComponent<MeshFilter>().sharedMesh;
        }
    }
    

   
}
