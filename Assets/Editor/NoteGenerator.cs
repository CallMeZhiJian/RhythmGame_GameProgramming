using UnityEngine;
using UnityEditor;
public class NoteGenerator : EditorWindow
{
    GameObject beatLine;

    string ParentName;
    GameObject ParentObject;
    GameObject NoteToCreate;
    GameObject minBorderX;
    GameObject maxBorderX;

    GameObject MusicSource;
    float BPM;

    AudioSource _audioSource;

    float Distance = 2f;
    float lastPosY = 0;

    private const float resetVal = 0f;
    Vector2 scrollPos = Vector2.zero;

    private float beatPerSec;

    [MenuItem("Tools/Note Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NoteGenerator));
    }

    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(1000));

        GUILayout.Label("Creating Beat Lines", EditorStyles.boldLabel);

        beatLine = EditorGUILayout.ObjectField("BeatLine Object", beatLine, typeof(GameObject), false) as GameObject;

        if(GUILayout.Button("Generate Lines"))
        {
            GenerateBeatLine();
        }

        GUILayout.Label("Creating Notes", EditorStyles.boldLabel);

        ParentName = EditorGUILayout.TextField("Parent Name", ParentName);

        ParentObject = EditorGUILayout.ObjectField("Parent Object", ParentObject, typeof(GameObject), false) as GameObject;
        NoteToCreate = EditorGUILayout.ObjectField("Note To Create", NoteToCreate, typeof(GameObject), false) as GameObject;

        minBorderX = EditorGUILayout.ObjectField("Left Border", minBorderX, typeof(GameObject), false) as GameObject;
        maxBorderX = EditorGUILayout.ObjectField("Right Border", maxBorderX, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Generate Note"))
        {
            SpawnObject();
        }

        GUILayout.Label("Audio Samples Generator", EditorStyles.boldLabel);

        MusicSource = EditorGUILayout.ObjectField("Music Source", MusicSource, typeof(GameObject), false) as GameObject;
        BPM = EditorGUILayout.FloatField("BPM", BPM);

        if(GUILayout.Button("Genrate Audio Samples"))
        {
            GetSpectrumSamples();
        }

        Distance = EditorGUILayout.FloatField("Distance Between Notes", Distance);
        lastPosY = EditorGUILayout.FloatField("Last Note Position Y", lastPosY);

        if (GUILayout.Button("Reset"))
        {
            Reset();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SpawnObject()
    {
        if(ParentName == string.Empty)
        {
            Debug.Log("Object has no name");
            return;
        }
        if(_audioSource == null)
        {
            Debug.Log("Audio Samples not generated");
            return;
        }

        //float _elapsedTime = _audioSource.timeSamples / (_audioSource.clip.frequency * beatPerSec);

        lastPosY += Distance;

        float minX = minBorderX.transform.position.x;
        float maxX = maxBorderX.transform.position.x;

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = ParentName;

        for (int i = 0; i < _audioSource.clip.length; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(minX, maxX), lastPosY, 0f);

            GameObject NewNote = Instantiate(NoteToCreate, SpawnPos, Quaternion.identity, NewParent.transform);

            NewNote.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            lastPosY += Distance;
        }
    }

    private void Reset()
    {
        lastPosY = resetVal;
    }

    void GetSpectrumSamples()
    {
        if(MusicSource == null)
        {
            Debug.Log("No Audio Source Given");
            return;
        }

        _audioSource = MusicSource.GetComponent<AudioSource>();

        beatPerSec = 60f / BPM;
    }

    void GenerateBeatLine()
    {
        float yPos = 0;
        float audioLength = _audioSource.clip.length;

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = ParentName;

        while (audioLength != 0)
        {
            Vector3 SpawnPos = new Vector3(0f, yPos, 0);

            Instantiate(beatLine, SpawnPos, Quaternion.identity, NewParent.transform);

            audioLength -= beatPerSec;
            yPos += beatPerSec;
        }
    }
}
