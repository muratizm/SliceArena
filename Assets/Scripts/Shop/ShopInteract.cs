using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopInteract : MonoBehaviour
{
    //references
    [SerializeField]
    private GameObject weaponShop;
    [SerializeField]
    private GameObject armorShop;

    private Animator animator;
    private AnimationClip animationClip;
    private float time;


    private void Start() {
        animator = FindObjectOfType<Animator>();
        //bu kısımda animasyonunsüresini ögreniyorum
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if(clip.name == "FadeOut"){
                time = clip.length;
            }
        }

        // bu sekilde saçma oldu biliyorum ama eskiden FadeOut animasyonu çalışmıyordu
        // bu animasyonun sonuna tetikleyici koyup bir şey yaptırma olayını tam anlayamadım belki farklı objelerde oldugu icin erişim sağalyamıyordu
        // fadeout çalışmadan süre bitince yana geçiyordu mecbur böyle yaptım
        // eski haline çevirmek daha mantıklı olacaktır bu da çalışıyor ama köylü işi gibi
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject == weaponShop)
                    {
                        animator.Play("FadeOut");
                        Audio.AudioManager.playAudio("WeaponShop");
                        Invoke(nameof(GoToWeaponShop), time);
                        //FadeOut();
                        
                    }
                    
                }
            }
        }
    }

    public void FadeOut(){
        //animator.SetTrigger("FadeOut");
    }
    private void GoToWeaponShop()
    {
        SceneManager.LoadScene("WeaponShop");
    }
    private void GoToArmorShop()
    {
        SceneManager.LoadScene("ArmorShop");
    }
}
