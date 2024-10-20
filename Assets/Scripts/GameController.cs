using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isEndGame = false;
    [SerializeField] private AirsoftGunController[] guns;
    [SerializeField] private Material coberturaGuns;
    [SerializeField] private GameObject itens;

    private void Start()
    {
        Time.timeScale = 1;
        // inicializa todas as armas como desativadas (player começa com nada)
        for (int ii = 0; ii < guns.Length; ii++)
            guns[ii].gameObject.SetActive(false);

        // shader material guns
        itens = GameObject.Find("ITENS");
        Transform Guns = itens.transform.GetChild(0); // guns
        for (int jj = 0; jj < Guns.childCount; jj++)
        {
            Transform gun = Guns.GetChild(jj).GetChild(0);
            gun.GetComponent<MeshRenderer>().material = coberturaGuns;
            if (gun.childCount != 0)
            {
                for (int kk = 0; kk < gun.childCount; kk++)
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
    }

    public AirsoftGunController[] GetGuns()
    {
        return guns;
    }

    public GameObject GetItens()
    {
        return itens;
    }
    private void Update()
    {
        if (isEndGame)
            if (Input.GetKeyDown(KeyCode.I))
                ResetGame();
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void GameOver(Transform playerUI)
    {
        playerUI.GetChild(5).gameObject.SetActive(true);
        playerUI.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Game Over :(";
        Time.timeScale = 0;
        isEndGame = true;
    }

    public void Victory(Transform playerUI)
    {
        playerUI.GetChild(5).gameObject.SetActive(true);
        playerUI.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Congratulations :)";
        Time.timeScale = 0;
        isEndGame=true;
    }
}
