using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using TMPro;

public class Cut : MonoBehaviour
{
    //referances 
    public GameObject myGameObj;
    public GameObject lastHit;
    public Camera myCamera;
    public Material mat;
    public LayerMask mask;
    private float angle;
    private Vector3 mousePos1;
    private Vector3 mousePos2;
    private Enemy enemy;
    private bool isDead;
    public float minCuttingVelocity = .02f;
    private Shield shield;
    Ray mray;
    public TextMeshProUGUI enemyHpText;
    public TextMeshProUGUI enemyNameText;
    private Animator animator;
    private HealthBar healthBar;
    private int currentDamage;
    private int bonusFactor = 1;
    private AudioSource audioSource;


    public void OnDrawGizmos()
    {
        EzySlice.Plane cuttingPlane = new EzySlice.Plane();
        cuttingPlane.Compute(transform);
        cuttingPlane.OnDebugDraw();
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentDamage = PlayerPrefs.GetInt("WeaponDamage");
        Debug.Log(currentDamage);

        
        
        shield = FindObjectOfType<Shield>();
        healthBar = FindObjectOfType<HealthBar>();
        animator = GameObject.FindGameObjectWithTag("Hand").GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mray = myCamera.ScreenPointToRay(Input.mousePosition);
            mousePos1 = Input.mousePosition;
            mousePos1.z = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(mray, out RaycastHit raycastHit))
            {
                transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
                lastHit = raycastHit.transform.gameObject;
                Vector3 collision = raycastHit.point;
            }

            mousePos2 = Input.mousePosition;
            mousePos2.z = 0;
            angle = Vector3.Angle(mousePos1 - mousePos2, new Vector3(-1, 0, 0));


            float velocity = (mousePos2 - mousePos1).magnitude * Time.deltaTime;

            if (velocity > minCuttingVelocity)
            {
                Search();
                shield.ShieldDeactive();
                            
                if (mousePos1.y > mousePos2.y)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -angle);
                    if(angle < 90){
                        //LTRB
                        animator.Play("SwordLTRB");
                        audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack1");
                    }
                    else{
                        //RTLB
                        animator.Play("SwordRTLB");
                        audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack5");
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    if(angle < 90){
                        //LBRT
                        animator.Play("SwordLBRT");
                        audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack4");
                    }
                    else{
                        //RBLT
                        animator.Play("SwordRBLT");
                        audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack4");
                    }
                }
            }
            else
            {
                if(mousePos2.x > Screen.width / 2){
                    shield.setIsRight(true);
                    audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack2");
                    shield.ShieldActive();
                }
                else{
                    shield.setIsRight(false);
                    audioSource.clip = (AudioClip)Resources.Load("FeelBarbariansAttack2");
                    shield.ShieldActive();
                }
            }

            

            audioSource.Play();

            

        }

    }
    //functions

    public void Search()
    {
        //alt tarafı anla
        Collider[] EnemyObjects = Physics.OverlapBox(transform.position, new Vector3(5f, 0.1f, 5f), transform.rotation, mask);
        foreach (Collider obj in EnemyObjects)
        {
            if (isDead)
            {
                SlicedHull kesilmisObje = Kes(obj.gameObject, mat);
                if (kesilmisObje != null)
                {
                    GameObject kesilmisUst = kesilmisObje.CreateUpperHull(obj.gameObject, mat);
                    GameObject kesilmisAlt = kesilmisObje.CreateLowerHull(obj.gameObject, mat);
                    BilesenEkle(kesilmisUst);
                    BilesenEkle(kesilmisAlt);

                    Destroy(obj.gameObject);
                }
            }

            if(obj.tag == "Enemy"){
                
                enemy = obj.gameObject.GetComponent<Enemy>();

                if(!enemy.getIsDead())
                {
                    foreach (var d in EnemyObjects)
                    {
                        if(d.tag != "Enemy"){
                            var s = d.GetComponent<Damages>();
                            switch (s.damageType)
                            {
                                case Damages.collisionType.head:
                                    Debug.Log(currentDamage);
                                    enemy.damage(currentDamage * 2 * bonusFactor);
                                    break;
                                case Damages.collisionType.body:
                                    Debug.Log(currentDamage);
                                    enemy.damage(currentDamage * bonusFactor);
                                    break;
                                default:
                                    break;
                            }
                            bonusFactor = 1;
                        }
                        
                    }
                    
                        
                    
                    //enemy.damage(10);
                    enemyHpText.text = enemy.getHp().ToString();
                    enemyNameText.text = obj.gameObject.name;
                    //healthBar.SetHealth(enemy.getHp(), enemy.getMaxHp());
                }
            }
            else if(obj.tag == "Bonus"){
                BonusEnemy bonusEnemy = obj.GetComponent<BonusEnemy>();
                bonusEnemy.Die();
            }

        }
    }


    public SlicedHull Kes(GameObject obj, Material mat = null)
    {
        return obj.Slice(transform.position, transform.up, mat);
    }

    void BilesenEkle(GameObject obj)
    {
        obj.tag = "dead";
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.GetComponent<Rigidbody>().drag = 10;
        obj.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        obj.GetComponent<Rigidbody>().AddExplosionForce(100, obj.transform.position, 20);
        obj.gameObject.layer = LayerMask.NameToLayer("WhatIsEnemy");
    }

    public void setBonusFactor(int bonus){
        bonusFactor = bonus;
    }


}
