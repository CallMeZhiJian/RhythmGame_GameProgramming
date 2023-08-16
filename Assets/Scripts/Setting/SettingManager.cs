using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    public AudioSource _gameMusicSource;
    [SerializeField] private AudioSource BGMSource, _effectSource;
    public AudioSource _hoverMusicSource;

    public static AudioClip _currentClip;

    public static GameObject SettingScreen;

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

        if (!BGMSource.isPlaying)
        {
            BGMSource.Play();
        }
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
        if (SettingScreen.activeInHierarchy)
        {
            SettingScreen.GetComponent<Animator>().SetBool("isOpen", true);
        }

        if (MainMenu.LevelSelectionScreen.activeInHierarchy)
        {
            MainMenu.LevelSelectionScreen.GetComponent<Animator>().SetBool("openSelection", false);
        }
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
}
