using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmerController : MonoBehaviour
{
    public string farmer_Name;      //��� �̸�
    public string family;           //��� �Ҽ� �йи�
    public int level;               //��� ����   
    public string faverite_Type;    //��� ��ȣ �۹� Ÿ��;
    public float full_stamina;      //��� �ִ� ü��
    public float stamina;           //��� ���� ü��
    public float harvest_Speed;     //��Ȯ �ӵ�
    public float atk;               //��� ���ݷ�

    private Field field;            //�� ����
    private GameObject target_Crop; // ���� ��ǥ �۹�
    private Crop crop;

    private Animator animator;
    private Animator cropAnimator;

    public int x;  //��Ȯ�ؾ� �ϴ� �۹� �迭 x 
    private int y;  //����� y��ġ;

    public int cropNum;

    public float restTime;

    private bool arrived; //��ΰ� �۹� �տ� �����ߴ��� Ȯ���ϴ� ����
    private bool isRest;  //��ΰ� �����ִ� ������ Ȯ���ϴ� ����
    private bool isFar;

    private float stage_level;


    private float frontRest;
    private float span;

    public GameObject harvestCanvas;
    public Text harvestText;
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
        span = 0;
        frontRest = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        //���� ��ǥ �۹��� ������ �޾ƿ´�.
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
                if(target_Crop)
                    stamina -= target_Crop.GetComponent<Crop>().har;
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
                stamina += full_stamina * 0.1f;
            }
            if (stamina >= full_stamina)
            {
                stamina = full_stamina;
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
                if (stamina < full_stamina)
                {
                    stamina += full_stamina * 0.1f;
                }
                if (stamina > full_stamina)
                {
                    stamina = full_stamina;
                }
                span = 0;
            }
        }

    }

    //��ǥ �۹� �޾ƿ��� �޼���
    private void GetCrop()
    {
        //1�� ������ ���� ù ��° �翡�� �޾ƿ���
        if (stage_level % 1 == 0)
        {
            target_Crop = field.firstField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
        //0.5�� ������ �� ��° �翡�� �޾ƿ���
        else
        {
            target_Crop = field.secondField[x, y];
            crop = target_Crop.GetComponent<Crop>();
        }
    }

    //��Ȯ �� �۹� ������ �ɾ�� �޼���
    private void Walk()
    {     
        //��ΰ� �۹� �տ� �������� �ʾ�����
        if (transform.position != target_Crop.transform.position - new Vector3(1f, 0, 0.5f))
        {
            //�۹� �ձ��� �̵��Ѵ�.
            transform.position = Vector3.MoveTowards(transform.position, target_Crop.transform.position - new Vector3(1f, 0, 0.5f), 0.05f);        
        }
        //����������
        else
        {
            //�����ߴٰ� �˸�
            arrived = true;
            delta = 1;
        }
    }

    private float delta = 0;
    private float RT = 0;
    //�۹� ��Ȯ
    private void Harvest()
    {
        delta += Time.deltaTime;
        //���ݼӵ��� ����
        if (delta * harvest_Speed > 1) 
        {
            //�۹��� ü���� ��´�.
            animator.SetTrigger("Harvest");
            cropAnimator.SetTrigger("Harvesting");
            crop.hp -= atk;
            delta = 0;
        }
        Rest();
        //�۹��� ü���� 0���� ���ų� ������
        if (crop.hp <= 0)
        {            
            GameObject go = Instantiate(harvestCanvas);
            harvestText = go.GetComponentInChildren<Text>();
            harvestText.text = "+"+crop.har.ToString();
            go.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);


            //���� �۹��� �޾ƿ��� ���� ���� �ʱ�ȭ
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
