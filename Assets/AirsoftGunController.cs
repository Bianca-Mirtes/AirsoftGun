using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirsoftGunController : MonoBehaviour
{
    public GameObject bb;
    public Transform slot;
    public Transform pipeTip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bb, pipeTip.localPosition, Quaternion.identity, slot);
        }
    }
}
