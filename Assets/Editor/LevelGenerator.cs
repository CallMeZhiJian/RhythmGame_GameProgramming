using UnityEngine;
using UnityEditor;
public class LevelGenerator : EditorWindow
{
    string ParentName = "Holder";
    GameObject ParentObject;

    float minBorderX = -3;
    float maxBorderX = 3;

    GameObject beatLine;

    GameObject NoteToCreate;
    

    GameObject MusicSource;
    AudioSource _audioSource;
    float BPM;
    float AudioLength;

    float Distance = 0;
    float NotePosY = 0;
    float LinePosY = 0;

    private const float resetVal = 0f;
    Vector2 scrollPos = Vector2.zero;

    private float beatPerSec;

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGenerator));
    }

    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(1000));

        GUILayout.Label("Parent Object Information", EditorStyles.boldLabel);

        ParentName = EditorGUILayout.TextField("Parent Name", ParentName);
        ParentObject = EditorGUILayout.ObjectField("Parent Object", ParentObject, typeof(GameObject), false) as GameObject;

        minBorderX = EditorGUILayout.FloatField("Min Border X", minBorderX);
        maxBorderX = EditorGUILayout.FloatField("Max Border X", maxBorderX);


        GUILayout.Space(25f);


        GUILayout.Label("Audio Samples Generator", EditorStyles.boldLabel);

        MusicSource = EditorGUILayout.ObjectField("Music Source", MusicSource, typeof(GameObject), false) as GameObject;
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
            SpawnObject();
        }


        Distance = EditorGUILayout.FloatField("Distance Between Notes", Distance);
        NotePosY = EditorGUILayout.FloatField("Last Note Position Y", NotePosY);
        LinePosY = EditorGUILayout.FloatField("Last Line Position Y", LinePosY);

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

        NotePosY += Distance;

        GameObject NewParent = Instantiate(ParentObject, ParentObject.transform.position, Quaternion.identity);
        NewParent.name = "Notes" + ParentName;

        for (int i = 0; i < AudioLength; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(minBorderX, maxBorderX), NotePosY, 0f);

            GameObject NewNote = Instantiate(NoteToCreate, SpawnPos, Quaternion.identity, NewParent.transform);

            NewNote.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            NotePosY += Distance;
        }
    }

    private void Reset()
    {
        NotePosY = resetVal;
        LinePosY = resetVal;
        AudioLength = resetVal;
        Distance = resetVal;
    }

    void GetSpectrumSamples()
    {
        if(MusicSource == null)
        {
            Debug.Log("No Audio Source Given");
            return;
        }

        if(BPM == 0)
        {
            Debug.Log("BPM not filled");
            return;
        }

        _audioSource = MusicSource.GetComponent<AudioSource>();

        AudioLength = _audioSource.clip.length;

        Distance = BPM / 60f;
    }

    void GenerateBeatLine()
    {
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
            
            LinePosY += Distance;
        }
    }
}
