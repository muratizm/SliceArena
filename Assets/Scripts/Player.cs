using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int hp = 100;

    private Shield shield;
    //public TextMeshProUGUI moneyText;

    private void Start() {
        shield = FindObjectOfType<Shield>();
        //moneyText.text = "Fame: " + ShopSystem.ShopData.totalFame;
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name == "Projectile(Clone)"){
            hp -= 10;
        }
    }


    public void changeFame(int change){
        ShopSystem.ShopUI.totalFame += change; 
        //moneyText.text = "Total Fame: " + ShopSystem.ShopUI.totalFame;
        PlayerPrefs.SetInt("TotalFame", ShopSystem.ShopUI.totalFame);
        //Debug.Log(ShopSystem.ShopUI.totalFame);
    }


    void setHp(int newHp){
        hp = newHp;
    }
}
