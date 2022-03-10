using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public string croptype; //�۹� Ÿ��
    public int cropLevel;   //�۹� ����
    public Sprite deadCrop; //��Ȯ �� �۹� �̹���
    public float atk;       //�۹� ���ݷ�
    public float hp;        //�۹� ü��
    public int har;         //��Ȯ��


    private Animator animator;

    public bool harvested;

    void Start()
    {
        animator = GetComponent<Animator>();

        harvested = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !harvested) 
        {
            animator.SetTrigger("Dead");
            harvested = true;
            Debug.Log("�����̴� �ٺ���!");
        }
        if (transform.position.x < GameManager.Instance.camera1.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }
}
