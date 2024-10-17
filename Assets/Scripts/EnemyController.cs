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

    [SerializeField] private float distanceForAttack = 10f, distanceForAttenction = 15f;
    private bool isDead = false;
    private float timeForFire = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rifle.SetCharger(charge);
        player = GameObject.FindWithTag("Player").transform;
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

            Vector3 direction = new Vector3(player.GetChild(1).position.x, transform.position.y, player.GetChild(1).position.z) - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            float distanceForPlayer = Vector3.Distance(transform.position, player.GetChild(1).position);
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
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("End"))
                Destroy(gameObject);
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
            timeForFire = 0.9f;
        }
        else
            timeForFire -= Time.deltaTime;
    }

    public void ReloadingGun()
    {
        ani.SetBool("isReloading", true);
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
            if(player.GetComponent<PlayerController>().GetCurrentGun().type == TYPE.SHOTGUN)
                receiveDamage(15);
            if(player.GetComponent<PlayerController>().GetCurrentGun().type == TYPE.P1911 || player.GetComponent<PlayerController>().GetCurrentGun().type == TYPE.GLOCK)
                receiveDamage(5);
            if(player.GetComponent<PlayerController>().GetCurrentGun().type == TYPE.RIFLE)
                receiveDamage(10);
            Destroy(other.gameObject, 2f);
        }
    }

    public void receiveDamage(int damage)
    {
        health -= damage;
        ani.SetTrigger("isDamage");
        Debug.Log("Acertou o inimigo");
    }
}
