using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameWinPanel;
    public GameObject room;
    public Camera myCamera;
    private GameObject[] gameObjects;

    


    public void NewFight()
    {
        DestroyAllObjects();
        Invoke("NewScene", 0.5f);
        Invoke("WinPanel", 1.5f);
       

    }

    void WinPanel(){
        GameWinPanel.SetActive(true);
    }
    void DestroyAllObjects()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("dead");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
    private void NewScene()
    {
        
        PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoToLevelScene()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
