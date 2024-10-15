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
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        rifle.SetCharger(charge);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            float distanceForPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceForPlayer <= distanceForAttenction)
                AimingRifle();
            else
                ani.SetBool("isAimingRifle", false);

            if (distanceForPlayer <= distanceForAttack)
                Firing();
            else
                ani.SetBool("isFiringGun", false);

            if(health <= 0)
                Dead();
            if (rifle.getCurrentBBs() <= 0)
                ReloadingGun();

            while (ani.GetBool("isFiringGun"))
                rifle.shoot();
        }
    }

    public void AimingRifle()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        ani.SetBool("isAimingRifle", true);
    }

    public void Firing()
    {
        ani.SetBool("isFiringGun", true);
    }

    public void ReloadingGun()
    {
        ani.SetBool("isReloading", true);
        rifle.GetCharger().fullAuto();
    }

    public void Dead()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        ani.SetBool("isDead", true);
        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BB"))
        {
            health -= 10;
            ani.SetBool("isDamage", true);
        }
    }
}
