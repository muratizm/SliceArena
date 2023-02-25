using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Shield : MonoBehaviour
{
    private bool isRight;
    Animator animator;
    public int needStamina;
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("ShieldDeactive", 0.5f);
    }

    public void ShieldDeactive(){

        if(isRight)
        {
            animator.Play("ShieldRightDown");
        }
        else
        {
            animator.Play("ShieldLeftDown");
        }
        
    }

    public void ShieldActive(){
        if(gameObject.activeSelf){
            // bu cancel invoke'u yaptım çünkü eğer kalkan açıkken tekrardan kalkanı açmaya çalışırsam, önceden açtığımda 1 saniye sonra kapat dediğim için yeni açtığım boşa gidiyordu hemen geri kapanıyordu.
            // artık kalkanı açtığımda kalkan zaten açıksa önceden verilen 1 saniye sonra kapatma emri iptal oluyor bu emri tekrar veriyor
            // yani stamina oldugu sürece kalkanı tekrar basarsak elimizde tutabiliyoruz.
            
            CancelInvoke();
        }
        if(StaminaBar.instance.isEnough(needStamina)){
            if(isRight){
                animator.Play("ShieldRight");
            }
            else{
                animator.Play("ShieldLeft");
            }
            StaminaBar.instance.UseStamina(needStamina);
           
        }
        
        Invoke("ShieldDeactive", 1f);
    }

    public bool getIsRight(){
        return isRight;
    }

    public void setIsRight(bool _isRight){
        isRight = _isRight;
    }

}
