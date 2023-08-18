using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    public AudioSource _gameMusicSource;
    public AudioSource BGMSource, _effectSource;
    public AudioSource _hoverMusicSource;

    public static AudioClip _currentClip;

    public static GameObject SettingScreen;
    public static GameObject ControlScreen;

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
        ControlScreen = GameObject.Find("ControlScreen");
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (HoverOverObject.onHover)
            {
                BGMSource.Pause();
            }
            else
            {
                BGMSource.UnPause();
            }
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (GameManager.onPause)
            {
                BGMSource.UnPause();
            }
            else
            {
                BGMSource.Pause();
            }

            _hoverMusicSource.Stop();

            _gameMusicSource = GameObject.Find("GameMusicSource").GetComponent<AudioSource>();

            if (_gameMusicSource != null)
            {
                _gameMusicSource.clip = _currentClip;
            }

            if (GameManager.instance.resultScreen.activeInHierarchy)
            {
                BGMSource.UnPause();
            }
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

    public void OnHoverMusic(AudioClip clip)
    {
        _hoverMusicSource.clip = clip;

        if (_hoverMusicSource.isPlaying)
        {
            _hoverMusicSource.Stop();
        }
        else
        {
            _hoverMusicSource.Play();
        }
    }

    public void OnControlScreen()
    {
        ControlScreen.GetComponent<Animator>().SetBool("ControlOpen", true);
    }

    public void BackButtonControlScreen()
    {
        ControlScreen.GetComponent<Animator>().SetBool("ControlOpen", false);
    }
}
