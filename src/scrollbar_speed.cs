using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class scrollbar_speed : MonoBehaviour
{
    // Start is called before the first frame update
    Scrollbar wind_speed;
    GameObject ws;
    GameObject text_obj;
    TextMeshProUGUI value_text;
    float rotationSpeed = 5f;
    int change = 0;
    mapRotate readDataScript;
    void Start()
    {
        ws = GameObject.Find("centralPoint");
        wind_speed = this.GetComponent<Scrollbar>();
        text_obj = GameObject.Find("Text_speed");

        value_text = text_obj.GetComponent<TextMeshProUGUI>();
        readDataScript = ws.GetComponent<mapRotate>();

        //wind_hight.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //map.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        wind_speed.onValueChanged.AddListener((value) => {
            float value_change = 0;

            value_change = value * 100;
            int value_change_int = (int)value_change;
            //if(change!= value_change_int)
            //{
            readDataScript.wind_speed = value_change;
            // }


            string stringValue = value_change_int.ToString();
            value_text.SetText(stringValue+"m/s");
            Debug.Log("Scrollbar angle: " + stringValue);
        });
    }
}
