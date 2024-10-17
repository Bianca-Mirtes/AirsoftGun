using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    public Transform gunBarrel;
    private ChargerController charger = null;
    public  GameObject bb;
    public void SetCharger(ChargerController charger)
    {
        this.charger = charger;
    }

    public ChargerController GetCharger()
    {
        return charger;
    }
    
    public int getCurrentBBs()
    {
        return charger.getCurrentBullets();
    }

    public void shoot()
    {
        Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel);
        charger.consumeBB();
    }
}
