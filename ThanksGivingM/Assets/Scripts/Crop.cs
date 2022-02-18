using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public string croptype; //�۹� Ÿ��
    public int cropLevel;   //�۹� ����
    public Sprite deadCrop; //��Ȯ �� �۹� �̹���
    public float atk;       //�۹� ���ݷ�
    public float hp;        //�۹� ü��
    public int har;         //��Ȯ��


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
