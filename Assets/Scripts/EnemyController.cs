using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Transform player;

    [Header("Attributes")]
    [SerializeField] private Animator ani;
    [SerializeField] private int damage;
    [SerializeField] private int health = 100;

    [Header("Equipaments")]
    public EnemyGunController rifle;
    public ChargerController charge;

    [SerializeField] private float distanceForAttack = 15f, distanceForAttenction = 25f;
    private bool isDead = false;
    private float timeForFire = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rifle.SetCharger(charge);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead){
            if (health <= 0){
                ani.SetBool("isAimingRifle", false);
                Dead();
                return;
            }

            //transform.LookAt(player);

            Vector3 direction = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            float distanceForPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceForPlayer <= distanceForAttenction)
                AimingRifle();
            else
                ani.SetBool("isAimingRifle", false);

            if (distanceForPlayer <= distanceForAttack)
                Firing();
            else
                ani.SetBool("isFiringGun", false);
            if (rifle.getCurrentBBs() <= 0)
                ReloadingGun();

        }
        else
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("End")){
                Destroy(gameObject);
            }
        }
    }

    public void AimingRifle()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        ani.SetBool("isAimingRifle", true);
    }

    public void Firing()
    {
        if(timeForFire <= 0)
        {
            ani.SetBool("isFiringGun", true);
            rifle.shoot();
            timeForFire = 0.8f;
        }
        else
        {
            timeForFire -= Time.deltaTime;
        }
    }

    public void ReloadingGun()
    {
        ani.SetBool("isReloading", true);
        rifle.GetCharger().fullAuto();
    }

    public void Dead()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        ani.SetTrigger("isDead");
        isDead = true;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("BB") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Dead")) { 
            receiveDamage();
        }
    }

    //private void OnTriggerEnter(Collider other){
    //    if (other.gameObject.CompareTag("BB") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Dead")){
    //        receiveDamage();
    //    }
    //}

    public void receiveDamage()
    {
        health -= 10;
        ani.SetTrigger("isDamage");
        Debug.Log("Acertou o inimigo");
    }
}
