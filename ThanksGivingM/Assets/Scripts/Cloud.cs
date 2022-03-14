using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    bool oneTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-0.2f * Time.deltaTime, 0, 0);
        if(transform.position.x < GameManager.Instance.camera1.transform.position.x-5)
        {
            if (!oneTime)
            {
                GameObject go = Instantiate(gameObject);
                go.transform.position = GameManager.Instance.camera1.transform.position + new Vector3(21.8f, 3.84f, 10f);
                go.name = "cloud";
                oneTime = true;
            }
        }

        if (transform.position.x < GameManager.Instance.camera1.transform.position.x - 24.5f)
        {
            Destroy(gameObject);
        }
    }
}
