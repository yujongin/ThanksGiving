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
    private List<GameObject> currentCrops;

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

    public int harvestLine;

    void Start()
    {
        field.firstField = new GameObject[WIDTH, HEIGHT];
        field.secondField = new GameObject[WIDTH, HEIGHT];
        currentCrops = new List<GameObject>();
        fieldLine = new GameObject[HEIGHT];
        cropPosX = 0;
        harvestLine = 0;
        //������ġ 0���� ù��° �� �迭�� ���� ����
        SetCrops(field.firstField);
        checkField = true;
        isCamera = false;
    }

    void Update()
    {
        Check_Harvested();
        if (!checkField)
        {
            if (harvestLine == 1)
            {
                if (GameManager.Instance.stage_Level % 1 == 0)
                {
                    SetCrops(field.secondField);
                }
                else
                {
                    SetCrops(field.firstField);
                }
            }
            isCamera = true;
            checkField = true;
            //ī�޶� �̵����Ѷ�
        }
    }


    //�������� ������ �ø��� ����
    void Check_Harvested()
    {
        currentCrops.Clear();
        int checkNum = 0;
        Crop crop;
        for(int i =0; i<WIDTH; i++)
        {
            for(int j =0; j < HEIGHT; j++)
            {
                if (GameManager.Instance.stage_Level % 1 == 0)
                {
                    crop = field.firstField[i, j].GetComponent<Crop>();
                    if (i == harvestLine && crop.harvested)
                    {
                        currentCrops.Add(crop.gameObject);
                    }
                }
                else
                {
                    crop = field.secondField[i, j].GetComponent<Crop>();
                    if (i == harvestLine && crop.harvested)
                    {
                        currentCrops.Add(crop.gameObject);
                    }
                }
                
                if (crop.harvested)
                {
                    checkNum++;
                }

            }
        }
        if (currentCrops.Count == 5)
        {
            checkField = false;
            if (harvestLine < 9)
            {
                harvestLine++;
            }
            else
            {
                harvestLine = 0;
            }

        }
        if (checkNum == HEIGHT * WIDTH)
        {
            GameManager.Instance.stage_Level += 0.5f;
        }
    }


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
