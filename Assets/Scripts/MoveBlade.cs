using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlade : MonoBehaviour {
    //referances 
   

    bool isCutting = false;
Rigidbody2D rb;
Camera cam;

private void Start()
{
    cam = Camera.main;
    rb = GetComponent<Rigidbody2D>();
}



void Update()
{
    
    if (Input.GetMouseButtonDown(0))
    {
        StartCutting();
    }
    else if (Input.GetMouseButtonUp(0))
    {
        StopCutting();
    }

    if (isCutting)
    {
        UpdateCut();
    }
    
}

//functions

//sonradan
    void UpdateCut()
    {

    rb.position = cam.WorldToViewportPoint(Input.mousePosition);


    }

    void StartCutting()
    {
    isCutting = true;

    }

    void StopCutting()
    {
    isCutting = false;

    }
}
