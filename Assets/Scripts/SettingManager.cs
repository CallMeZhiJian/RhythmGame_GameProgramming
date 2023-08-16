using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;
    [SerializeField] private AudioClip[] _musicClip;

    private GameObject SettingScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SettingScreen = GameObject.Find("SettingScreen");  
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            _musicSource.Stop();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void BackButton()
    {
        SettingScreen.GetComponent<Animator>().SetBool("isOpen", true);
    }
}
