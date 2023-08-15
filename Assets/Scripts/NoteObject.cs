using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NoteObject : MonoBehaviour
{
    public GameObject EffectOnDestroy;
    public bool canBePressed;
    private SpriteRenderer sr;
    private Light2D L2D;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        L2D = GetComponentInChildren<Light2D>();
        L2D.color = sr.color;
    }

    void Update()
    {
        var main = EffectOnDestroy.GetComponent<ParticleSystem>().main;
        main.startColor = sr.color;

        if (PlayerController.isPressed)
        {
            if (canBePressed)
            {
                if(Mathf.Abs(transform.position.y) > 0.25)
                {
                    GameManager.instance.NormalHit();
                    GameManager.instance.ShakeCamera(0.5f);
                }
                else if(Mathf.Abs(transform.position.y) > 0.05)
                {
                    GameManager.instance.GoodHit();
                    GameManager.instance.ShakeCamera(1f);
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    GameManager.instance.ShakeCamera(2f);
                }

                GameObject effects = Instantiate(EffectOnDestroy, transform.position, Quaternion.identity);
                Destroy(effects, 1f);
                Destroy(gameObject);
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePressed = false;
        }
    }
}
