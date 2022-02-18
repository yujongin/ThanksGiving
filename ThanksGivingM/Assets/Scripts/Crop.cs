using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public string croptype; //작물 타입
    public int cropLevel;   //작물 레벨
    public Sprite deadCrop; //수확 된 작물 이미지
    public float atk;       //작물 공격력
    public float hp;        //작물 체력
    public int har;         //수확량


    private SpriteRenderer SR;
    private FieldMaker FM;
    public bool harvested;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        FM = GameObject.Find("FieldMaker").GetComponent<FieldMaker>();
        harvested = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !harvested) 
        {
            SR.sprite = deadCrop;
            harvested = true;
            FM.Check_Line();
        }
        if (transform.position.x < GameManager.Instance.camera1.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }
}
