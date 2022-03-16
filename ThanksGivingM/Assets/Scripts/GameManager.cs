using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float stage_Level;

    public float tension; //텐션게이지
    public float maxTension = 100; //텐션게이지 최대치

    public float exp; //현재 경험치


    public Crop crop;
    
    public FieldMaker FM;

    public GameObject camera1;
    public Vector3 targetPos;

    public Text current_harvest;
    public float total_yield;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tension = maxTension;
        stage_Level = 1.0f;
        FM = GameObject.Find("FieldMaker").GetComponent<FieldMaker>();
    }

    // Update is called once per frame
    void Update()
    {




        if (FM.isCamera)
        {
            if (camera1.transform.position == targetPos)
            {
                FM.isCamera = false;
            }
            camera1.transform.position = Vector3.MoveTowards(camera1.transform.position, targetPos, 0.05f);
        }
        else
        {
            targetPos = camera1.transform.position + new Vector3(2f, 0, 0);
        }

        current_harvest.text = total_yield.ToString();
    }

}
