using UnityEngine;
using UnityEditor;
using Cinemachine;
using UnityEngine.UI;

public class LevelGenerator : EditorWindow
{
    GameObject _GameManager;
    Sprite BackgroundSprite;
    Sprite ActivatorSprite;

    private string ParentName = "Holder";
    GameObject ParentObject;

    float minBorderX = -3;
    float maxBorderX = 3;

    GameObject beatLine;

    GameObject NoteToCreate;
    
    AudioClip Clip;
    float BPM;
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

    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(Screen.height));

        GUILayout.Label("Game Scene Objects", EditorStyles.boldLabel);

        _GameManager = EditorGUILayout.ObjectField("Game Manager Prefab", _GameManager, typeof(GameObject), false) as GameObject;

        BackgroundSprite = EditorGUILayout.ObjectField("Background Image", BackgroundSprite, typeof(Sprite), false) as Sprite;

        ActivatorSprite = EditorGUILayout.ObjectField("Activator Image", ActivatorSprite, typeof(Sprite), false) as Sprite;


        if (GUILayout.Button("Generate Scene"))
        {
            CreateScene();
        }

        GUILayout.Label("Parent Object Information", EditorStyles.boldLabel);

        ParentObject = EditorGUILayout.ObjectField("Parent Object", ParentObject, typeof(GameObject), false) as GameObject;

        minBorderX = EditorGUILayout.FloatField("Min Border X", minBorderX);
        maxBorderX = EditorGUILayout.FloatField("Max Border X", maxBorderX);


        GUILayout.Space(25f);


        GUILayout.Label("Audio Samples Generator", EditorStyles.boldLabel);

        Clip = EditorGUILayout.ObjectField("Audio Clip", Clip, typeof(AudioClip), false) as AudioClip;
        BPM = EditorGUILayout.FloatField("BPM", BPM);
        AudioLength = EditorGUILayout.FloatField("Clip Length", AudioLength);

        if (GUILayout.Button("Genrate Audio Samples"))
        {
            GetSpectrumSamples();
        }


        GUILayout.Label("Creating Beat Lines", EditorStyles.boldLabel);

        beatLine = EditorGUILayout.ObjectField("BeatLine Object", beatLine, typeof(GameObject), false) as GameObject;

        if(GUILayout.Button("Generate Lines"))
        {
            GenerateBeatLine();
        }


        GUILayout.Label("Creating Notes", EditorStyles.boldLabel);

        NoteToCreate = EditorGUILayout.ObjectField("Note To Create", NoteToCreate, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Generate Note"))
        {
            GenerateNotes();
        }


        Interval = EditorGUILayout.FloatField("Distance Between Notes", Interval);
        NotePosY = EditorGUILayout.FloatField("Last Note Position Y", NotePosY);
        LinePosY = EditorGUILayout.FloatField("Last Line Position Y", LinePosY);

        if (GUILayout.Button("Reset"))
        {
            Reset();
        }

        EditorGUILayout.EndScrollView();
    }

    private void GenerateNotes()
    {
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
        if (AudioLength == 0)
        {
            Debug.Log("Audio Clip is not read");
            return;
        }

        NotePosY += Interval;

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = "Notes" + ParentName;

        for (int i = 0; i < AudioLength; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(minBorderX, maxBorderX), NotePosY, 0f);

            GameObject NewNote = Instantiate(NoteToCreate, SpawnPos, Quaternion.identity, NewParent.transform);

            NewNote.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            NotePosY += Interval;
        }
    }

    private void Reset()
    {
        NotePosY = resetVal;
        LinePosY = resetVal;
        AudioLength = resetVal;
        Interval = resetVal;
    }

    void GetSpectrumSamples()
    {
        if(Clip == null)
        {
            Debug.Log("No Audio Clip Given");
            return;
        }

        if(BPM == 0)
        {
            Debug.Log("BPM not filled");
            return;
        }

        AudioLength = Clip.length;

        Interval = BPM / 60f;
    }

    void GenerateBeatLine()
    {
        if(ParentObject == null)
        {
            Debug.Log("Parent Object is Empty");
            return;
        }
        if(beatLine == null)
        {
            Debug.Log("BeatLine Object is Empty");
            return;
        }
        if(AudioLength == 0)
        {
            Debug.Log("Audio Clip is not read");
            return;
        }

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = "Lines" + ParentName;

        for (int i = 0; i < AudioLength; i++)
        {
            Vector3 SpawnPos = new Vector3(0f, LinePosY, 0);

            GameObject NewLine = Instantiate(beatLine, SpawnPos, Quaternion.identity, NewParent.transform);

            if(i % 4 == 0)
            {
                NewLine.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 145);
            }
            
            LinePosY += Interval;
        }
    }

    void CreateScene()
    {
        if(BackgroundSprite == null)
        {
            Debug.Log("Background Image is Empty");
            return;
        }
        if(ActivatorSprite == null)
        {
            Debug.Log("Activator Image is Empty");
            return;
        }
        if(_GameManager == null)
        {
            Debug.Log("Game Manager Object is Empty");
        }

        //Creating GameManager
        Instantiate(_GameManager, _GameManager.transform.position, Quaternion.identity);

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

        _track.transform.localScale = new Vector3(7, 20, 1);

        SpriteRenderer _trackSR = _track.AddComponent<SpriteRenderer>();

        _trackSR.sprite = ActivatorSprite;
        _trackSR.color = new Color(0.254902f, 0.254902f, 0.254902f, 0.5882353f);
        _trackSR.sortingOrder = 0;

        //Creating MoveLine
        GameObject _moveLine = new GameObject("MoveLine");

        _moveLine.transform.localScale = new Vector3(_track.transform.localScale.x, 0.024f, 1);

        SpriteRenderer _moveLineSR = _moveLine.AddComponent<SpriteRenderer>();

        _moveLineSR.sprite = ActivatorSprite;
        _moveLineSR.sortingOrder = 1;

        //Creating Activator
        GameObject _activator = new GameObject("Activator");

        _activator.tag = "Player";

        SpriteRenderer _activatorSR = _activator.AddComponent<SpriteRenderer>();

        _activatorSR.sprite = ActivatorSprite;
        _activatorSR.sortingLayerName = "Notes";
        _activatorSR.sortingOrder = 1;

        _activator.AddComponent<BoxCollider2D>().isTrigger = true;

        _activator.AddComponent<PlayerController>();

    }
}
