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
    //�۹� Ȯ��
    const int grain = 60;
    const int fruits = 30;
    const int vegetables = 10;

    public Field field;
    void Start()
    {
        field.firstField = new GameObject[WIDTH, HEIGHT];
        field.secondField = new GameObject[WIDTH, HEIGHT];
        fieldLine = new GameObject[HEIGHT];
        //������ġ 0���� ù��° �� �迭�� ���� ����
        SetCrops(0,field.firstField);
    }

    void Update()
    {
        
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
    public void SetCrops(int startPoint, GameObject[,] fieldArray)
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
                go.transform.position = new Vector3(i * HORIZONTAL_SPACING + j * DIAGONAL_SPACING + startPoint, j * VARTICAL_SPACING, j);
                go.transform.parent = cropsParent.transform;
                fieldArray[i,j] = go;
            }
        }
    } 
}
