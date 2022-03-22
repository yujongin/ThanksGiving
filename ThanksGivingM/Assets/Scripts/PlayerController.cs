using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick joystick;

    public float moveSpeed;
    Vector2 moveVec;
    private Animator player_Animator;

    private void Start()
    {
        player_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        moveVec = new Vector2(moveX, moveY);

        #region 이동 제한

        VerticalLimit();
        HorizontalLimit();
        ZLayerCulc(); //Z축 맞춰주기

        #endregion


        transform.Translate(moveVec * Time.deltaTime * moveSpeed);

        if(moveX != 0 || moveY != 0) //움직이고 있으면
        {
            player_Animator.SetBool("isWalk", true);

            if(moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);


        }


        else
            player_Animator.SetBool("isWalk", false);




    }


    private void VerticalLimit() //세로 이동의 한계점을 잡아주는 메소드
    {
        if (transform.position.y < 0.1f)   
        {
            transform.position = new Vector2(transform.position.x, 0.1f);
        }
        else if (transform.position.y > 4.8f)
        {
            transform.position = new Vector2(transform.position.x, 4.8f);
        }
    }

    private void HorizontalLimit() //가로 이동의 한계점을 잡아주는 메소드
    {
        if(GameManager.Instance.camera1.transform.position.x - 6.7f > transform.position.x) //카메라의 중심 좌표를 기준으로 계산한 값.
        {
            transform.position = new Vector2(GameManager.Instance.camera1.transform.position.x - 6.7f, transform.position.y);
        }
        else if(GameManager.Instance.camera1.transform.position.x + 7 < transform.position.x)
        {
            transform.position = new Vector2(GameManager.Instance.camera1.transform.position.x + 7, transform.position.y);
        }
    }

    private void ZLayerCulc() //Y축 이동에 따라 Z축을 바꿔주는 메소드
    {

        if(transform.position.y < 0.4f) // 범위가 너무 어색하다. 작물의 밑둥을 지날 때 z축이 변화하게 하자.
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.4f);
        }
        else if(0.4f < transform.position.y && transform.position.y < 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.4f);
        }
        else if (1.5f < transform.position.y && transform.position.y < 2.7f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1.4f); //3일 때
        }
        else if (2.7f < transform.position.y && transform.position.y < 4.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 2.4f);
        } 
        else if(4.1f < transform.position.y && transform.position.y < 5.3f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 3.4f);
        }
        else if(5.3f < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 4.4f);
        }
    }

}

