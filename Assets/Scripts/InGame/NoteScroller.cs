using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NoteScroller : MonoBehaviour
{
    public float beatTempo;

    public bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60f;
    }

    void Update()
    {
        if (GameManager.isCounted)
        {
            if (!hasStarted)
            {
                hasStarted = true;
            }
            else
            {
                transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
            }
        }
    }
}
