using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using MoreMountains.Tools;
public class Enemy : MonoBehaviour
{
    //Warning
    private ColorChange colorChange;
    //Health
    public GameObject[] positions;
    private int hp;
    public int maxHp;
    public GameObject FloatingTextPrefab;

    private Transform playerPos;
    private Animator animator;
    private bool isDead;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    private  Rigidbody rb;
    private Damages damages;
    private GameManager gameManager;
    private Player player;
    private AudioSource audioSource;

    private MMProgressBar mMProgressBar;
   // public enum collisionType {head, body}
   // public collisionType damageType;
    //States

    private void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        hp = maxHp;
        //warning
        colorChange = gameObject.GetComponentInChildren<ColorChange>();
        //hp = 100;
        playerPos = GameObject.Find("PositionPlayer").transform;
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mMProgressBar = FindObjectOfType<MMProgressBar>();
    }

    private void Update()
    {
        
        transform.LookAt(playerPos);
        if (!alreadyAttacked && hp > 0)
        {
            AttackPlayer();
        }
        

    }

    private void AttackPlayer()
    {
        
        ///Attack code here
        animator.Play("Attack0");
        Invoke("BulletGo", 0.5f);
        ///End of attack cod

        alreadyAttacked = true;

        int number = Random.Range(0,3);
        Vector3 newPosition = positions[number].transform.position;
        StartCoroutine(MoveOverSeconds(gameObject, newPosition, 0.5f));
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        
    }

    
    public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed){
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame ();
        }
    }

    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void BulletGo(){
        Rigidbody rb_projectile = Instantiate(projectile, new Vector3(transform.position.x , transform.position.y + 1, transform.position.z-1), Quaternion.identity).GetComponent<Rigidbody>();
        
        rb_projectile.AddForce(Physics.gravity * rb_projectile.mass);
        rb_projectile.AddForce(transform.forward * 40f, ForceMode.Impulse);
        rb_projectile.AddForce(transform.up * 5f, ForceMode.Impulse);
    }


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    //Health

    public void setHp(int newHp)
    {
        hp = newHp;
    }

    public void damage(int dmg){
        mMProgressBar.Minus((float)dmg/maxHp);
        hp -= dmg;
        if(hp<=0){
            animator.Play("Dead0");
            audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansGrunt1");
            audioSource.Play();
            gameManager.NewFight();
            player.changeFame(20);
        }
        else{
            animator.Play("Damage0");
            audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansGrunt" + Random.Range(2,6).ToString());
            audioSource.Play();
            player.changeFame(5);
        }
        ShowFloatingText(dmg);
    }

    public int getHp()
    {
        if(hp<=0){
            return 0;
        }
        else{
            return hp;
        }
    }

    public int getMaxHp(){
        return maxHp;
    }
    public void ShowFloatingText(int damage)
    {
        if(FloatingTextPrefab != null){
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
            go.GetComponent<TextMeshPro>().text = damage.ToString();
        }

    }

    public void setIsDead(bool _isDead){
        isDead = _isDead;
    }

    public bool getIsDead(){
        return isDead;
    }

}
