using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(Mathf.Clamp(transform.rotation.x, 38f, -25f), transform.rotation.y, transform.rotation.z);
    }
}
