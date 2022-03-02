using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeOut : MonoBehaviour
{
    private CanvasGroup cg;
    private float fadeTime = 1f;
    private float delta = 0;
    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (delta < fadeTime)
        {
            delta += 0.01f*Time.deltaTime;
            cg.alpha -= delta;          
        }
        else
        {
            Destroy(gameObject);
        }
        transform.Translate(0, 0.5f * Time.deltaTime, 0);
    }
}
