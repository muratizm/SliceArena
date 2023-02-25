using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusEnemy : MonoBehaviour
{
    private Cut cut;
    public GameObject FloatingTextPrefab;
    private int bonusFactor = 2;
    void Start()
    {
        cut = FindObjectOfType<Cut>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0.05f, 0, 0);
    }

    public void Die(){
        Debug.Log(bonusFactor + "x Bonus kazandınız");
        cut.setBonusFactor(bonusFactor);
        ShowFloatingText();
        Destroy(gameObject);
    }

    public void ShowFloatingText()
    {
        if(FloatingTextPrefab != null){
            var go = Instantiate(FloatingTextPrefab, transform.position - new Vector3(0, 1, 0), Quaternion.identity);
            go.GetComponent<TextMeshPro>().text = bonusFactor + "x Bonus";
        }

    }
}
