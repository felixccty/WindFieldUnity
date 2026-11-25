using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_wind : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject get_speed;
    float wind_speed_new = 0;
    ParticleSystem ps;
    mapRotate readDataScript;
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
        get_speed = GameObject.Find("centralPoint");
        readDataScript = get_speed.GetComponent<mapRotate>();

    }

    // Update is called once per frame
    void Update()
    {
        var folt = ps.forceOverLifetime;
        //fo.enabled = true;
        wind_speed_new = readDataScript.wind_speed;
        folt.z = wind_speed_new + 2f;
        var limitVelocity = ps.limitVelocityOverLifetime;

        limitVelocity.limit = wind_speed_new + 2f;

    }
    void FixedUpdate()
    {
        
        
    }
}
