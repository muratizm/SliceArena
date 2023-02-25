using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    int levels;
    private OptionsManager optionsManager;
    private void Start()
    {
        // PlayerPrefs.DeleteAll();
        //audioSource = GetComponent<AudioSource>();
        optionsManager = FindObjectOfType<OptionsManager>();
        /*
        if(SceneManager.GetActiveScene().path == "Assets/Scenes/Levels/" + SceneManager.GetActiveScene().name + ".unity"){
            Audio.AudioManager.stopTheme();
        }
        else{
            Audio.AudioManager.startTheme();
        }
        */
        lock_level_system();
    }
    public void Update()
    {
        lock_level_system();
        print(PlayerPrefs.GetInt("level"));
        
    }
    public Button[] buttons;
    public void Open_Level(string level_name)
    {
        Audio.AudioManager.playAudio(level_name);
        SceneManager.LoadScene(level_name);        
    }

    public void ResetGame(){
        ShopSystem.SaveLoadData.ClearData();
        PlayerPrefs.DeleteAll();
        optionsManager.ResetPrefs();
    }

    public void lock_level_system()
    {

        // key value kafası levellerın intleri var
        //Debug.Log(PlayerPrefs.GetInt("level"));
        levels = PlayerPrefs.GetInt("level");
        if (levels == 0)
        {
            levels = 1;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            //parse to int yapıyor stringi

            if (levels >= int.Parse(buttons[i].name ))
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;

            }
        }
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
