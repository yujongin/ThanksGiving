using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmerCondition : MonoBehaviour
{
    public GameObject staminaGaugePrefab;
    public GameObject staminaGauge;
    public Image staminaGaugeImage;

    public GameObject healedFX;
    public GameObject reactFX;


    public string farmer_Name;      //농부 이름
    public string family;           //농부 소속 패밀리
    public int level;               //농부 레벨   
    public string faverite_Type;    //농부 선호 작물 타입;
    public float full_stamina;      //농부 최대 체력
    public float stamina;           //농부 현재 체력
    public float harvest_Speed;     //수확 속도
    public float atk;               //농부 공격력
    public float heal_To_exp_weight = 0.5f; //힐량을 경험치로 바꾸는 것에 대한 가중치.

    #region 호응관련 변수들
    public float reactGauge; //호응 게이지
    public const float MAXREACTGAUGE = 100; //최대 호응 게이지
    private float reactDuration = 3; //호응 게이지 지속시간
    private float reactTimer = 0f;
    private float reactSpan = 1f;
    private float increaseReactGaugeValue = 20; //호응 게이지 상승량
    private float decreaseReactGaugeValue = 10; //호응 게이지 하락량
    #endregion

    private bool isInBuffZone; //버프 존 안에 들어와있는가?


    // Start is called before the first frame update
    void Start()
    {
        staminaGauge = Instantiate(staminaGauge, GameObject.Find("WorldCanvas").transform);
        staminaGauge.name = $"{this.name} staminaGauge";
        staminaGauge.GetComponent<StaminaGauge>().farmerTransform = gameObject.transform;
        staminaGaugeImage = staminaGauge.GetComponentInChildren<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        ReactTimer(); //호응 관련 메소드

    }

    public float value;

    public void DecreaseStamina(params float[] values) //스태미나 감소
    {
        for (int i = 0; i < values.Length; i++)
        {
            value += values[i];
        }

        stamina -= value; //스태미나 회복
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();    

        value = 0; //변수 초기화 
    }

    public void IncreaseStamina(params float[] values) //스태미나 회복
    {
        for (int i = 0; i < values.Length; i++)
        {
            value += values[i];
        }

        stamina += value; //스태미나 회복
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();

        value = 0; //변수 초기화 
    }


    public void Healed(params float[] values) //힐 받기
    {
        for (int i = 0; i < values.Length; i++) 
        {
            value += values[i];
        }

        stamina += value; //스태미나 회복
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();
        GameManager.Instance.GainEXP(value * heal_To_exp_weight); //힐량에 가중치 만큼 곱해서 경험치로 환산 후 게임매니저에게 넘겨줌.

        value = 0; //변수 초기화

        healedFX.SetActive(true);
    }

    IEnumerator RewindReactBuff(float increaseATK)
    {
        yield return new WaitForSeconds(reactDuration);

        atk -= increaseATK;

    }
    private void ReactTimer()
    {
        reactTimer += Time.deltaTime;
        if (reactTimer > reactSpan) // 호응 타이머를 잰다.
        {
            if (isInBuffZone)
                reactGauge += increaseReactGaugeValue; //1초가 되었을 때 버프존에 있다면 증가.
            else
                reactGauge -= decreaseReactGaugeValue; //아니라면 감소.

            reactTimer = 0;

            if (reactGauge > MAXREACTGAUGE) //호응 게이지가 꽉 차면
            {
                atk += (GameManager.Instance.player_Level * 2);
                Debug.Log("호응!");
                reactFX.SetActive(true);
                StartCoroutine(RewindReactBuff(GameManager.Instance.player_Level * 2));
                GameManager.Instance.GainEXP(GameManager.Instance.requiredexp * 0.05f); //필요 경험치의 5퍼센트 획득
                reactGauge = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BuffZone")
        {
            isInBuffZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "BuffZone")
        {
            isInBuffZone = false;
        }
    }
}
