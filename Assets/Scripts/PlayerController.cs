using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AirsoftGunController currentGun = null;
    [SerializeField] private AirsoftGunController[] guns;

    private void Start(){
        for (int i = 0; i < guns.Length; i++)
            guns[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        //CARREGAR
        if (Input.GetKeyDown(KeyCode.F))
            if (currentGun != null)
            {
                Transform playerUI = transform.GetChild(2).GetChild(0); // get the playerUI
                currentGun.checkAndLoadCharger(playerUI); 
                playerUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentGun.type + "";
            }
            else
            {
                Transform playerUI = transform.GetChild(2).GetChild(0);
                playerUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Sem Arma";
                playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sem carregador";
                Debug.Log("Sem arma!");
            }

        float scrollInput = Input.mouseScrollDelta.y;
        if (currentGun != null) { 
            if (scrollInput != 0) { 
                Debug.Log("Rolagem do scroll: " + scrollInput);
            }
        }
        checkBox();
    }

    Vector3 boxSize = new Vector3(1, 1.6f, 1.5f);
    [SerializeField] private float distancia = 1f;

    // CHECA PROXIMIDADE COM ITENS
    private void checkBox(){
        //PRESSIONA BOTAO DE COLETA
        if (Input.GetKeyDown(KeyCode.E)){

            //CHECA ARMAS
            //Vector3 boxPosition = transform.position + transform.forward * distancia;
            Vector3 boxPosition = transform.position + transform.TransformDirection(Vector3.forward) * distancia;
            Collider[] hitsGuns = Physics.OverlapBox(boxPosition, boxSize / 2, transform.rotation, LayerMask.GetMask("Gun"));
            foreach (Collider hit in hitsGuns){
                AirsoftGunController gun = hit.gameObject.GetComponent<AirsoftGunController>();

                if (gun != null){
                    selectGun(gun);
                }
            }

            // CHECA CARREGADORES
            Collider[] hitsChargers = Physics.OverlapBox(boxPosition, boxSize / 2, transform.rotation, LayerMask.GetMask("Charger"));
            foreach (Collider hit in hitsChargers){
                ChargerController charger = hit.gameObject.GetComponent<ChargerController>();

                if (charger != null){
                    bool selected = selectCharger(charger);
                    if (selected)
                        hit.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 boxPosition = transform.position + transform.TransformDirection(Vector3.forward) * distancia;
        Gizmos.matrix = Matrix4x4.TRS(boxPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
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
                    Debug.Log(currentGun.GetCharger().type.ToString());
                    if (chargers.GetChild(jj).GetComponent<ChargerController>().type.ToString() == currentGun.GetCharger().type.ToString())
                    {
                        chargers.GetChild(jj).GetComponent<ChargerController>().fullAuto(); //reseta ultimo carregador ao trocar de arma
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
}
