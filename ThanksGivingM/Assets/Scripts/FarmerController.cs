using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{
    public string farmer_Name;      //농부 이름
    public string family;           //농부 소속 패밀리
    public int level;               //농부 레벨   
    public string faverite_Type;    //농부 선호 작물 타입;
    public float full_stamina;      //농부 최대 체력
    public float stamina;           //농부 현재 체력
    public float harvest_Speed;     //수확 속도
    public float atk;               //농부 공격력

    private Field field;            //밭 정보
    private GameObject target_Crop; // 현재 목표 작물
    private Crop crop;

    private Animator animator;

    public int x;  //수확해야 하는 작물 배열 x 
    private int y;  //농부의 y위치;

    public int cropNum;

    public float restTime;

    private bool arrived; //농부가 작물 앞에 도착했는지 확인하는 변수
    private bool isRest;  //농부가 쉬고있는 중인지 확인하는 변수
    private bool isFar;

    private float stage_level;
    // Start is called before the first frame update
    void Start()
    {
        field = GameObject.Find("Field").GetComponent<Field>();
        animator = gameObject.GetComponent<Animator>();
        x = 0;
        y = (int)transform.position.y;
        cropNum = 0;
        arrived = false;
        complete = true;

    }

    // Update is called once per frame
    void Update()
    {
        //만약 목표 작물이 없으면 받아온다.
        if (target_Crop == null)
        {
            GetCrop();            
        }
        TooFar();
        if (!isRest&&!isFar&&complete)
        {
            animator.SetBool("Rest", false);
            animator.SetBool("Walk", true);

            Walk();
        }
        if (arrived)
        {
            complete = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Rest", false);
            Harvest();
        }

    }

    //목표 작물 받아오기 메서드
    private void GetCrop()
    {
        //1로 나누어 지면 첫 번째 밭에서 받아오기
        if (stage_level % 1 == 0)
        {
            target_Crop = field.firstField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
        //0.5가 남으면 두 번째 밭에서 받아오기
        else
        {
            target_Crop = field.secondField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
    }

    //수확 할 작물 앞으로 걸어가기 메서드
    private void Walk()
    {     
        //농부가 작물 앞에 도착하지 않았으면
        if (transform.position != target_Crop.transform.position - new Vector3(1f, 0, 0.5f))
        {
            //작물 앞까지 이동한다.
            transform.position = Vector3.MoveTowards(transform.position, target_Crop.transform.position - new Vector3(1f, 0, 0.5f), 0.05f);        
        }
        //도착했으면
        else
        {
            //도착했다고 알림
            arrived = true;
            delta = 1;
        }
    }

    private float delta = 0;
    //작물 수확
    private void Harvest()
    {
        delta += Time.deltaTime;
        //공격속도에 따라서
        if (delta * harvest_Speed > 1) 
        {
            //작물의 체력을 깎는다.
            animator.SetTrigger("Harvest");
            crop.hp -= atk;
            delta = 0;
        }
        //작물의 체력이 0보다 같거나 작으면
        if (crop.hp <= 0)
        {    
            //농부의 스테미나를 작물 공격력 만큼 감소
            stamina -= crop.atk;
            Rest();
            

            //다음 작물을 받아오기 위해 변수 초기화
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
            StartCoroutine("Wait");
        }
    }
    private bool complete;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        complete = true;
    }
    private void Rest()
    {
        if (stamina<=0)
        {
            animator.SetBool("Rest", true);
            isRest = true;
            StartCoroutine("Recovery");
        }  
    }

    IEnumerator Recovery()
    {
        yield return new WaitForSeconds(restTime);
        stamina = full_stamina;
        isRest = false;
    }

    private void TooFar()
    {
        float addX = 8f;
        if (transform.position.y == 0)
        {
            addX -= 2f;
        }
        if(target_Crop.transform.position.x > GameManager.Instance.camera1.transform.position.x + addX)
        {
            isFar = true;
        }
        else
        {
            isFar = false;
        }
    }


}
