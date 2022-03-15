using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        Heal, //ȸ��
        Speed //��������
    }
    

    public BuffType bufftype;
    public List<GameObject> farmersInBuffZone; //���� ������ �����ִ� ��ε��� ��� ���� ����.


    #region ��ȭ��ư ���� ����
    public float enhanceButtonCool { get; private set; } //��ȭ��ư ��Ÿ�� ��Ȳ�� UI�Ŵ������� �Ѱ��ֱ� ���� ������Ƽ.
    public float enhanceDuration = 3f;
    private const float ENHANCECOOLTIME = 1f; //��ȭ��ư ��Ÿ�� �ֱ�
    private bool isOnEnhanceCoolTime = false;
    #endregion

    public float noteGenerateSpan = 8f;   //��Ʈ ���� �ֱ�
    private float noteGenerateTimer = 0f; // ��Ʈ ���� Ÿ�̸�

    private float buffSpan = 1; //�����ֱ�
    private float buffTimer;

    public float default_HealValue = 1;  //�⺻ ����. Ư���� ���� ���� ���ɼ��� ����Ͽ� �ۺ����� ����.
    public float enhanced_HealValue = 4; //��ȭ ����. �����δ� �⺻���� + ��ȭ�������� ���ȴ�.

    private bool isEnhanced = false;

    public int maxnotes = 5; //�ִ� ��Ʈ ��. Ư���� ���� �þ �� �ֱ� ������ ������ ���ξ���.
    public int notes; //���� ��Ʈ ��.

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

        BuffTimer(); //�ֱ� ���� ������ ����(?)

        GenerateNoteTimer(); // n�ʸ��� ��Ʈ�� �������ִ� Ÿ�̸� �޼ҵ�.

        EnhanceButtonTimer(); //��ȭ ��ư ��Ÿ�� Ÿ�̸�

    }

    private void GenerateNoteTimer() //��Ʈ ���� Ÿ�̸�
    {
        noteGenerateTimer -= Time.deltaTime;

        if (noteGenerateTimer < 0)
        {
            notes++;
            UIManager.instance.UpdateNoteUI();
            noteGenerateTimer = noteGenerateSpan;
        }
    }
    private void EnhanceButtonTimer() //��Ʈ ���� Ÿ�̸�
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


    #region ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Farmer")
        {              
            farmersInBuffZone.Add(collision.gameObject);
            //if(!farmersInBuffZone.Contains(collision.GetComponent<FarmerController>()))
            //   farmersInBuffZone.Add(collision.GetComponent<FarmerController>());

               Debug.Log($"{farmersInBuffZone.Count}���� ��ΰ� ���� �ȿ� ����.");
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Farmer")
        {
            farmersInBuffZone.Remove(collision.gameObject);

            //farmersInBuffZone.Remove(collision.GetComponent<FarmerController>());
            Debug.Log($"��ΰ� ���� ���� ����. �� {farmersInBuffZone.Count}���� ��ΰ� ������ ����.");
        }
    }

    #endregion


    #region ������

    private void BuffTimer()
    {
        buffTimer += Time.deltaTime; //Ÿ�̸Ӹ� ���.

        if (buffTimer > buffSpan) //���� �ֱ⸦ �Ѿ�� ���� �޼ҵ� ����.
        {
            switch (bufftype) //���� ���� Ÿ���� �����ΰ�?
            {
                case BuffType.Heal: //�� ����
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

        if (farmersInBuffZone == null) //�����ȿ� ��ΰ� ������ ����.
            return;


        if(!isEnhanced)//��ȭ ���°� �ƴ϶��
            for (int i = 0; i < farmersInBuffZone.Count; i++)
            {
                farmersInBuffZone[i].GetComponent<FarmerController>().stamina += default_HealValue; //�������� ��ε鿡�� �⺻������ŭ ���¹̳� ȸ��.
            }

        else //��ȭ���¶��
            for (int i = 0; i < farmersInBuffZone.Count; i++)
            {
                farmersInBuffZone[i].GetComponent<FarmerController>().stamina
                    += (default_HealValue + enhanced_HealValue); //�������� ��ε鿡�� �⺻���� + ��ȭ������ŭ ȸ��
            }


    }
    #endregion


    #region ��ȭ��ư�� ���� �޼ҵ� ��

    //��ư�� ������ ��ȭ���� ��ƾ�� �����ϸ鼭 ��Ÿ�� ��ƾ�� ���� �����Ѵ�.
    //�׷��鼭 enhanceButtonCool�̶�� ������Ƽ�� ��Ÿ�� ���� ��Ƽ� UI�Ŵ������� �Ѱ��ְ�, UI�Ŵ����� ��Ÿ���� �ð�ȭ�ؼ� �����ش�.

    public void CallEnhancedBuffMethod()
    {
        if(notes > 0 && !isOnEnhanceCoolTime) //��Ʈ�� 1�� �̻��̰�, ��ư ��Ÿ���� �ƴ϶�� ����.
        {
            StartCoroutine(EnhancedBuffRoutine());     //��ȭ ���� ��ƾ ����
            StartCoroutine(EnhanceCoolTimeRoutine());  //��ȭ ���� ��ư ��Ÿ�� ����
        }   
    }
    IEnumerator EnhancedBuffRoutine() //��ȭ���� �ڷ�ƾ.
    {
        notes--;
        UIManager.instance.UpdateNoteUI();
        StopCoroutine(EnhancedBuffRoutine());
        isEnhanced = true;
        yield return new WaitForSeconds(enhanceDuration);
        isEnhanced = false;
    }
    IEnumerator EnhanceCoolTimeRoutine() //��ȭ��ư�� ��Ÿ��.
    {
        isOnEnhanceCoolTime = true;
        yield return new WaitForSeconds(ENHANCECOOLTIME);
        isOnEnhanceCoolTime = false;
    }
    #endregion

}
