using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float stage_Level;
    public float player_Level;


    public float tension; //텐션게이지
    public float maxTension = 100; //텐션게이지 최대치


    #region 경험치 관련 변수
    public float exp { get; private set;} //현재 경험치. 게임매니저 만이 수치를 조정할 수 있다.

    public float requiredexp { get; private set;} //필요 경험치. 게임매니저 만이 수치를 조정할 수 있다.
    private float additionalRequiredExp = 30; //레벨 상승에 따른 추가 필요경험치 값.
    private const float defaultRequiredExp = 50; //필요 경험치 기본 값.
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
        if (Input.GetKeyDown(KeyCode.E)) //테스트를 위한 메소드.
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
        requiredexp = (player_Level * additionalRequiredExp) + defaultRequiredExp; //(레벨 * 추가 필요경험치) + 기본 필요경험치
        Debug.Log($"{player_Level}레벨의 필요 경험치는 {requiredexp}입니다.");
    }

    public void GainEXP(float gaind_Exp) //경험치 증가 메소드
    {
        exp += gaind_Exp;
        UIManager.instance.UpdateExpGauge();
        Debug.Log($"현재까지 얻은 경험치는{exp}입니다.");

    }

    public void LevelUp()
    {
        player_Level++;
        exp = 0;
        //특성포인트++;
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
