using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMaker : MonoBehaviour
{
    //작물 리스트
    public List<GameObject> crops;
    //밭의 가로
    const int WIDTH = 10;
    //밭의 세로
    const int HEIGHT = 5;
    //밭 라인 배열
    private GameObject[] fieldLine;

    //농부 배열
    private Crop[] currentCrops;

    //작물 확률
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
        //시작위치 0부터 첫번째 밭 배열에 밭을 생성
        SetCrops(field.firstField);
        checkField = false;
        isCamera = false;
    }

    void Update()
    {
        if (cropNums>=5&&!checkField)
        {
            //밭 생성
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
            //카메라 이동시켜라
        }
        else
        {
            Check_Harvested();
        }
    }

    //현재 수확중인 라인이 모두 수확 되었는지 확인
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

    //스테이지 레벨을 올리기 위해
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

    //작물 라인 초기화 메서드
    void InitLine()
    { 
        //밭의 세로 크기 만큼
        for (int i = 0; i < HEIGHT; i++)
        {
            //작물 확률
            int cropPer = Random.Range(0, 100);
            //곡식이 나올 확률
            if (cropPer < grain)
            {
                fieldLine[i] = crops[0];
            }
            //과일이 나올 확률
            else if (cropPer >= grain && cropPer < grain+fruits)
            {
                fieldLine[i] = crops[1];
            }
            //채소가 나올 확률
            else if (cropPer >= grain + fruits && cropPer < grain + fruits + vegetables)
            {
                fieldLine[i] = crops[2];
            }
        }
    }

   
    //작물 간 간격 
    private const float HORIZONTAL_SPACING = 2f;// 가로 간격
    private const float VARTICAL_SPACING = 1.2f;//세로 간격
    private const float DIAGONAL_SPACING = 0.5f;//대각 간격

    //처음 시작할 때 작물 세팅
    public void SetCrops(GameObject[,] fieldArray)
    {
        //작물의 부모 오브젝트
        GameObject cropsParent = GameObject.Find("Field");
        //초기 밭 크기 만큼
        for (int i = 0; i < WIDTH; i++) 
        {
            //작물 배열 랜덤하게 재생성
            InitLine();
            //작물을 한 라인씩 생성
            for (int j =0; j< fieldLine.Length; j++)
            {
                //배열에 있는 작물을 한 개씩 배치
                GameObject go = Instantiate(fieldLine[j]);
                go.transform.position = new Vector3(i * HORIZONTAL_SPACING + j * DIAGONAL_SPACING + cropPosX, j * VARTICAL_SPACING, j);
                go.transform.parent = cropsParent.transform;
                fieldArray[i,j] = go;
            }
        }
        cropPosX += WIDTH*2;
    } 
}
