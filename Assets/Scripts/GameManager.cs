using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource MusicSource;
    //public AudioClip[] MusicClip;

    public bool startPlaying;

    public NoteScroller noteScrollerScript;

    private int currentScore;
    private int normalScore = 100;
    private int goodScore = 125;
    private int perfectScore = 150;
    public static int currentMultiplier;
    private int combo;
    private int highestCombo;

    private int perfectHitCount;
    private int goodHitCount;
    private int normalHitCount;
    private int missedHitCount;

    private int totalNotes;
    private float audioSpeed;

    [Header("In-Game text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI comboText;

    public TextMeshProUGUI perfectHitText;
    public TextMeshProUGUI goodHitText;
    public TextMeshProUGUI normalHitText;
    public TextMeshProUGUI missedText;

    [Header("Result Text")]
    public GameObject resultScreen;

    public TextMeshProUGUI perfectHitCounterText;
    public TextMeshProUGUI goodHitCounterText;
    public TextMeshProUGUI normalHitCounterText;
    public TextMeshProUGUI missedHitCounterText;
    public TextMeshProUGUI highestComboCounterText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI rankText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        currentScore = 1;
        currentMultiplier = 1;
        combo = 0;
        highestCombo = 0;

        perfectHitCount = 0;
        goodHitCount = 0;
        normalHitCount = 0;
        missedHitCount = 0;

        totalNotes = FindObjectsOfType<NoteObject>().Length;
        noteScrollerScript = FindObjectOfType<NoteScroller>();
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

                MusicSource.Play();
            }
        }
        else
        {
            if (!MusicSource.isPlaying && !resultScreen.activeInHierarchy)
            {
                resultScreen.SetActive(true);

                perfectHitCounterText.text = perfectHitCount.ToString();
                goodHitCounterText.text = goodHitCount.ToString();
                normalHitCounterText.text = normalHitCount.ToString();
                missedHitCounterText.text = missedHitCount.ToString();
                highestComboCounterText.text = highestCombo.ToString();
                finalScoreText.text = currentScore.ToString();

                float totalHit = perfectHitCount + goodHitCount + normalHitCount;
                float percentage = (totalHit / totalNotes) * 100f;

                string rank = "F";

                if(percentage >= 40)
                {
                    rank = "D";
                    if(percentage >= 50)
                    {
                        rank = "C";
                        if(percentage >= 60)
                        {
                            rank = "B";
                            if(percentage >= 80)
                            {
                                rank = "A";
                                if(percentage >= 95)
                                {
                                    rank = "S";
                                }
                            }
                        }
                    }
                }

                rankText.text = rank;
            }
        }

        switch (currentMultiplier)
        {
            case 2:
                Time.timeScale = 1.5f;
                audioSpeed = 1.5f;
                break;
            case 3:
                Time.timeScale = 2f;
                audioSpeed = 2f;
                break;
            case 4:
                Time.timeScale = 3f;
                audioSpeed = 3f;
                break;
            default:
                Time.timeScale = 1;
                audioSpeed = 1;
                break;
        }

        MusicSource.pitch = audioSpeed;
        //MusicSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / audioSpeed);
    }

    public void NoteHit()
    {
        combo += 1;
        currentMultiplier = 1;

        if (combo >= 10)
        {
            currentMultiplier = 2;
            if(combo>= 30)
            {
                currentMultiplier = 3;
                if(combo >= 50)
                {
                    currentMultiplier = 4;
                }
            }
        }

        if(combo > highestCombo)
        {
            highestCombo = combo;
        }

        scoreText.text = currentScore.ToString();
        multiplierText.text = "x" + currentMultiplier;
        comboText.text = "Combo:\n" + combo;

        perfectHitText.text = "Perfect Hit: " + perfectHitCount;
        goodHitText.text = "Good Hit: " + goodHitCount;
        normalHitText.text = "Normal Hit: " + normalHitCount;
    }

    public void MissedNote()
    {
        combo = 0;
        currentMultiplier = 1;
        missedHitCount++;

        multiplierText.text = "x" + currentMultiplier;
        comboText.text = "Combo:\n" + combo;
        missedText.text = "Missed: " + missedHitCount;
    }

    public void NormalHit()
    {
        currentScore += normalScore * currentMultiplier;
        normalHitCount++;

        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += goodScore * currentMultiplier;
        goodHitCount++;

        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += perfectScore * currentMultiplier;
        perfectHitCount++;

        NoteHit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        MissedNote();
    }
}
