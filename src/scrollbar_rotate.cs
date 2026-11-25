using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class scrollbar_rotate : MonoBehaviour
{
    // Start is called before the first frame update
    Scrollbar wind_angle;
    GameObject map;
    GameObject text_obj;
    TextMeshProUGUI value_text;
    float rotationSpeed = 5f;
    int change = 0;
    void Start()
    {
        map = GameObject.Find("centralPoint");
        wind_angle = this.GetComponent<Scrollbar>();
        text_obj = GameObject.Find("Text_angle");

        value_text = text_obj.GetComponent<TextMeshProUGUI>();


        //wind_hight.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //map.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        
        wind_angle.onValueChanged.AddListener((value) => {
           float value_change = 0;

            value_change = value*360;
            int value_change_int = (int)value_change;
            //if(change!= value_change_int)
            //{
                map.transform.rotation = Quaternion.Lerp(map.transform.rotation, Quaternion.Euler(Vector3.up * (value_change_int)), Time.deltaTime * rotationSpeed);
                //change = value_change_int;
           // }
            
            
            string stringValue = value_change_int.ToString();
            value_text.SetText(stringValue + "бу");
            Debug.Log("Scrollbar angle: " + stringValue);
        });
    }
}
