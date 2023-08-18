using UnityEngine;
using UnityEditor;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class LevelGenerator : EditorWindow
{
    GameObject _GameManager;
    Sprite BackgroundSprite;
    Sprite AlbumCover;
    Sprite ActivatorSprite;
    Sprite Square;

    string ParentName = "Holder";
    GameObject ParentObject;

    float minX = -3;
    float maxX = 3;

    GameObject beatLine;

    GameObject NoteToCreate;
    
    AudioClip Clip;
    public float BPM;
    float AudioLength;

    float Interval = 0;
    float NotePosY = 0;
    float LinePosY = 0;

    private const float resetVal = 0f;
    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGenerator));
    }

    public void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(Screen.height));

        GUILayout.Label("Audio Samples Generator", EditorStyles.boldLabel);

        Clip = EditorGUILayout.ObjectField("Audio Clip", Clip, typeof(AudioClip), false) as AudioClip;
        BPM = EditorGUILayout.FloatField("BPM", BPM);
        AudioLength = EditorGUILayout.FloatField("Clip Length", AudioLength);

        //Showing Values for BeatLine/Notes
        Interval = EditorGUILayout.FloatField("Interval Between Notes", Interval);
        NotePosY = EditorGUILayout.FloatField("Last Note Position Y", NotePosY);
        LinePosY = EditorGUILayout.FloatField("Last Line Position Y", LinePosY);

        if (GUILayout.Button("Generate Audio Samples (Clip Length & Interval)"))
        {
            GetAudioLength();
            SetInterval();
        }
        if(GUILayout.Button("Generate Clip Length"))
        {
            GetAudioLength();
        }
        if(GUILayout.Button("Calculate Interval value"))
        {
            SetInterval();
        }
        if (GUILayout.Button("Reset Values"))
        {
            Reset();
        }



        GUILayout.Space(25f);



        GUILayout.Label("Game Scene Objects", EditorStyles.boldLabel);

        //Propmt Game Manager Prefabs
        _GameManager = EditorGUILayout.ObjectField("Game Manager Prefab", _GameManager, typeof(GameObject), false) as GameObject;



        GUILayout.Space(25f);



        //Prompt Background Images
        BackgroundSprite = EditorGUILayout.ObjectField("Background Image", BackgroundSprite, typeof(Sprite), false) as Sprite;
        GUILayout.Label("*Resize the background yourself if it does not fit the scene.", EditorStyles.miniBoldLabel);

        GUILayout.Space(15f);

        //Prompt Album Cover Images
        AlbumCover = EditorGUILayout.ObjectField("Album Cover Image", AlbumCover, typeof(Sprite), false) as Sprite;

        GUILayout.Space(15f);

        //Prompt Activator Image (Square)
        ActivatorSprite = EditorGUILayout.ObjectField("Activator Image", ActivatorSprite, typeof(Sprite), false) as Sprite;
        GUILayout.Label("*Usually using Square Sprite, make sure to resize if using other image.", EditorStyles.miniBoldLabel);

        GUILayout.Space(15f);

        //Prompt Square Sprite
        Square = EditorGUILayout.ObjectField("Square Sprite", Square, typeof(Sprite), false) as Sprite;
        GUILayout.Label("*This section is just for creating of GameObject in scene.", EditorStyles.miniBoldLabel);



        GUILayout.Space(25f);



        //Prompt for Parent object of Notes/BeatLines
        GUILayout.Label("Parent Object Information", EditorStyles.boldLabel);

        //Prompt for Holder Prefabs
        ParentObject = EditorGUILayout.ObjectField("Holder Object", ParentObject, typeof(GameObject), false) as GameObject;

        //Prompt Range of Movement for Activator & Range of MusicNotes to Generate
        minX = EditorGUILayout.FloatField("Min Border X", minX);
        maxX = EditorGUILayout.FloatField("Max Border X", maxX);



        GUILayout.Space(25f);



        //Prompt BeatLine Prefabs
        GUILayout.Label("Beat Lines Information", EditorStyles.boldLabel);
        beatLine = EditorGUILayout.ObjectField("BeatLine Object", beatLine, typeof(GameObject), false) as GameObject;



        GUILayout.Space(25f);



        //Prompt MusicNote Prefabs
        GUILayout.Label("Music Notes Information", EditorStyles.boldLabel);
        NoteToCreate = EditorGUILayout.ObjectField("Note Object", NoteToCreate, typeof(GameObject), false) as GameObject;

        //Button to Access Method using the data from above
        if (GUILayout.Button("Generate Scene (Everything)"))
        {
            CreateScene();
            GenerateActivator();
            GenerateBeatLine();
            GenerateNotes();
        }

        if(GUILayout.Button("Generate Activator only"))
        {
            GenerateActivator();
        }

        if(GUILayout.Button("Generate Beat Lines only"))
        {
            GenerateBeatLine();
        }

        if(GUILayout.Button("Generate Music Notes only"))
        {
            GenerateNotes();
        }



        EditorGUILayout.EndScrollView();
    }

    void GetAudioLength()
    {
        if (Clip == null)
        {
            Debug.Log("No Audio Clip Given");
            return;
        }

        AudioLength = Clip.length;  
    }

    private void SetInterval()
    {
        if (BPM == 0)
        {
            Debug.Log("BPM not filled");
            return;
        }

        Interval = BPM / 60f;
    }

    private void GenerateNotes()
    {
        if (Interval == 0)
        {
            Debug.Log("Interval is 0");
            return;
        }
        if (AudioLength == 0)
        {
            Debug.Log("Audio Clip is not read");
            return;
        }
        if (ParentObject == null)
        {
            Debug.Log("Parent Object is Empty");
            return;
        }
        if(NoteToCreate == null)
        {
            Debug.Log("Note object is Empty");
            return;
        }
        

        NotePosY += Interval;

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = "Notes" + ParentName;

        NewParent.GetComponent<NoteScroller>().beatTempo = BPM;

        for (int i = 0; i < AudioLength; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(minX, maxX), NotePosY, 0f);

            GameObject NewNote = Instantiate(NoteToCreate, SpawnPos, Quaternion.identity, NewParent.transform);

            NewNote.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            NotePosY += Interval;
        }
    }

    void GenerateBeatLine()
    {
        if(Interval == 0)
        {
            Debug.Log("Interval is 0");
            return;
        }
        if (AudioLength == 0)
        {
            Debug.Log("Audio Clip is not read");
            return;
        }
        if (ParentObject == null)
        {
            Debug.Log("Parent Object is Empty");
            return;
        }
        if(beatLine == null)
        {
            Debug.Log("BeatLine Object is Empty");
            return;
        }


        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = "Lines" + ParentName;

        NewParent.GetComponent<NoteScroller>().beatTempo = BPM;

        for (int i = 0; i < AudioLength * 2; i++)
        {
            Vector3 SpawnPos = new Vector3(0f, LinePosY, 0);

            GameObject NewLine = Instantiate(beatLine, SpawnPos, Quaternion.identity, NewParent.transform);

            if(i % 4 == 0)
            {
                NewLine.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 145);
            }

            NewLine.transform.localScale = new Vector3(NewLine.transform.localScale.x + Mathf.Abs(maxX) + Mathf.Abs(minX), NewLine.transform.localScale.y, NewLine.transform.localScale.z);

            LinePosY += Interval;
        }
    }

    void CreateScene()
    {
        if (Interval == 0)
        {
            Debug.Log("Interval is 0");
            return;
        }
        if (BackgroundSprite == null)
        {
            Debug.Log("Background Image is Empty");
            return;
        }
        if(AlbumCover == null)
        {
            Debug.Log("Album Cover Image is Empty");
            return;
        }
        if (_GameManager == null)
        {
            Debug.Log("Game Manager Object is Empty");
        }

        //Creating Camera
        GameObject _mainCamera = Camera.main.gameObject;
        if(_mainCamera.GetComponent<CinemachineBrain>() == null)
        {
            _mainCamera.AddComponent<CinemachineBrain>();
        }

        CinemachineVirtualCamera CineCam = new GameObject("CineCam").AddComponent<CinemachineVirtualCamera>();
        CineCam.m_Lens.OrthographicSize = 5;
        CinemachineTransposer _transposer = CineCam.AddCinemachineComponent<CinemachineTransposer>();
        

        //Accessing NoiseSetting to set Noise Profile
        NoiseSettings _noiseSetting = EditorGUIUtility.Load("Assets/Editor/6D Shake.asset") as NoiseSettings;

        CineCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = _noiseSetting;

        //Creating Scene Background
        GameObject _background = new GameObject("Background");
        _background.transform.position = new Vector3(0, 4, 0);

        SpriteRenderer _backgroundSR = _background.AddComponent<SpriteRenderer>();
        _backgroundSR.sprite = BackgroundSprite;
        _backgroundSR.sortingOrder = -1;

        CineCam.m_Follow = _background.transform;
        CineCam.m_LookAt = _background.transform;

        //Creating Track
        GameObject _track = new GameObject("Track");

        _track.transform.position = _background.transform.position;
        _track.transform.localScale = new Vector3(_track.transform.localScale.x + Mathf.Abs(maxX) + Mathf.Abs(minX), 20, 1);

        SpriteRenderer _trackSR = _track.AddComponent<SpriteRenderer>();

        _trackSR.sprite = Square;
        _trackSR.color = new Color(0.254902f, 0.254902f, 0.254902f, 0.5882353f);
        _trackSR.sortingOrder = 0;

        //Creating MoveLine
        GameObject _moveLine = new GameObject("MoveLine");

        _moveLine.transform.localScale = new Vector3(_track.transform.localScale.x, 0.1f, 1);

        SpriteRenderer _moveLineSR = _moveLine.AddComponent<SpriteRenderer>();

        _moveLineSR.sprite = Square;
        _moveLineSR.sortingOrder = 1;

        //Creating GameManager
        Instantiate(_GameManager, _GameManager.transform.position, Quaternion.identity);

        //Inserting Audio Clip
        AudioSource _gameMusciSource = GameObject.Find("GameMusicSource").GetComponent<AudioSource>();
        _gameMusciSource.clip = Clip;

        //Changing Song Name
        TextMeshProUGUI _songName = GameObject.Find("SongName").GetComponent<TextMeshProUGUI>();
        _songName.text = Clip.name;

        //Changing Album Cover
        Image _albumCover = GameObject.Find("AlbumCover").GetComponent<Image>();
        _albumCover.sprite = AlbumCover;
    }

    void GenerateActivator()
    {
        if (ActivatorSprite == null)
        {
            Debug.Log("Activator Image is Empty");
            return;
        }

        //Creating Activator
        GameObject _activator = new GameObject("Activator");

        _activator.tag = "Player";

        SpriteRenderer _activatorSR = _activator.AddComponent<SpriteRenderer>();

        _activatorSR.sprite = ActivatorSprite;
        _activatorSR.sortingLayerName = "Notes";
        _activatorSR.sortingOrder = 1;

        _activator.AddComponent<BoxCollider2D>().isTrigger = true;

        PlayerController _playerController =_activator.AddComponent<PlayerController>();
        _playerController.minBorderX = minX;
        _playerController.maxBorderX = maxX;
    }

    private void Reset()
    {
        NotePosY = resetVal;
        LinePosY = resetVal;
        Interval = resetVal;
        AudioLength = resetVal;
        BPM = resetVal;
    }
}
