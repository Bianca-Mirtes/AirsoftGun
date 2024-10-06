using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirsoftGunController : MonoBehaviour
{
    public enum TYPE
    {
        SHOTGUN,
        RIFLE,
        P1911,
        GLOCK
    }

    public TYPE tipo;

    [Header("BBs")]
    public GameObject bb1;
    public GameObject bb2;
    public GameObject bb3;

    [Header("GameObjects")]
    public Transform slot;
    public Transform gunBarrel;

    [Header("Time")]
    [SerializeField][Range(0, 1)] private float time = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            shoot(bb1);
        else if (Input.GetButtonDown("Fire2"))
            shoot(bb2);
        /*else if (Input.GetKeyDown(KeyCode.E))
        {
            shoot(bb3);
        }*/
    }

    private void FixedUpdate()
    {
        Time.timeScale = time;
    }

    private void shoot(GameObject bb)
    {
        Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel); //LocalPosition seta muito errado
    }
}
