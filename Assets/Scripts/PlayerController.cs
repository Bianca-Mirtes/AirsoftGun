using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class PlayerController : MonoBehaviour
{
    private int health = 200;
    [SerializeField] public AirsoftGunController currentGun = null;
    [SerializeField] private AirsoftGunController[] guns;
    [SerializeField] private Material coberturaGuns;
    [SerializeField] private Transform slots;
    private Transform playerUI;

    [SerializeField] private Sprite healthBox;
    [SerializeField] private Sprite AmmoBox;
    private bool canReaload = true;

    private void Start(){
        slots = GameObject.FindWithTag("Slots").transform;
        playerUI = transform.GetChild(2).GetChild(0);

        // inicializa todas as armas como desativadas (player começa com nada)
        for (int ii = 0; ii < guns.Length; ii++)
            guns[ii].gameObject.SetActive(false);

        // shader material guns
        GameObject itens = GameObject.Find("ITENS");
        Transform Guns = itens.transform.GetChild(0); // guns
        for (int jj = 0; jj < Guns.childCount; jj++)
        {
            Transform gun = Guns.GetChild(jj).GetChild(0);
            gun.GetComponent<MeshRenderer>().material = coberturaGuns;
            if (gun.childCount != 0)
            {
                for (int kk=0; kk < gun.childCount; kk++)
                    gun.GetChild(kk).GetComponent<MeshRenderer>().material = coberturaGuns;
            }
        }
        // shader material chargers
        Transform chargers = itens.transform.GetChild(1); // Chargers
        for (int jj = 0; jj < Guns.childCount; jj++)
        {
            Transform charger = chargers.GetChild(jj).GetChild(0);
            charger.GetComponent<MeshRenderer>().material = coberturaGuns;
            if (charger.childCount != 0)
            {
                for (int kk = 0; kk < charger.childCount; kk++)
                    charger.GetChild(kk).GetComponent<MeshRenderer>().material = coberturaGuns;
            }
        }

        // player inicia sem nada
        playerUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sem Arma";
        playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sem carregador";
    }

    // Update is called once per frame
    void Update() {
        if (currentGun != null && currentGun.GetCharger() != null && !currentGun.isContabilizando)
            currentGun.StartCounter();
        
        if(health <= 0)
        {
            gameObject.GetComponent<FirstPersonController>().cameraEnable = false;
            FindObjectOfType<GameController>().GameOver(playerUI);
            return;
        }

        // ATIVAR E DESATIVAR FULL AUTO
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(currentGun != null)
            {
                playerUI.GetChild(7).GetComponent<Toggle>().isOn = !currentGun.getfullauto();
                currentGun.Fullauto();
            }
        }

        // CARREGAR ARMA
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentGun != null)
                currentGun.checkAndLoadCharger(playerUI);
        }

        // USAR KIT MÉDICO
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int ii = 0; ii < slots.childCount; ii++)
            {
                Image slot = slots.GetChild(ii).GetComponent<Image>();
                if (slot.sprite != null)
                {
                    if(slot.sprite.name == "HealthBox")
                    {
                        if (health < 200)
                        {
                            UseHealthBox();
                            slot.sprite = null;
                            break;
                        }
                    }
                }
            }
        }

        float scrollInput = Input.mouseScrollDelta.y;
        if (currentGun != null) {
            if (scrollInput != 0)
                currentGun.changeHopUp(scrollInput);
        }
        checkBox();
    }

    private void UseHealthBox()
    {
        health += 25;
        if (health > 200)
            health = 200;
        playerUI.GetChild(4).GetComponent<Slider>().value = health;
    }

    Vector3 boxSize = new Vector3(1, 1.6f, 1.5f);
    [SerializeField] private float distancia = 1f;

    // CHECA PROXIMIDADE COM ITENS
    private void checkBox(){
        //PRESSIONA BOTAO DE COLETA
        if (Input.GetKeyDown(KeyCode.E)){

            //CHECA ARMAS
            Vector3 boxPosition = transform.position + transform.TransformDirection(Vector3.forward) * distancia;
            Collider[] hitsGuns = Physics.OverlapBox(boxPosition, boxSize, transform.rotation, LayerMask.GetMask("Gun"));
            foreach (Collider hit in hitsGuns){
                AirsoftGunController gun = hit.gameObject.GetComponent<AirsoftGunController>();

                if (gun != null){
                    selectGun(gun);
                    playerUI.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Hop-up: " + currentGun.GetBackspindrag();
                    playerUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentGun.type + "";
                }
            }

            // CHECA CARREGADORES
            Collider[] hitsChargers = Physics.OverlapBox(boxPosition, boxSize, transform.rotation, LayerMask.GetMask("Charger"));
            foreach (Collider hit in hitsChargers){
                ChargerController charger = hit.gameObject.GetComponent<ChargerController>();

                if (charger != null){
                    bool selected = selectCharger(charger);
                    if (selected) {
                        hit.gameObject.SetActive(false);
                        currentGun.resetHopUp();
                        playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + charger.getCurrentBullets() +
                                                            "/" + charger.GetCapacity() + " - " + charger.GetMassBB() + "g";
                    }
                }
            }

            Collider[] hitsAmmoBox = Physics.OverlapBox(boxPosition, boxSize, transform.rotation, LayerMask.GetMask("AmmoBox"));
            foreach (Collider hit in hitsAmmoBox)
            {
                for(int ii=0; ii < slots.childCount; ii++)
                {
                    Image slot = slots.GetChild(ii).GetComponent<Image>();
                    if (slot.sprite == null)
                    {
                        slot.sprite = AmmoBox;
                        Destroy(hit.gameObject);
                        break;
                    }
                }
            }

            Collider[] hitsHealthBox = Physics.OverlapBox(boxPosition, boxSize, transform.rotation, LayerMask.GetMask("HealthBox"));
            foreach (Collider hit in hitsHealthBox)
            {
                for (int ii = 0; ii < slots.childCount; ii++)
                {
                    Image slot = slots.GetChild(ii).GetComponent<Image>();
                    if (slot.sprite == null)
                    {
                        slot.sprite = healthBox;
                        Destroy(hit.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void selectGun(AirsoftGunController gun){
        //Evita pegar a atual
        if (currentGun != null && currentGun.type == gun.type)
            return;

        gun.gameObject.SetActive(false);

        // Reativa a arma atual que foi trocada por outra
        if (currentGun != null)
        {
            GameObject itens = GameObject.Find("ITENS");
            Transform guns = itens.transform.GetChild(0); // guns
            for (int jj = 0; jj < guns.childCount; jj++)
            {
                if (guns.GetChild(jj).gameObject.tag == currentGun.type.ToString())
                {
                    guns.GetChild(jj).GetComponent<AirsoftGunController>().SetCharger(null);
                    guns.GetChild(jj).gameObject.SetActive(true);
                    break;
                }
            }
            // Reativa o CARREGADOR atual que foi trocado por outro
            if (currentGun.GetCharger() != null)
            {
                Transform chargers = itens.transform.GetChild(1); // chargers
                for (int jj = 0; jj < chargers.childCount; jj++)
                {
                    if (chargers.GetChild(jj).GetComponent<ChargerController>().type.ToString() == currentGun.GetCharger().type.ToString())
                    {
                        chargers.GetChild(jj).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        //Liga e desliga baseado na enum
        for (int ii = 0; ii < guns.Length; ii++) {
            guns[ii].gameObject.SetActive(guns[ii].type == gun.type);
            if(guns[ii].type == gun.type)
                currentGun = guns[ii];
        }

        Debug.Log("Pegou "+ gun.type.ToString());
    }

    private bool selectCharger(ChargerController charger){
        //Liga e desliga baseado na enum
        if(currentGun != null){
            if(currentGun.type == charger.type){
                currentGun.SetCharger(charger);
                Debug.Log("Novo carregador");
                return true;
            }else
                Debug.Log("Carregador incompatível");
        }
        else
            Debug.Log("Sem arma");

        return false;
    }

    public AirsoftGunController GetCurrentGun() { return currentGun; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("BB"))
        {
            health -= 10;
            Slider slider = playerUI.GetChild(4).GetComponent<Slider>();
            slider.value -= 10;
            Debug.Log("Acertou o player");
            Destroy(other.gameObject, 2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Victory"))
            FindObjectOfType<GameController>().Victory(playerUI);
    }
}
