using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneWind : MonoBehaviour
{
    // Start is called before the first frame update
    public singleWindArea singleWA;
    void Start()
    {
        //float flaotFromSWA = singleWA.global_wind_dir_x;
        //Debug.Log("111");
       // Debug.Log(flaotFromSWA);
        //wind_avg_x = GetComponent<singleWindArea>();
        GameObject obj = GameObject.Find("windPT");
      //  float a = wind_avg_x.global_wind_dir_x;
        for (int i = 0; i < 150; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                for (int k = 0; k < 1; k++)
                {
                    GameObject clone = Instantiate(obj, new Vector3((i*4)-300, 100, (k*2)-400), obj.transform.rotation);
                    
                }
                    
            }
                
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
