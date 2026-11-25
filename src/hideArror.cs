using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideArror : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
       
    }
   
    // Update is called once per frame
    bool isActive = false;
    void Update()
    {
       // Camera.main.cullingMask &= ~(1 << 6); // 在原来的基础上减掉第x层

    }

    public void onClickHideArror()
    {
        Camera.main.cullingMask &= ~(1 << 6); // 在原来的基础上减掉第x层
    }
    public void onClickShowArror()
    {
        Camera.main.cullingMask = -1;
    }
    /*
    void FixedUpdate()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (isActive)
            {
                isActive = false;
                Camera.main.cullingMask &= ~(1 << 6); // 在原来的基础上减掉第x层
            }
            else
            {
                isActive = true;
                Camera.main.cullingMask = -1;
            }

        }

    }
    */
}
