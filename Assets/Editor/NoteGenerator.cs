using UnityEngine;
using UnityEditor;
public class NoteGenerator : EditorWindow
{
    string ParentName;
    GameObject ParentObject;
    GameObject NoteToCreate;
    GameObject minBorderX;
    GameObject maxBorderX;

    GameObject MusicSource;
    float BPM;

    float Distance = 2f;

    private const float resetVal = 0f;
    float lastPosY = 0;

    [MenuItem("Tools/Note Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NoteGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Creating Notes", EditorStyles.boldLabel);

        ParentName = EditorGUILayout.TextField("Parent Name", ParentName);

        ParentObject = EditorGUILayout.ObjectField("Parent Object", ParentObject, typeof(GameObject), false) as GameObject;
        NoteToCreate = EditorGUILayout.ObjectField("Note To Create", NoteToCreate, typeof(GameObject), false) as GameObject;

        minBorderX = EditorGUILayout.ObjectField("Left Border", minBorderX, typeof(GameObject), false) as GameObject;
        maxBorderX = EditorGUILayout.ObjectField("Right Border", maxBorderX, typeof(GameObject), false) as GameObject;

        MusicSource = EditorGUILayout.ObjectField("Music Source", MusicSource, typeof(GameObject), false) as GameObject;
        BPM = EditorGUILayout.FloatField("BPM", BPM);
        
        Distance = EditorGUILayout.FloatField("Distance Between Notes", Distance);
        lastPosY = EditorGUILayout.FloatField("Last Note Position Y", lastPosY);

        if (GUILayout.Button("Generate Note"))
        {
            SpawnObject();
        }
        else if (GUILayout.Button("Reset"))
        {
            Reset();
        }
    }

    private void SpawnObject()
    {
        if(ParentName == string.Empty)
        {
            Debug.Log("Object has no name");
            return;
        }

        float beatPerSec = 60f / BPM;

        AudioSource _audioSource = MusicSource.GetComponent<AudioSource>();

        float _elapsedTime = _audioSource.timeSamples / (_audioSource.clip.frequency * beatPerSec);

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
}
