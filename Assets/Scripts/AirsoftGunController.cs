using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TYPE{
    GLOCK,
    P1911,
    RIFLE,
    SHOTGUN
}

public class AirsoftGunController : MonoBehaviour
{
    public TYPE type;

    [Header("BBs")]
    public GameObject bb1;
    public GameObject bb2;
    public GameObject bb3;

    [Header("GameObjects")]
    public Transform slot;
    public Transform gunBarrel;
    public Transform aimLookAt;

    [Header("Time")]
    [SerializeField][Range(0, 1)] private float time = 1f;

    [Header("Clamp")]
    [SerializeField]private float clamp = 40f;

    private ChargerController charger = null;


    // Update is called once per frame
    void Update()
    {
        //fixClamp();
        if (Input.GetButtonDown("Fire1"))
            shoot(bb1);
        else if (Input.GetButtonDown("Fire2"))
            shoot(bb2);
        /*else if (Input.GetKeyDown(KeyCode.E))
        {
            shoot(bb3);
        }*/
    }

    //private void fixClamp(){
    //    Quaternion rotation = gameObject.transform.rotation;
    //    Vector3 eulerAngles = rotation.eulerAngles;

    //    if (eulerAngles.z > 180)
    //        eulerAngles.z -= 360;

    //    if (eulerAngles.z > clamp)
    //        eulerAngles.z = clamp;
    //    if (eulerAngles.z < -clamp)
    //        eulerAngles.z = -clamp;
    //    Quaternion newRotation = Quaternion.Euler(eulerAngles);
    //    transform.rotation = newRotation;
    //}

    private void FixedUpdate()
    {
        lookAtCenter();
        Time.timeScale = time;
    }
    private void lookAtCenter(){
        transform.LookAt(aimLookAt);
    }

    public void checkAndLoadCharger(){
        if (charger != null)
            charger.fullAuto();
        else
            Debug.Log("Sem carregador!");
    }

    private void shoot(GameObject bb)
    {
        //TODO: checar antes se o charger é nao nulo e se tem bala nele -> metodo charger.getCurrentCapacity()
        Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel); //LocalPosition seta muito errado
        //TODO: diminui bala do carregador com o metodo charger.consumeBB()
    }
}
