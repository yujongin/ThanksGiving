using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{
    public string farmer_Name;      //��� �̸�
    public string family;           //��� �Ҽ� �йи�
    public int level;               //��� ����   
    public string faverite_Type;    //��� ��ȣ �۹� Ÿ��;
    public float full_stamina;      //��� �ִ� ü��
    public float stamina;           //��� ���� ü��
    public float harvest_Speed;     //��Ȯ �ӵ�
    public float atk;               //��� ���ݷ�

    private Field field;            //�� ����
    private FieldMaker FM;
    private GameObject target_Crop; // ���� ��ǥ �۹�
    private Crop crop;

    public bool lineCheck;

    public int x;  //��Ȯ�ؾ� �ϴ� �۹� �迭 x 
    private int y;  //����� y��ġ;

    public int cropNum;

    private bool arrived; //��ΰ� �۹� �տ� �����ߴ��� Ȯ���ϴ� ����
    private bool isRest;  //��ΰ� �����ִ� ������ Ȯ���ϴ� ����

    private float stage_level;
    // Start is called before the first frame update
    void Start()
    {
        field = GameObject.Find("Field").GetComponent<Field>();
        FM = GameObject.Find("FieldMaker").GetComponent<FieldMaker>();
        x = 0;
        y = (int)transform.position.y;
        cropNum = 0;
        arrived = false;
        lineCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ��ǥ �۹��� ������ �޾ƿ´�.
        if (target_Crop == null)
        {
            GetCrop();
        }
        if (!isRest)
        {
            Walk();
            if (arrived)
            {
                Harvest();
            }
        }

    }

    //��Ȯ �� �۹� ������ �ɾ�� �޼���
    private void Walk()
    {     
        //��ΰ� �۹� �տ� �������� �ʾ�����
        if (transform.position != target_Crop.transform.position - new Vector3(1f, 0, 0))
        {
            //�۹� �ձ��� �̵��Ѵ�.
            transform.position = Vector3.MoveTowards(transform.position, target_Crop.transform.position - new Vector3(1f, 0, 0), 0.05f);
        }
        //����������
        else
        {
            //�����ߴٰ� �˸�
            arrived = true;
        }
    }

    private float delta = 0;
    //�۹� ��Ȯ
    private void Harvest()
    {
        delta += Time.deltaTime;
        //���ݼӵ��� ����
        if (delta * harvest_Speed > 1) 
        {
            //�۹��� ü���� ��´�.
            crop.hp -= atk;
            delta = 0;
        }
        //�۹��� ü���� 0���� ���ų� ������
        if (crop.hp <= 0)
        {    
            //����� ���׹̳��� �۹� ���ݷ� ��ŭ ����
            stamina -= crop.atk;
            Rest();
            //���� �۹��� �޾ƿ��� ���� ���� �ʱ�ȭ
            cropNum++;
            if (cropNum > 9)
            {
                stage_level += 0.5f;
                cropNum = 0;
            }

            arrived = false;
            if (x >= 9)
            {
                x = 0;
            }
            else
            {
                x++;
            }
            target_Crop = null;
        }
    }

    private void Rest()
    {
        if (stamina <= 0)
        {
            isRest = true;
            StartCoroutine("Recovery");          
        }
    }

    IEnumerator Recovery()
    {
        yield return new WaitForSeconds(3f);
        stamina = full_stamina;
        isRest = false;
    }

    //��ǥ �۹� �޾ƿ��� �޼���
    private void GetCrop()
    {
        //1�� ������ ���� ù ��° �翡�� �޾ƿ���
        if (stage_level % 1==0)
        {
            target_Crop = field.firstField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
        //0.5�� ������ �� ��° �翡�� �޾ƿ���
        else
        {
            target_Crop = field.secondField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
    }
}