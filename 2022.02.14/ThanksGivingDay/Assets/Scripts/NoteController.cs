using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public int moveSpeed;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime); //1.697
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "JudgLine")
        {
            Debug.Log($"걸린시간: {timer}");
        }
    }
}
