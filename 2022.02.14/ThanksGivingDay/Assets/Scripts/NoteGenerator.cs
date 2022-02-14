using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class NoteGenerator : MonoBehaviour
{
    public GameObject notePrefab;
    public NoteDate notedata;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        FromData();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SoundManager.instance.audioSource.time);
    }




    public void FromData()
    {
        string path = Application.dataPath + "/aa.json"; //�뷡 �������� ������ �ٲ���.
        string json = File.ReadAllText(path);
        notedata = JsonUtility.FromJson<NoteDate>(json);
    }

    public IEnumerator NoteGenerate()
    {
        Debug.Log($"��Ʈ ���� �޼ҵ� ����: {Time.time}");

        yield return new WaitForSeconds(3 - 1.697f + 0.8525623679161072f); //3�� �غ�Ⱓ - ��Ʈ ���� �� ������������ �Ÿ� - ù ��Ʈ�� ��������

        Debug.Log($"��Ʈ ���� ����: {Time.time}");

        for (int i=0; i < notedata.musicProcessivity.Count; i++)
        {
            Instantiate(notePrefab, new Vector2(10, 0), Quaternion.Euler(0, 0, 90));
            yield return new WaitForSeconds((float)(notedata.musicProcessivity[i+1] - notedata.musicProcessivity[i]));        
        }
       
    }

}
