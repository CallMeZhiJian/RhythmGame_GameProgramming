using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static GameObject LevelSelectionScreen;

    private void Start()
    {
        //SettingScreen = GameObject.Find("SettingScreen");
        LevelSelectionScreen = GameObject.Find("LevelSelectionScreen");
    }
    public void StartGame()
    {
        LevelSelectionScreen.GetComponent<Animator>().SetBool("openSelection", true);
        //SceneManager.LoadScene("GameScene");
    }

    public void OnSetting()
    {
        SettingManager.SettingScreen.GetComponent<Animator>().SetBool("isOpen", false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
