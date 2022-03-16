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

        StartCoroutine(tension());
        //tensionGauge.fillAmount = GameManager.Instance.tension * .01f;
    }
    IEnumerator tension()
    {
        for (float f = tensionGauge.fillAmount; f > GameManager.Instance.tension * .01f; f -= .01f)
        {
            tensionGauge.fillAmount -= .01f;
            yield return new WaitForSeconds(.01f);
        }
    }


}
