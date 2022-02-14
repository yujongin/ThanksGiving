using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NoteDate
{
    public string musicName; //�뷡 ����
    public int bpm;
    public List<double> musicProcessivity; //��Ʈ�� ������ ���� �뷡 ����ð�  
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
