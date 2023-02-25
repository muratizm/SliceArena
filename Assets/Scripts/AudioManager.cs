using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio{
    public class AudioManager : MonoBehaviour
    {    
        private static AudioManager audioManagerInstance;
        public static AudioClip audioClip;
        static AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        
            if(audioManagerInstance == null){
                audioManagerInstance = this;
                DontDestroyOnLoad(this);
                
            }
            else{
                Destroy(gameObject);
            }
    
        
        }


        public static void playAudio(string level_name){
            audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
            //audioSource = FindObjectOfType<AudioSource>();
            switch (level_name)
            {
                case "MainMenu":
                    audioClip = (AudioClip)Resources.Load("prevnext");
                    break;
                case "LevelScene":
                    audioClip = (AudioClip)Resources.Load("prevnext");
                    break;
                case "ShopMain":
                    audioClip = (AudioClip)Resources.Load("prevnext");
                    break;
                case "WeaponShop":
                    audioClip = (AudioClip)Resources.Load("anvilhammer");
                    break;
                default:
                    audioClip = (AudioClip)Resources.Load("prevnext");
                    break;
            }
            audioSource.clip = audioClip;
            audioSource.Play();

        }

        public static void stopTheme(){
            audioSource = GameObject.FindGameObjectWithTag("Theme").GetComponent<AudioSource>();
            audioSource.Stop();
        }

        public static void startTheme(){
            audioSource = GameObject.FindGameObjectWithTag("Theme").GetComponent<AudioSource>();
            if(!audioSource.isPlaying){
                audioSource.Play();     
            }
            
        }




    }

}
