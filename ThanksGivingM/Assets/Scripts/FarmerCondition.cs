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


    public string farmer_Name;      //��� �̸�
    public string family;           //��� �Ҽ� �йи�
    public int level;               //��� ����   
    public string faverite_Type;    //��� ��ȣ �۹� Ÿ��;
    public float full_stamina;      //��� �ִ� ü��
    public float stamina;           //��� ���� ü��
    public float harvest_Speed;     //��Ȯ �ӵ�
    public float atk;               //��� ���ݷ�
    public float heal_To_exp_weight = 0.5f; //������ ����ġ�� �ٲٴ� �Ϳ� ���� ����ġ.

    #region ȣ������ ������
    public float reactGauge; //ȣ�� ������
    public const float MAXREACTGAUGE = 100; //�ִ� ȣ�� ������
    private float reactDuration = 3; //ȣ�� ������ ���ӽð�
    private float reactTimer = 0f;
    private float reactSpan = 1f;
    private float increaseReactGaugeValue = 20; //ȣ�� ������ ��·�
    private float decreaseReactGaugeValue = 10; //ȣ�� ������ �϶���
    #endregion

    private bool isInBuffZone; //���� �� �ȿ� �����ִ°�?


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
        ReactTimer(); //ȣ�� ���� �޼ҵ�

    }

    public float value;

    public void DecreaseStamina(params float[] values) //���¹̳� ����
    {
        for (int i = 0; i < values.Length; i++)
        {
            value += values[i];
        }

        stamina -= value; //���¹̳� ȸ��
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();    

        value = 0; //���� �ʱ�ȭ 
    }

    public void IncreaseStamina(params float[] values) //���¹̳� ȸ��
    {
        for (int i = 0; i < values.Length; i++)
        {
            value += values[i];
        }

        stamina += value; //���¹̳� ȸ��
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();

        value = 0; //���� �ʱ�ȭ 
    }


    public void Healed(params float[] values) //�� �ޱ�
    {
        for (int i = 0; i < values.Length; i++) 
        {
            value += values[i];
        }

        stamina += value; //���¹̳� ȸ��
        staminaGauge.GetComponent<StaminaGauge>().UpdateStamina();
        GameManager.Instance.GainEXP(value * heal_To_exp_weight); //������ ����ġ ��ŭ ���ؼ� ����ġ�� ȯ�� �� ���ӸŴ������� �Ѱ���.

        value = 0; //���� �ʱ�ȭ

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
        if (reactTimer > reactSpan) // ȣ�� Ÿ�̸Ӹ� ���.
        {
            if (isInBuffZone)
                reactGauge += increaseReactGaugeValue; //1�ʰ� �Ǿ��� �� �������� �ִٸ� ����.
            else
                reactGauge -= decreaseReactGaugeValue; //�ƴ϶�� ����.

            reactTimer = 0;

            if (reactGauge > MAXREACTGAUGE) //ȣ�� �������� �� ����
            {
                atk += (GameManager.Instance.player_Level * 2);
                Debug.Log("ȣ��!");
                reactFX.SetActive(true);
                StartCoroutine(RewindReactBuff(GameManager.Instance.player_Level * 2));
                GameManager.Instance.GainEXP(GameManager.Instance.requiredexp * 0.05f); //�ʿ� ����ġ�� 5�ۼ�Ʈ ȹ��
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
