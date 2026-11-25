using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class createObject : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 dir = new Vector3(1, 0, 0);

    float speed = 10f;

    void Start()
    {
       // GameObject ori_text = GameObject.Find("Text_cell_data");



        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                for (int k = 0; k < 10; k++)
                {
                    int i_scale = i * 10;
                    int j_scale = j * 10;
                    int k_scale = k * 10;
                    Vector3 location = new Vector3(i, j, k);
                    Vector3 new_scale = new Vector3(1, 8, 1);
                    GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

                    cap.transform.rotation = Quaternion.Euler(i_scale, j_scale, k_scale);

                    cap.transform.localScale = new_scale / 30;
                    cap.transform.position = location;
                   // GameObject copied_text = Instantiate(ori_text);
                    //copied_text.GetComponent<TextMeshPro>().text = "(" + i_scale.ToString() + "," + j_scale.ToString() + "," + k_scale.ToString() + ")";
                    //copied_text.transform.position = location;


                }

            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        //cap.transform.position += dir * speed * Time.deltaTime;
    }
}
