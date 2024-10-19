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
    [SerializeField][Range(0.001f, 0.1f)] private float backSpinDrag; // backspin da arma
    [SerializeField] public TYPE type; // tipo da arma
    [SerializeField] private ChargerController charger = null; // carregador da arma
    [SerializeField] float nextTimeForFire = 0f; // tempo para o proximo tiro
    [SerializeField] private float delayBetweenShots;  // Delay entre cada disparo em segundos
    [SerializeField] private bool canShoot = true; // booleano para saber se pode ou não atirar
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

    public float GetBackspindrag() { return backSpinDrag; }

    public void Fullauto()
    {
        fullauto = !fullauto;
    }

    public void StartCounter()
    {
        count = 0;
        numShots = 0;
        isContabilizando = true;
    }

    private void Start()
    {
        if (type == TYPE.SHOTGUN)
            delayBetweenShots = 0.8f;
        if (type == TYPE.GLOCK || type == TYPE.P1911)
            delayBetweenShots = 0.3f;
        if (type == TYPE.RIFLE)
            delayBetweenShots = 0.15f;
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

        if (!canShoot)
            nextTimeForFire += Time.deltaTime;

        if (nextTimeForFire >= delayBetweenShots)
        {
            canShoot = true;
            nextTimeForFire = 0f;
        }

        // Simula um disparo
        if (Input.GetButtonDown("Fire1") || fullauto)
        {
            if(canShoot)
                if (charger != null)
                    shoot(charger.bbPrefab);
        }

        chargerMesh.SetActive(charger != null);
        lookAtCenter();
    }

    public void resetHopUp()
    {
        backSpinDrag = 0.001f;
    }

    public void changeHopUp(float num)
    {
        if (num > 0)
            backSpinDrag = Mathf.Min(backSpinDrag + 0.001f, 0.1f);
        else
            backSpinDrag = Mathf.Max(backSpinDrag - 0.001f, 0.0001f);
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
            canShoot = false;
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
