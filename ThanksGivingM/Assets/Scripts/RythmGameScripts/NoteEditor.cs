using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteEditor : MonoBehaviour
{
    public GameObject notePrefab;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate(notePrefab, canvas.transform);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300,320);      
            go.name = "Note";      
        }
    }
}
