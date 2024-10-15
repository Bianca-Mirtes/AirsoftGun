using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameOver(Transform playerUI)
    {
        playerUI.GetChild(5).gameObject.SetActive(true);
        playerUI.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Game Over :(";
        Time.timeScale = 0;
    }

    public void Victory(Transform playerUI)
    {
        playerUI.GetChild(5).gameObject.SetActive(true);
        playerUI.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Congratulations :)";
        Time.timeScale = 0;
    }
}
