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





}
