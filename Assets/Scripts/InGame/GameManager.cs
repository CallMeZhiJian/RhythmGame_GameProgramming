using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource MusicSource;

    public static bool startPlaying;

    public NoteScroller noteScrollerScript;

    [SerializeField] private GameObject startScene;
    [SerializeField] private GameObject pauseScreen;
    private GameObject SettingScreen;
    public static bool onPause = false;
    private bool isStart = false;

    [SerializeField] private GameObject Counter;
    public static bool isCounted = false;
    private bool counterStarted = false;

    private int currentScore;
    private int normalScore = 100;
    private int goodScore = 125;
    private int perfectScore = 150;
    private int currentMultiplier;
    private int combo;
    private int highestCombo;

    private int perfectHitCount;
    private int goodHitCount;
    private int normalHitCount;
    private int missedHitCount;

    private int totalNotes;
    public static float audioSpeed;

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

    [Header("Camera Setting")]
    public CinemachineVirtualCamera cineCam;
    [SerializeField] private float shakeTime;
    private float timer;

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
        SettingScreen = GameObject.Find("SettingScreen");

        StopShake();

        Time.timeScale = 1;
        startPlaying = false;
        onPause = false;
        isCounted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }

        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startScene.SetActive(false);
                isStart = true;
            }

            if (isStart)
            {
                if (isCounted)
                {
                    startPlaying = true;
                    noteScrollerScript.hasStarted = true;

                    MusicSource.Play();
                }
                else
                {
                    if (!counterStarted)
                    {
                        StartCoroutine(CountDown());
                    }
                }
            }
        }
        else
        {
            if (!MusicSource.isPlaying && !resultScreen.activeInHierarchy && FindObjectsOfType<NoteObject>().Length == 0)
            {
                StartCoroutine(SetTrueDelay());

                perfectHitCounterText.text = perfectHitCount.ToString();
                goodHitCounterText.text = goodHitCount.ToString();
                normalHitCounterText.text = normalHitCount.ToString();
                missedHitCounterText.text = missedHitCount.ToString();
                highestComboCounterText.text = highestCombo.ToString();
                finalScoreText.text = currentScore.ToString();

                float totalHit = perfectHitCount + goodHitCount + normalHitCount;
                float percentage = (totalHit / totalNotes) * 100f;

                string rank = "F";

                if (percentage >= 40)
                {
                    rank = "D";
                    rankText.color = Color.gray;
                    if (percentage >= 50)
                    {
                        rank = "C";
                        rankText.color = Color.green;
                        if (percentage >= 60)
                        {
                            rank = "B";
                            rankText.color = Color.blue;
                            if (percentage >= 80)
                            {
                                rank = "A";
                                rankText.color = Color.red;
                                if (percentage >= 95)
                                {
                                    rank = "S";
                                    rankText.color = Color.yellow;
                                }
                            }
                        }
                    }
                }

                rankText.text = rank;
            }
        }

        //switch (currentMultiplier)
        //{
        //    case 2:
        //        audioSpeed = 1.5f;
        //        break;
        //    case 3:
        //        audioSpeed = 2f;
        //        break;
        //    case 4:
        //        audioSpeed = 3f;
        //        break;
        //    default:
        //        audioSpeed = 1;
        //        break;
        //}

        //MusicSource.pitch = audioSpeed;
        //MusicSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / audioSpeed);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResumeGame();
        }
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

    IEnumerator SetTrueDelay()
    {
        yield return new WaitForSeconds(1f);
        resultScreen.SetActive(true);
    }

    public void ShakeCamera(float amplitute)
    {
        cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitute;
        timer = shakeTime;
    }

    public void StopShake()
    {
        cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        timer = 0f;
    }

    public void Replay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PauseResumeGame()
    {
        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            MusicSource.UnPause();
            onPause = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            MusicSource.Pause();
            onPause = true;
        }
    }

    public void OnSetting()
    {
        SettingScreen.GetComponent<Animator>().SetBool("isOpen", false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator CountDown()
    {
        Counter.SetActive(true);
        counterStarted = true;

        Counter.GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1);
        Counter.GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1);
        Counter.GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1);
        Counter.GetComponent<TextMeshProUGUI>().text = "START";
        yield return new WaitForSeconds(1f);

        Counter.SetActive(false);
        isCounted = true;
    }
}
