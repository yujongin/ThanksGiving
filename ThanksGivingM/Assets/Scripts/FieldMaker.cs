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
    //작물 확률
    const int grain = 60;
    const int fruits = 30;
    const int vegetables = 10;

    public Field field;
    void Start()
    {
        field.firstField = new GameObject[WIDTH, HEIGHT];
        field.secondField = new GameObject[WIDTH, HEIGHT];
        fieldLine = new GameObject[HEIGHT];
        //시작위치 0부터 첫번째 밭 배열에 밭을 생성
        SetCrops(0,field.firstField);
    }

    void Update()
    {
        
    }

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
    public void SetCrops(int startPoint, GameObject[,] fieldArray)
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
                go.transform.position = new Vector3(i * HORIZONTAL_SPACING + j * DIAGONAL_SPACING + startPoint, j * VARTICAL_SPACING, j);
                go.transform.parent = cropsParent.transform;
                fieldArray[i,j] = go;
            }
        }
    } 
}
