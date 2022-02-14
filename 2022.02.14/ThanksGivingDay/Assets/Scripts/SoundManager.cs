using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        GetComponent<AudioSource>();
        StartCoroutine(DelayMusicStart());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    
    }

    IEnumerator DelayMusicStart()
    {
        StartCoroutine(GameObject.Find("Generator").GetComponent<NoteGenerator>().NoteGenerate());
        yield return new WaitForSeconds(3);
        PlayTheMusic();
    }
    public void PlayTheMusic()
    {
        Debug.Log($"노래 시작: {Time.time}");
        audioSource.Play();
    }
}
