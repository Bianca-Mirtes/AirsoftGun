using System.Collections;
using System.Collections.Generic;
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
                currentGun.checkAndLoadCharger();
                /*Transform playerUI = transform.GetChild(2).GetChild(0);
                playerUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munição: " + currentGun.GetCharger().getCurrentBullets() +
                                                                            "/" + currentGun.GetCharger().GetCapacity() + " - " + currentGun.GetCharger().GetMassBB() + "g";
                playerUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentGun.type + "";*/
            }
            else
                Debug.Log("Sem arma!");

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

    //CHECA PROXIMIDADE COM ITENS
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
                    hit.gameObject.SetActive(false);
                }
            }

            //CHECA CARREGADORES
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

        //Liga e desliga baseado na enum
        for (int i = 0; i < guns.Length; i++) {
            guns[i].gameObject.SetActive(guns[i].type == gun.type);
            if(guns[i].type == gun.type)
                currentGun = guns[i];
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
