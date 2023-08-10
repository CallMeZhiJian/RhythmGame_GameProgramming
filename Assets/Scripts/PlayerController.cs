using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 mousePos;
    
    public Transform minBorderX;
    public Transform maxBorderX;

    private SpriteRenderer playerSR;

    public KeyCode KeyBinding;
    public static bool isPressed;

    private void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Button following the mouse cursor in X position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, minBorderX.position.x, maxBorderX.position.x);
        transform.position = new Vector3(mousePos.x, transform.position.y, 0f);

        //Button pressed
        if (Input.GetKeyDown(KeyBinding))
        {
            playerSR.color = Color.gray;
            isPressed = true;
        }

        if (Input.GetKeyUp(KeyBinding))
        {
            playerSR.color = Color.white;
            isPressed = false;
        }
    }
}
