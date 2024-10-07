using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AirsoftGunController firstGun = null;
    [SerializeField] private AirsoftGunController currentGun = null;

    [SerializeField] private AirsoftGunController[] guns;

    // Start is called before the first frame update
    void Start()
    {
        selectGun(firstGun);
    }

    // Update is called once per frame
    void Update(){
        //CARREGAR
        if (Input.GetKeyDown(KeyCode.F))
            if (currentGun != null)
                currentGun.checkAndLoadCharger();
            else
                Debug.Log("Sem arma!");

        checkBox();
    }

    //private void OnTriggerStay(Collider other){
    //    if (other.gameObject.tag.Equals("Pistol1911"))
    //    {
    //        if(Input.GetKeyDown(KeyCode.E))
    //        {
    //            other.gameObject.transform.parent = transform;
    //            other.gameObject.transform.position = Vector3.zero;
    //        }
    //    }
    //    if (other.gameObject.tag.Equals("PistolGlock"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            other.gameObject.transform.parent = transform;
    //            other.gameObject.transform.position = Vector3.zero;
    //        }

    //    }
    //    if (other.gameObject.tag.Equals("AK47"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            other.gameObject.transform.parent = transform;
    //            other.gameObject.transform.position = Vector3.zero;
    //        }
    //    }
    //    if (other.gameObject.tag.Equals("Shotgun"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            other.gameObject.transform.parent = transform;
    //            other.gameObject.transform.position = Vector3.zero;
    //        }
    //    }
    //}

    Vector3 boxSize = new Vector3(1, 1.5f, 1);
    Vector3 direction = Vector3.forward;
    [SerializeField] private float distancia = 1f;

    //CHECA PROXIMIDADE COM ITENS
    private void checkBox(){
        RaycastHit hit;

        //PRESSIONA BOTAO DE COLETA
        if (Input.GetKeyDown(KeyCode.E)){

            //CHECA ARMAS
            if (Physics.BoxCast(transform.position, boxSize / 2, direction, out hit, transform.rotation, distancia)){
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Gun")){
                    AirsoftGunController gun = hit.collider.gameObject.GetComponent<AirsoftGunController>();

                    selectGun(gun);
                    Destroy(hit.collider.gameObject);
                }
            }

            //CHECA CARREGADORES
            //if (Physics.BoxCast(transform.position, boxSize / 2, direction, out hit, transform.rotation, distancia)){
            //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Chargers")){
            //        
            //    }
            //}
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + transform.rotation * (direction * (distancia / 2));

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }

    private void selectGun(AirsoftGunController gun){

        //Liga e desliga baseado na enum
        for (int i = 0; i < guns.Length; i++)
            guns[i].gameObject.SetActive(guns[i].type == gun.type);
        currentGun = gun;
        Debug.Log("Pegou "+ gun.type.ToString());

        //...
        //currentGun = ...
    }
}
