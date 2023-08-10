using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource BGM;

    public bool startPlaying;

    public NoteScroller noteScrollerScript;

    public int currentScore;
    private int normalNote = 100;
    private int goodNote = 125;
    private int perfectNote = 150;

    public int currentMultiplier = 1;

    public int combo;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI comboText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                noteScrollerScript.hasStarted = true;

                BGM.Play();
            }
        }
    }

    public void NoteHit()
    {
        combo += 1;
        currentMultiplier = 1;

        if (combo >= 10)
        {
            currentMultiplier = 2;
            if(combo>= 20)
            {
                currentMultiplier = 3;
                if(combo >= 30)
                {
                    currentMultiplier = 4;
                }
            }
        }

        scoreText.text = currentScore.ToString();
        multiplierText.text = "x" + currentMultiplier;
        comboText.text = "Combo:\n" + combo;
    }

    public void MissedNote()
    {
        combo = 0;
        currentMultiplier = 1;

        multiplierText.text = "x" + currentMultiplier;
        comboText.text = "Combo:\n" + combo;
    }

    public void NormalHit()
    {
        currentScore += normalNote * currentMultiplier;
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += goodNote * currentMultiplier;
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += perfectNote * currentMultiplier;
        NoteHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        MissedNote();

    }
}
