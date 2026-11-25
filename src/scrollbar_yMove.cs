using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class scrollbar_yMove : MonoBehaviour
{
    // Start is called before the first frame update
    Scrollbar wind_hight;
    GameObject map;
    GameObject text_obj;
    TextMeshProUGUI value_text;
    void Start()
    {
        map = GameObject.Find("centralPoint");
        wind_hight = this.GetComponent<Scrollbar>();
        text_obj = GameObject.Find("Text_hight");
        
        value_text =text_obj.GetComponent<TextMeshProUGUI>();

        
        //wind_hight.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        wind_hight.onValueChanged.AddListener((value) => {
            int value_change = 0;
            value_change = (int) (value * 100);
            Vector3 newPosition = new Vector3(0, 100-value_change, 0); // x、y、z分别表示新的位置坐标值
            map.transform.position = newPosition;

            string stringValue = value_change.ToString();
            value_text.SetText(stringValue);
            Debug.Log("Scrollbar value changed: " + value_change);
        });
    }
}
