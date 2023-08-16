using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject SettingScreen;

    private void Start()
    {
        SettingScreen = GameObject.Find("SettingScreen");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSetting()
    {
        SettingScreen.GetComponent<Animator>().SetBool("isOpen", false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
