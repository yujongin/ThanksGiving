using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        Heal, //회복
        Speed //공속증가
    }
    

    public BuffType bufftype;
    public List<GameObject> farmersInBuffZone; //버프 범위에 들어와있는 농부들을 담아 놓을 변수.


    #region 강화버튼 관련 변수
    public float enhanceButtonCool { get; private set; } //강화버튼 쿨타임 현황을 UI매니저에게 넘겨주기 위한 프로퍼티.
    public float enhanceDuration = 3f;
    private const float ENHANCECOOLTIME = 1f; //강화버튼 쿨타임 주기
    private bool isOnEnhanceCoolTime = false;
    #endregion

    public float noteGenerateSpan = 8f;   //노트 생성 주기
    private float noteGenerateTimer = 0f; // 노트 생성 타이머

    private float buffSpan = 1; //버프주기
    private float buffTimer;

    public float default_HealValue = 1;  //기본 힐량. 특성에 의해 변동 가능성을 고려하여 퍼블릭으로 설정.
    public float enhanced_HealValue = 4; //강화 힐량. 실제로는 기본힐량 + 강화힐량으로 사용된다.

    private bool isEnhanced = false;

    public int maxnotes = 5; //최대 노트 수. 특성에 의해 늘어날 수 있기 때문에 변수로 놔두었다.
    public int notes; //현재 노트 수.

    // Start is called before the first frame update
    void Start()
    {
        bufftype = BuffType.Heal;
        notes = maxnotes;
        noteGenerateTimer = noteGenerateSpan;
    }

    // Update is called once per frame
    void Update()
    {

        BuffTimer(); //주기 마다 버프를 방출(?)

        GenerateNoteTimer(); // n초마다 노트를 생성해주는 타이머 메소드.

        EnhanceButtonTimer(); //강화 버튼 쿨타임 타이머

    }

    private void GenerateNoteTimer() //노트 생성 타이머
    {
        noteGenerateTimer -= Time.deltaTime;

        if (noteGenerateTimer < 0)
        {
            notes++;
            UIManager.instance.UpdateNoteUI();
            noteGenerateTimer = noteGenerateSpan;
        }
    }
    private void EnhanceButtonTimer() //노트 생성 타이머
    {
        if (isOnEnhanceCoolTime)
        {
            enhanceButtonCool -= Time.deltaTime;
        }
        else
        {
            enhanceButtonCool = 1f;
        }
        if (enhanceButtonCool < 0f)
            enhanceButtonCool = 1f;
    }


    #region 농부 감지

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Farmer")
        {              
            farmersInBuffZone.Add(collision.gameObject);
            //if(!farmersInBuffZone.Contains(collision.GetComponent<FarmerController>()))
            //   farmersInBuffZone.Add(collision.GetComponent<FarmerController>());

               Debug.Log($"{farmersInBuffZone.Count}명의 농부가 범위 안에 들어옴.");
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Farmer")
        {
            farmersInBuffZone.Remove(collision.gameObject);

            //farmersInBuffZone.Remove(collision.GetComponent<FarmerController>());
            Debug.Log($"농부가 범위 에서 빠짐. 총 {farmersInBuffZone.Count}명의 농부가 범위에 남음.");
        }
    }

    #endregion


    #region 버프들

    private void BuffTimer()
    {
        buffTimer += Time.deltaTime; //타이머를 잰다.

        if (buffTimer > buffSpan) //버프 주기를 넘어서면 버프 메소드 실행.
        {
            switch (bufftype) //현재 버프 타입이 무엇인가?
            {
                case BuffType.Heal: //힐 버프
                    HealBuff();
                    break;

                default:
                    break;
            }

            buffTimer = 0;
        }
    }



    private void HealBuff()
    {

        if (farmersInBuffZone == null) //범위안에 농부가 없으면 리턴.
            return;


        if(!isEnhanced)//강화 상태가 아니라면
            for (int i = 0; i < farmersInBuffZone.Count; i++)
            {
                farmersInBuffZone[i].GetComponent<FarmerController>().stamina += default_HealValue; //범위안의 농부들에게 기본힐량만큼 스태미나 회복.
            }

        else //강화상태라면
            for (int i = 0; i < farmersInBuffZone.Count; i++)
            {
                farmersInBuffZone[i].GetComponent<FarmerController>().stamina
                    += (default_HealValue + enhanced_HealValue); //범위안의 농부들에게 기본힐량 + 강화힐량만큼 회복
            }


    }
    #endregion


    #region 강화버튼에 관한 메소드 들

    //버튼을 누르면 강화버프 루틴을 시작하면서 쿨타임 루틴도 같이 실행한다.
    //그러면서 enhanceButtonCool이라는 프로퍼티에 쿨타임 값을 담아서 UI매니저에게 넘겨주고, UI매니저가 쿨타임을 시각화해서 보여준다.

    public void CallEnhancedBuffMethod()
    {
        if(notes > 0 && !isOnEnhanceCoolTime) //노트가 1개 이상이고, 버튼 쿨타임이 아니라면 실행.
        {
            StartCoroutine(EnhancedBuffRoutine());     //강화 버프 루틴 시작
            StartCoroutine(EnhanceCoolTimeRoutine());  //강화 버프 버튼 쿨타임 시작
        }   
    }
    IEnumerator EnhancedBuffRoutine() //강화버프 코루틴.
    {
        notes--;
        UIManager.instance.UpdateNoteUI();
        StopCoroutine(EnhancedBuffRoutine());
        isEnhanced = true;
        yield return new WaitForSeconds(enhanceDuration);
        isEnhanced = false;
    }
    IEnumerator EnhanceCoolTimeRoutine() //강화버튼의 쿨타임.
    {
        isOnEnhanceCoolTime = true;
        yield return new WaitForSeconds(ENHANCECOOLTIME);
        isOnEnhanceCoolTime = false;
    }
    #endregion

}
