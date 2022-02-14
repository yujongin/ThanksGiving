using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NoteDate
{
    public string musicName; //노래 제목
    public int bpm;
    public List<double> musicProcessivity; //노트가 찍혔을 때의 노래 진행시간  
}

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NoteDate notedata;


    [ContextMenu("To NoteData")]
    public void ToNoteData()
    {
        string json = JsonUtility.ToJson(notedata, true);
        string path = Application.dataPath + $"/{notedata.musicName}.json";
        File.WriteAllText(path, json);
    }


    [ContextMenu("From NoteData")]
    public void FromData()
    {
        string path = Application.dataPath + "/JsonData.json";
        string json = File.ReadAllText(path);
        notedata = JsonUtility.FromJson<NoteDate>(json);
    }

    public void ReturnName(string name)
    {
        notedata.musicName = name;       
    }
    public void ReturnBPM(string bpm)
    {
        notedata.bpm = int.Parse(bpm);
    }
}
