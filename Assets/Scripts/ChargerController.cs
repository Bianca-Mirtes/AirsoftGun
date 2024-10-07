using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public TYPE type;

    [SerializeField] private int capacity;
    [SerializeField] private int currentBullets;
    [SerializeField] private float currentMassBBs;
    // Start is called before the first frame update
    void Start()
    {
        
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
