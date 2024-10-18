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
    [SerializeField][Range(0f, 0.1f)] private float backSpinDrag; // backspin da arma
    [SerializeField] public TYPE type; // tipo da arma
    [SerializeField] private ChargerController charger = null; // carregador da arma
    [SerializeField] float rpm = 600;   // 600 rotações por minuto
    [SerializeField] float nextTimeForFire = 0f; // tempo para o proximo tiro
    [SerializeField] public float delayBetweenShots = 0.2f;  // Delay entre cada disparo em segundos
    [SerializeField] bool fullauto = false; // booleano para controle da função full-auto

    [Header("Rate of Fire (ROF)")]
    [SerializeField] int numShots = 0;
    [SerializeField] float countTime= 60f;
    [SerializeField] float count = 0;
    [SerializeField] float rateOfFire;
    [SerializeField] public bool isContabilizando = false;

    [Header("GameObjects")]
    public Transform slot;
    public Transform gunBarrel;
    public Transform aimLookAt;
    public GameObject chargerMesh;

    [Header("Time")]
    [SerializeField][Range(0, 1)] private float time = 1f;

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

    public void StartCounter()
    {
        count = 0;
        numShots = 0;
        isContabilizando = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (count >= countTime)
        {
            // Calcula a cadência de disparos por minuto
            rateOfFire = numShots;
            Debug.Log("Cadência de tiro: " + rateOfFire + " BBs/m");

            isContabilizando = false;
        }
        count += Time.deltaTime;

        // Simula um disparo
        if (Input.GetButtonDown("Fire1"))
        {
            if (charger != null)
                    shoot(charger.bbPrefab);
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
        backSpinDrag = Mathf.Round(backSpinDrag * 100f) / 100f;

        charger.bbPrefab.GetComponent<BBController>().backSpinDrag = backSpinDrag;
        RectTransform playerUI = GameObject.Find("PlayerUI").GetComponent<RectTransform>();
        playerUI.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = "Hop-up: " + backSpinDrag;
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
                gunBarrel.parent.GetChild(6).GetComponent<AudioSource>().PlayDelayed(0.5f);

            Instantiate(bb, gunBarrel.position, Quaternion.identity, gunBarrel);
            charger.consumeBB();

            if (isContabilizando)
                numShots++;

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
