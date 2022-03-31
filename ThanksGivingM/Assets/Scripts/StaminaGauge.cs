using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    public float xOffset = 1;
    public float yOffset = .5f;
    public float zOffset = 3.5f;

    public Transform farmerTransform;
    public FarmerCondition farmerCondition;
    public Image fill;
    // Start is called before the first frame update
    void Start()
    {
       farmerCondition = farmerTransform.GetComponent<FarmerCondition>();
    }

    public void UpdateStamina() // 스태미나 UI 업데이트
    {
        fill.fillAmount = (farmerCondition.stamina / farmerCondition.full_stamina);
        if (.5f < fill.fillAmount)
            fill.color = new Color(.25f, 1, 0, 1);
        else if (.2f < fill.fillAmount && fill.fillAmount <= .5f)
            fill.color = new Color(1, .6f, 0, 1);
        else
            fill.color = Color.red;      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(farmerTransform.transform.position.x - xOffset, farmerTransform.transform.position.y + yOffset, zOffset);
    }
}
