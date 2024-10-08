using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public TYPE type;

    [SerializeField] private int capacity;
    [SerializeField] private int currentBullets;
    [SerializeField] private float massBBs;

    private void Start()
    {
        if(type == TYPE.RIFLE)
        {
            massBBs = 0.28f;
        }
        if(type == TYPE.P1911 || type == TYPE.GLOCK || type == TYPE.SHOTGUN)
        {
            massBBs = 0.2f;
        }

        fullAuto();
    }

    public float GetMassBB()
    {
        return massBBs;
    }
    public int GetCapacity()
    {
        return capacity;
    }

    public int getCurrentBullets()
    {
        return currentBullets;
    }

    public void consumeBB()
    {
        --currentBullets;
    }

    public void fullAuto()
    {
        currentBullets = capacity;
    }
}
