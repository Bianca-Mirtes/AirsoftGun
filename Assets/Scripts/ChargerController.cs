using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{

    [Header("Infos Gun")]
    public TYPE type;
    [SerializeField] private int capacity;
    [SerializeField] private int currentBullets;

    [Header("BBs")]
    public GameObject bbPrefab;

    private void Start()
    {
        fullAuto();
    }

    public float GetMassBB()
    {
        return bbPrefab.GetComponent<BBController>().GetMass();
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
