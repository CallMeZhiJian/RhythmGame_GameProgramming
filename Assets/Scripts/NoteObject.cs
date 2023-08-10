using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public GameObject EffectOnDestroy;
    public bool canBePressed;
    private SpriteRenderer sr;
    private ParticleSystem effects;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        effects = GetComponentInChildren<ParticleSystem>();

        var main = effects.main;
        main.startColor = sr.color;
    }

    void Update()
    {
        var main1 = EffectOnDestroy.GetComponent<ParticleSystem>().main;
        main1.startColor = sr.color;

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

                Instantiate(EffectOnDestroy, transform.position, Quaternion.identity);
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
