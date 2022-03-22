using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance //싱글톤.
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

    private static UIManager m_instance; // 싱글톤이 할당될 변수



    public List<GameObject> notes; //음표UI들의 배열. 차례대로 꺼줄 것.
    public Buff buff;
    public Image encoreButtonImage;
    public Image tensionGauge;
    public Image expGauge;
    public Text levelText;

    private void Update()
    {
        encoreButtonImage.fillAmount = buff.enhanceButtonCool; //강화버프의 쿨타임에 따라 강화 버튼의 fill amount를 조정.
    }


    public void UpdateNoteUI() //노트 UI 업데이트.
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


    IEnumerator TensionDecreaseAnimation() //게이지가 줄어드는 것에 대한 애니메이션.
    {
        for (float f = tensionGauge.fillAmount; f > GameManager.Instance.tension * .01f; f -= .01f)
        {
            tensionGauge.fillAmount -= .01f;
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator EXPIncreaseAnimation() //경험치가 늘어나는 것에 대한 애니메이션.
    { //수정해야함! 2022.03.17 15:24
        float a = (GameManager.Instance.exp * 100) / GameManager.Instance.requiredexp;

        for (float f = expGauge.fillAmount; f < a; f += .01f)
        {
            tensionGauge.fillAmount += .01f;
            yield return new WaitForSeconds(.01f);
        }
    }
}
