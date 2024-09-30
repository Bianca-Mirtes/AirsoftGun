using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineController : MonoBehaviour
{
    public enum TYPE {
        SHOTGUN,
        RIFLE,
        P1911,
        GLOCK
    }

    public TYPE tipo;

    [SerializeField] private int capacity;
    private int currentCapacity;
    private float currentMassBBs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
