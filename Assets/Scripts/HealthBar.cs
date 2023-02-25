using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;


public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int health, int maxHp){
         slider.maxValue = maxHp;
         slider.value = health;
    }
}
