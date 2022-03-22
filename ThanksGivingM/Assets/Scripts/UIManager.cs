using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance //�̱���.
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // �̱����� �Ҵ�� ����



    public List<GameObject> notes; //��ǥUI���� �迭. ���ʴ�� ���� ��.
    public Buff buff;
    public Image encoreButtonImage;
    public Image tensionGauge;
    public Image expGauge;
    public Text levelText;

    private void Update()
    {
        encoreButtonImage.fillAmount = buff.enhanceButtonCool; //��ȭ������ ��Ÿ�ӿ� ���� ��ȭ ��ư�� fill amount�� ����.
    }


    public void UpdateNoteUI() //��Ʈ UI ������Ʈ.
    {        
        for(int i = 0; i < notes.Count; i++)
        {
            notes[i].SetActive(true);
        }
        for (int i = 0; i < buff.maxnotes - buff.notes; i++)
        {
            notes[i].SetActive(false);
        }
    }

    public void UpdateTensionGauge()
    {

        StartCoroutine(TensionDecreaseAnimation());
        //tensionGauge.fillAmount = GameManager.Instance.tension * .01f;
    }
    public void UpdatePlayerLevel()
    {
        levelText.text = ($"{GameManager.Instance.player_Level}");
    }
    public void UpdateExpGauge()
    {
        //StartCoroutine(EXPIncreaseAnimation());

        float a = (GameManager.Instance.exp * 100) / GameManager.Instance.requiredexp;
        expGauge.fillAmount = a * 0.01f;
        //Debug.Log(a);
    }


    IEnumerator TensionDecreaseAnimation() //�������� �پ��� �Ϳ� ���� �ִϸ��̼�.
    {
        for (float f = tensionGauge.fillAmount; f > GameManager.Instance.tension * .01f; f -= .01f)
        {
            tensionGauge.fillAmount -= .01f;
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator EXPIncreaseAnimation() //����ġ�� �þ�� �Ϳ� ���� �ִϸ��̼�.
    { //�����ؾ���! 2022.03.17 15:24
        float a = (GameManager.Instance.exp * 100) / GameManager.Instance.requiredexp;

        for (float f = expGauge.fillAmount; f < a; f += .01f)
        {
            tensionGauge.fillAmount += .01f;
            yield return new WaitForSeconds(.01f);
        }
    }
}
