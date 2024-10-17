using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public enum TYPE{
    GLOCK,
    P1911,
    RIFLE,
    SHOTGUN
}

public class AirsoftGunController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField][Range(0f, 0.1f)] private float backSpinDrag;

    public TYPE type;
    float rpm = 600; // 600 rotações por minuto
    float nextTimeForFire = 0f;

    [Header("GameObjects")]
    public Transform slot;
    public Transform gunBarrel;
    public Transform aimLookAt;
    public GameObject chargerMesh;
    [SerializeField] bool fullauto = false;

    [Header("Time")]
    [SerializeField][Range(0, 1)] private float time = 1f;

    [SerializeField] private ChargerController charger = null;

    public bool getfullauto()
    {
        return fullauto;
    }

    public float GetBackspindrag()
    {
        return backSpinDrag;
    }

    public void setfullauto(bool value)
    {
        fullauto = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeForFire)
        {
            if(charger != null)
            {
                nextTimeForFire = Time.time + 60f / rpm; // calcula o delay em segundos baseado no rpm
                shoot(charger.bbPrefab);
            }
        }
        chargerMesh.SetActive(charger != null);
        lookAtCenter();
    }

    public void resetHopUp()
    {
        backSpinDrag = 0.01f;
    }

    public void changeHopUp(float num)
    {
        if (num > 0)
            backSpinDrag = Mathf.Min(backSpinDrag + 0.01f, 0.1f);
        else
            backSpinDrag = Mathf.Max(backSpinDrag - 0.01f, 0f);

        charger.bbPrefab.GetComponent<BBController>().backSpinDrag = backSpinDrag;
        Transform playerUI = transform.GetChild(2).GetChild(0);
        playerUI.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Hop-up: " + backSpinDrag;
    }

    public ChargerController GetCharger()
    {
        return charger;
    }

    public void SetCharger(ChargerController charger)
    {
        this.charger = charger;
    }

    private void FixedUpdate()
    {
        
        Time.timeScale = time;
    }
    private void lookAtCenter(){
        transform.LookAt(aimLookAt);
    }


    //PRA VERSAO DOIS
    public float rotationRange = 2f;
    private IEnumerator SineRotationLoop()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;

            float rotationX = Mathf.Sin(time) * rotationRange;
            float rotationY = Mathf.Sin(time) * rotationRange;
            float rotationZ = gameObject.transform.localRotation.eulerAngles.z;
            gameObject.transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

            yield return null;
        }
    }

    public void checkAndLoadCharger(Transform playerUI){
        if (charger != null)
        {
            charger.Reaload();
            playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                                        "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
        }
        else
            playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sem carregador";
    }

    private void shoot(GameObject bb)
    {
        if (charger.getCurrentBullets() > 0)
        {
            gunBarrel.GetComponent<AudioSource>().Play();
            if (type == TYPE.SHOTGUN)
                gunBarrel.parent.GetChild(6).GetComponent<AudioSource>().PlayDelayed(0.4f);

            Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel);
            charger.consumeBB();
            RectTransform playerUI = GameObject.Find("PlayerUI").GetComponent<RectTransform>();
            playerUI.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                                    "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
        }
        else
        {
            if (fullauto)
            {
                charger.Reaload();
                RectTransform playerUI = GameObject.Find("PlayerUI").GetComponent<RectTransform>();
                playerUI.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                                        "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
            }
        }
    }

}
