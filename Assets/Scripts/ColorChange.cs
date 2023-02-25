using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public Material myMaterial;

    public void SetColorToRed()
    {
        myMaterial.color = Color.red;
    }
    public void SetColorToBlue()
    {
        myMaterial.color = Color.blue;
    }
}
