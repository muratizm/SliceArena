using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public GameObject[] Tutorials;
    private int count = 0;    

    void Start()
    {
        Invoke("ActivateTutorial", 1f);
    }


    public void ActivateTutorial(){
        if(count < Tutorials.Length) {
            Tutorials[count].SetActive(true);
            count++;
        }
        
    }

    public void CancelTutorial(GameObject gameObj){
        gameObj.SetActive(false);
        Invoke("ActivateTutorial", 0.1f);
    }
}
