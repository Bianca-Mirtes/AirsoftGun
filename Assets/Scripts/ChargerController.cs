using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

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
        currentBullets = capacity;
    }

    public float GetMassBB() { return bbPrefab.GetComponent<BBController>().GetMass(); }
    public int GetCapacity() { return capacity; }

    public int getCurrentBullets() {  return currentBullets; }

    public void consumeBB()
    {
        --currentBullets;
    }

    public void Reaload()
    {
        Transform slots = GameObject.FindWithTag("Slots").transform;
        for (int ii = 0; ii < slots.childCount; ii++)
        {
            Image slot = slots.GetChild(ii).GetComponent<Image>();
            if (slot.sprite != null && slot.sprite.name == "AmmoBox")
            {
                if (type == TYPE.SHOTGUN)
                    currentBullets += 6;
                if (type == TYPE.GLOCK)
                    currentBullets += 11;
                if (type == TYPE.P1911)
                    currentBullets += 15;
                if (type == TYPE.RIFLE)
                    currentBullets += 50;
                slot.sprite = null;
                break;
            }
        }
    }
}
