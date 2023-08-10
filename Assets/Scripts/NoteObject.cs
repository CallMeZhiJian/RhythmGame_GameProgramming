using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public GameObject EffectOnDestroy;
    public bool canBePressed;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
                }
                else if(Mathf.Abs(transform.position.y) > 0.05)
                {
                    GameManager.instance.GoodHit();
                }
                else
                {
                    GameManager.instance.PerfectHit();
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
