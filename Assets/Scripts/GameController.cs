using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isEndGame = false;
    private void Update()
    {
        if (isEndGame)
            if (Input.GetKeyDown(KeyCode.I))
                ResetGame();
    }

    public void ResetGame()
    {
        Time.timeScale = 1.0f;
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
