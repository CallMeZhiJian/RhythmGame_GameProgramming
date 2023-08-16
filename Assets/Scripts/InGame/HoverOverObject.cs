using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class HoverOverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator albumAnim;
    public static bool onHover;

    public GameObject ChooseMusicText;
    public GameObject HoverLockText;
    public GameObject ClickLockText;

    public AudioClip _musicClip;

    public GameObject _lock;
    public bool unlock;

    private void Start()
    {
        albumAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (unlock)
        {
            _lock.SetActive(false);
        }
        else
        {
            _lock.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        albumAnim.SetBool("isHovering", true);
        onHover = true;
        SettingManager.instance.OnHoverMusic(_musicClip);

        if (unlock)
        {
            ChooseMusicText.SetActive(true);
        }
        else
        {
            HoverLockText.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        albumAnim.SetBool("isHovering", false);
        onHover = false;
        SettingManager.instance.OnHoverMusic(_musicClip);
        if (unlock)
        {
            ChooseMusicText.SetActive(false);
        }
        else
        {
            HoverLockText.SetActive(false);
            ClickLockText.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlock)
        {
            SettingManager._currentClip = _musicClip;
            ChooseMusicText.SetActive(false);
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            ClickLockText.SetActive(true);
            return;
        }

        albumAnim.SetBool("isHovering", false);
    }
}
