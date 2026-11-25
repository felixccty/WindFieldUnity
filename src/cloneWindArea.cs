using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cloneWindArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int shift_x = -30;
        int shift_y = -20;
        int shift_z = -30;
        GameObject obj = GameObject.Find("singleWindArea");
        //GameObject obj_dir = GameObject.Find("dir");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    GameObject clone = Instantiate(obj, new Vector3(i*60+ shift_x, j+ shift_y, k * 60+ shift_z), Quaternion.Euler(i*20, j * 20, k * 20));
                   // GameObject clone_dir = Instantiate(obj_dir, new Vector3(i, j, k), Quaternion.Euler(i * 10, j * 10, k * 10));
                }
}

        }
    }

    // Update is called once per frame
    void Update()
{
        
    }
}
