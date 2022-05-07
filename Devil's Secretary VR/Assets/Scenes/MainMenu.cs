using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsScreen;
   public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void Credits()
    {
        creditsScreen.SetActive(true);
    }
    public void CreditsOff()
    {
        creditsScreen.SetActive(false);
    }
}
