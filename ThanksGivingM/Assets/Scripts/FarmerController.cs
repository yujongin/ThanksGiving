using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmerController : MonoBehaviour
{
    private FarmerCondition farmerCondition;

    private Field field;            //밭 정보
    private GameObject target_Crop; // 현재 목표 작물
    private Crop crop;

    private Animator animator;
    private Animator cropAnimator;

    public int x;  //수확해야 하는 작물 배열 x 
    private int y;  //농부의 y위치;

    public int cropNum;

    public float restTime;

    private bool arrived; //농부가 작물 앞에 도착했는지 확인하는 변수
    private bool isRest;  //농부가 쉬고있는 중인지 확인하는 변수
    private bool isFar;

    private float stage_level;


    private float frontRest;
    private float span;



    public GameObject harvestCanvas;
    public Text harvestText;
    // Start is called before the first frame update
    void Start()
    {
        farmerCondition = GetComponent<FarmerCondition>();
        field = GameObject.Find("Field").GetComponent<Field>();
        animator = gameObject.GetComponent<Animator>();

        x = 0;
        y = (int)transform.position.y;
        cropNum = 0;
        arrived = false;
        complete = true;
        span = 0;
        frontRest = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) //테스트를 위한 메소드. 누르면 탈진한다.
        {
            farmerCondition.stamina = 0;
        }

    

            //만약 목표 작물이 없으면 받아온다.
            if (target_Crop == null)
            {
            GetCrop();
            cropAnimator = target_Crop.GetComponent<Animator>();
            }
        TooFar();
        if (!isRest&&!isFar&&complete)
        {
            animator.SetBool("FrontRest", false);
            animator.SetBool("Exhaust", false);
            animator.SetBool("Walk", true);

            Walk();
        }
        if (arrived&&!isRest)
        {
            complete = false;
            animator.SetBool("FrontRest", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Exhaust", false);
            Harvest();
            span += Time.deltaTime;
            if (span > 1f)
            {
                span = 0;
                if (target_Crop)
                    farmerCondition.DecreaseStamina(target_Crop.GetComponent<Crop>().har);                 
            }

        }
        if (isRest)
        {
            animator.SetBool("FrontRest", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Exhaust", true);
            RT += Time.deltaTime;
            if (RT > 1)
            {
                RT = 0;
                farmerCondition.IncreaseStamina(farmerCondition.full_stamina * 0.1f);              
            }
            if (farmerCondition.stamina >= farmerCondition.full_stamina)
            {
                farmerCondition.stamina = farmerCondition.full_stamina;
                isRest = false;
            }
        }
        if (isFar)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Exhaust" +
                "", false);
            animator.SetBool("FrontRest", true);
            span = span += Time.deltaTime;
            if (span > frontRest)
            {
                if (farmerCondition.stamina < farmerCondition.full_stamina)
                {
                    farmerCondition.IncreaseStamina(farmerCondition.full_stamina * 0.1f);
                }
                if (farmerCondition.stamina > farmerCondition.full_stamina)
                {
                    farmerCondition.stamina = farmerCondition.full_stamina;
                }
                span = 0;
            }
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
    private float RT = 0;
    //작물 수확
    private void Harvest()
    {
        delta += Time.deltaTime;
        //공격속도에 따라서
        if (delta * farmerCondition.harvest_Speed > 1) 
        {
            //작물의 체력을 깎는다.
            animator.SetTrigger("Harvest");
            cropAnimator.SetTrigger("Harvesting");
            crop.hp -= farmerCondition.atk;
            delta = 0;
        }
        Rest();
        //작물의 체력이 0보다 같거나 작으면
        if (crop.hp <= 0)
        {            
            GameObject go = Instantiate(harvestCanvas);
            harvestText = go.GetComponentInChildren<Text>();
            harvestText.text = "+"+crop.har.ToString();
            go.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);


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
        if (farmerCondition.stamina <=0)
        {
            isRest = true;          
            GameManager.Instance.tension -= 10;
            UIManager.instance.UpdateTensionGauge();
        }  
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
