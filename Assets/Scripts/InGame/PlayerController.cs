using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 mousePos;
    
    public float minBorderX = -3;
    public float maxBorderX = 3;

    private SpriteRenderer playerSR;
    private Color defaultColor;
    private Color pressedColor;

    public KeyCode KeyBinding = KeyCode.Space;
    public static bool isPressed;

    private void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
        defaultColor = playerSR.color;
        pressedColor = Color.black;
    }

    void Update()
    {
        if (!GameManager.onPause && GameManager.startPlaying)
        {
            //Button following the mouse cursor in X position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, minBorderX, maxBorderX);
            transform.position = new Vector3(mousePos.x, transform.position.y, 0f);

            //Button pressed
            if (Input.GetKeyDown(KeyBinding))
            {
                StartCoroutine(PressDelay());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pressedColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
    }

    IEnumerator PressDelay()
    {
        playerSR.color = pressedColor;
        isPressed = true;
        yield return new WaitForSeconds(0.1f);
        playerSR.color = defaultColor;
        isPressed = false;
    }
}
