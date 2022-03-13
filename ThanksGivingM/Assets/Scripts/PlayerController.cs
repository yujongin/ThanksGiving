using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick joystick;

    public float moveSpeed;
    Vector2 moveVec;

    void Update()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        moveVec = new Vector2(x, y);

        #region �̵� ����

        VerticalLimit();
        HorizontalLimit();
        ZLayerCulc(); //Z�� �����ֱ�

        #endregion


        transform.Translate(moveVec * Time.deltaTime * moveSpeed);

    }


    private void VerticalLimit() //���� �̵��� �Ѱ����� ����ִ� �޼ҵ�
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

    private void HorizontalLimit() //���� �̵��� �Ѱ����� ����ִ� �޼ҵ�
    {
        if(GameManager.Instance.camera1.transform.position.x - 6.7f > transform.position.x) //ī�޶��� �߽� ��ǥ�� �������� ����� ��.
        {
            transform.position = new Vector2(GameManager.Instance.camera1.transform.position.x - 6.7f, transform.position.y);
        }
        else if(GameManager.Instance.camera1.transform.position.x + 7 < transform.position.x)
        {
            transform.position = new Vector2(GameManager.Instance.camera1.transform.position.x + 7, transform.position.y);
        }
    }

    private void ZLayerCulc() //Y�� �̵��� ���� Z���� �ٲ��ִ� �޼ҵ�
    {

        if(transform.position.y < 0.4f) // ������ �ʹ� ����ϴ�. �۹��� �ص��� ���� �� z���� ��ȭ�ϰ� ����.
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        }
        else if(0.4f < transform.position.y && transform.position.y < 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
        }
        else if (1.5f < transform.position.y && transform.position.y < 2.7f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1.5f); //3�� ��
        }
        else if (2.7f < transform.position.y && transform.position.y < 4.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 2.5f);
        } 
        else if(4.1f < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 3.5f);
        }
    }

}

