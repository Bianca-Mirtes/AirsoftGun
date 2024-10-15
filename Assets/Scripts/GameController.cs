using System.Collections;
using System.Collections.Generic;
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
        Time.timeScale = 0;
    }
}
