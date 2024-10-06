using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Pistol1911"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.transform.parent = transform;
                other.gameObject.transform.position = Vector3.zero;
            }
        }
        if (other.gameObject.tag.Equals("PistolGlock"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.transform.parent = transform;
                other.gameObject.transform.position = Vector3.zero;
            }

        }
        if (other.gameObject.tag.Equals("AK47"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.transform.parent = transform;
                other.gameObject.transform.position = Vector3.zero;
            }
        }
        if (other.gameObject.tag.Equals("Shotgun"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.transform.parent = transform;
                other.gameObject.transform.position = Vector3.zero;
            }
        }
    }
}
