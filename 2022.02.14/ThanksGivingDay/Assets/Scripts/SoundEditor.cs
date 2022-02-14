using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEditor : MonoBehaviour
{
    DataManager dataManager;
    AudioSource audioSource;
    public Slider slider;
    public Image image;

    public static double processivity;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        slider.maxValue = audioSource.clip.length;     
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        Debug.Log(audioSource.clip.length);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dataManager.notedata.musicProcessivity.Add(audioSource.time);
            image.color = Color.green;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) image.color = Color.white;




        slider.value = audioSource.time;



    }

    public void PlayMusic()
    {
        audioSource.Play();     
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void UnPauseMusic()
    {
        audioSource.UnPause();
    }
    public void CheckTime()
    {
        processivity = audioSource.time / audioSource.clip.length;

    }

}
