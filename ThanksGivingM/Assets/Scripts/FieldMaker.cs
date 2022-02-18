using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMaker : MonoBehaviour
{
    //�۹� ����Ʈ
    public List<GameObject> crops;
    //���� ����
    const int WIDTH = 10;
    //���� ����
    const int HEIGHT = 5;
    //�� ���� �迭
    private GameObject[] fieldLine;

    //��� �迭
    private Crop[] currentCrops;

    //�۹� Ȯ��
    const int grain = 60;
    const int fruits = 30;
    const int vegetables = 10;

    public Field field;
    public FarmerController farmerController;

    int cropPosX;
    public int cropNums;

    bool checkField;

    public bool isCamera;
    void Start()
    {
        field.firstField = new GameObject[WIDTH, HEIGHT];
        field.secondField = new GameObject[WIDTH, HEIGHT];
        currentCrops = new Crop[5];
        fieldLine = new GameObject[HEIGHT];
        cropPosX = 0;
        cropNums = 0;
        //������ġ 0���� ù��° �� �迭�� ���� ����
        SetCrops(field.firstField);
        checkField = false;
        isCamera = false;
    }

    void Update()
    {
        if (cropNums>=5&&!checkField)
        {
            //�� ����
            if (GameManager.Instance.harvestLine == 1)
            {
                if (GameManager.Instance.stage_Level % 1 == 0)
                {
                    SetCrops(field.secondField);
                    cropNums = 0;
                }
                else
                {
                    SetCrops(field.firstField);
                    cropNums = 0;
                }
            }
            isCamera = true;
            checkField = true;
            //ī�޶� �̵����Ѷ�
        }
        else
        {
            Check_Harvested();
        }
    }

    //���� ��Ȯ���� ������ ��� ��Ȯ �Ǿ����� Ȯ��
    public void Check_Line()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            if (GameManager.Instance.stage_Level % 1 == 0)
            {
                currentCrops[i] = field.firstField[GameManager.Instance.harvestLine, i].GetComponent<Crop>();
            }
            else
            {
                currentCrops[i] = field.secondField[GameManager.Instance.harvestLine, i].GetComponent<Crop>();
            }          
        }
        for (int i = 0; i < currentCrops.Length; i++)
        {
            if (currentCrops[i].harvested)
            {
                cropNums++;
            }
        }

        if(cropNums != 5)
        {
            cropNums = 0;
        }
        else
        {
            if (GameManager.Instance.harvestLine >= 9)
            {
                GameManager.Instance.harvestLine = 0;
            }
            else
            {
                GameManager.Instance.harvestLine++;
            }
            checkField = false;
        }

    }

    //�������� ������ �ø��� ����
    void Check_Harvested()
    {
        int checkNum = 0;
        Crop crop;
        for(int i =0; i<WIDTH; i++)
        {
            for(int j =0; j < HEIGHT; j++)
            {
                if (GameManager.Instance.stage_Level % 1 == 0)
                {
                    crop = field.firstField[i, j].GetComponent<Crop>();
                }
                else
                {
                    crop = field.secondField[i, j].GetComponent<Crop>();
                }
                
                if (crop.harvested)
                {
                    checkNum++;
                }
            }
        }
        if (checkNum == HEIGHT * WIDTH)
        {
            checkField = false;
            GameManager.Instance.stage_Level += 0.5f;
            //Destroyfield();
        }
    }

/*    void Destroyfield()
    {
        for(int i = 0; i<WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if (GameManager.Instance.stage_Level % 1 == 0)
                {
                    Destroy(field.secondField[i, j]);
                }
                else
                {
                    Destroy(field.firstField[i, j]);
                }
            }
        }
    }*/

    //�۹� ���� �ʱ�ȭ �޼���
    void InitLine()
    { 
        //���� ���� ũ�� ��ŭ
        for (int i = 0; i < HEIGHT; i++)
        {
            //�۹� Ȯ��
            int cropPer = Random.Range(0, 100);
            //����� ���� Ȯ��
            if (cropPer < grain)
            {
                fieldLine[i] = crops[0];
            }
            //������ ���� Ȯ��
            else if (cropPer >= grain && cropPer < grain+fruits)
            {
                fieldLine[i] = crops[1];
            }
            //ä�Ұ� ���� Ȯ��
            else if (cropPer >= grain + fruits && cropPer < grain + fruits + vegetables)
            {
                fieldLine[i] = crops[2];
            }
        }
    }

   
    //�۹� �� ���� 
    private const float HORIZONTAL_SPACING = 2f;// ���� ����
    private const float VARTICAL_SPACING = 1.2f;//���� ����
    private const float DIAGONAL_SPACING = 0.5f;//�밢 ����

    //ó�� ������ �� �۹� ����
    public void SetCrops(GameObject[,] fieldArray)
    {
        //�۹��� �θ� ������Ʈ
        GameObject cropsParent = GameObject.Find("Field");
        //�ʱ� �� ũ�� ��ŭ
        for (int i = 0; i < WIDTH; i++) 
        {
            //�۹� �迭 �����ϰ� �����
            InitLine();
            //�۹��� �� ���ξ� ����
            for (int j =0; j< fieldLine.Length; j++)
            {
                //�迭�� �ִ� �۹��� �� ���� ��ġ
                GameObject go = Instantiate(fieldLine[j]);
                go.transform.position = new Vector3(i * HORIZONTAL_SPACING + j * DIAGONAL_SPACING + cropPosX, j * VARTICAL_SPACING, j);
                go.transform.parent = cropsParent.transform;
                fieldArray[i,j] = go;
            }
        }
        cropPosX += WIDTH*2;
    } 
}
