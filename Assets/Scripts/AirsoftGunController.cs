using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("GameObjects")]
    public Transform slot;
    public Transform gunBarrel;
    public Transform aimLookAt;

    [Header("Time")]
    [SerializeField][Range(0, 1)] private float time = 1f;

    [Header("Clamp")]
    [SerializeField] private float clamp = 40f;

    [SerializeField] private ChargerController charger = null;

    // Update is called once per frame
    void Update()
    {
        //fixClamp();
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(charger);
            if(charger != null)
            {
                shoot(charger.bbPrefab);
                Transform playerUI = GameObject.Find("PlayerUI").transform;
                playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                            "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
            }
        }
    }

    public ChargerController GetCharger()
    {
        return charger;
    }

    public void SetCharger(ChargerController charger)
    {
        this.charger = charger;
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

    public void checkAndLoadCharger(Transform playerUI){
        if (charger != null)
        {
            charger.fullAuto();
            playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                                        "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
        }
        else
        {
            playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sem carregador";
            Debug.Log("Sem carregador!");
        }
    }

    private void shoot(GameObject bb)
    {
        if(charger != null)
        {
            if (charger.getCurrentBullets() > 0)
            {
                Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel); //LocalPosition seta muito errado
                charger.consumeBB();
            }
        }
    }
}
