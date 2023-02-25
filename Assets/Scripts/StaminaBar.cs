using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;
    public static StaminaBar instance;

    private void Awake(){
        instance = this;
    }

    private void Start() {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;    
    }

    public void UseStamina(int amount){
        currentStamina -= amount;
        staminaBar.value = currentStamina;

        if(regen != null){
            StopCoroutine(regen);
        }

        regen = StartCoroutine(RegenStamina());
    }

    public bool isEnough(int amount){
        return (currentStamina - amount >= 0);
    }


    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(0.2f);

        while(currentStamina < maxStamina){
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }
}
