using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public string croptype; //작물 타입
    public int cropLevel;   //작물 레벨
    public Sprite deadCrop; //수확 된 작물 이미지
    public float atk;       //작물 공격력
    public float hp;        //작물 체력
    public int har;         //수확량


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
            Debug.Log("상윤이는 바보야!");
        }
        if (transform.position.x < GameManager.Instance.camera1.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }
}
