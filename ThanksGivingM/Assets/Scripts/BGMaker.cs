using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMaker : MonoBehaviour
{
    bool oneTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < GameObject.Find("Main Camera").transform.position.x - 5f)
        {
            if (!oneTime)
            {
                GameObject go = Instantiate(gameObject);
                go.transform.position = transform.position + new Vector3(21.8f, 0, 0);
                go.name = "BG";
                oneTime = true;
            }
        }


        if (transform.position.x < GameManager.Instance.camera1.transform.position.x - 24.5f)
        {
            Destroy(gameObject);
        }
    }
}
