using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float stage_Level;
    public float player_Level;


    public float tension; //�ټǰ�����
    public float maxTension = 100; //�ټǰ����� �ִ�ġ


    #region ����ġ ���� ����
    public float exp { get; private set;} //���� ����ġ. ���ӸŴ��� ���� ��ġ�� ������ �� �ִ�.

    public float requiredexp { get; private set;} //�ʿ� ����ġ. ���ӸŴ��� ���� ��ġ�� ������ �� �ִ�.
    private float additionalRequiredExp = 30; //���� ��¿� ���� �߰� �ʿ����ġ ��.
    private const float defaultRequiredExp = 50; //�ʿ� ����ġ �⺻ ��.
    #endregion


    public Crop crop;
    
    public FieldMaker FM;

    public GameObject augSelect;
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
        player_Level = 1;
        tension = maxTension;
        stage_Level = 1.0f;
        FM = GameObject.Find("FieldMaker").GetComponent<FieldMaker>();
        SetRequiredEXP();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //�׽�Ʈ�� ���� �޼ҵ�.
        {
            GainEXP(10);
        }

        if(exp > requiredexp)
        {
            LevelUp();
        }

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

    public void SetRequiredEXP()
    {
        requiredexp = (player_Level * additionalRequiredExp) + defaultRequiredExp; //(���� * �߰� �ʿ����ġ) + �⺻ �ʿ����ġ
        Debug.Log($"{player_Level}������ �ʿ� ����ġ�� {requiredexp}�Դϴ�.");
    }

    public void GainEXP(float gaind_Exp) //����ġ ���� �޼ҵ�
    {
        exp += gaind_Exp;
        UIManager.instance.UpdateExpGauge();
        Debug.Log($"������� ���� ����ġ��{exp}�Դϴ�.");

    }

    public void LevelUp()
    {
        player_Level++;
        exp = 0;
        //Ư������Ʈ++;
        SetRequiredEXP();
        UIManager.instance.UpdatePlayerLevel();
        UIManager.instance.UpdateExpGauge();
        ActiveAugSelect();
    }

    private void ActiveAugSelect()
    {
        Time.timeScale = 0;
        augSelect.SetActive(true);
    }
    public void SelectAugDone()
    {
        Time.timeScale = 1;
        augSelect.SetActive(false);
    }
}
