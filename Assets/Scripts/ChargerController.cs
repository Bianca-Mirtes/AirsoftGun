using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public TYPE type;

    [SerializeField] private int capacity;
    [SerializeField] private int currentCapacity;
    [SerializeField] private float currentMassBBs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int getCurrentCapacity()
    {
        return currentCapacity;
    }

    public void consumeBB()
    {
        --currentCapacity;
    }

    public void fullAuto()
    {
        currentCapacity = capacity;
    }
}
