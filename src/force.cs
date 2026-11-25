using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class force : MonoBehaviour
{

    GameObject wind_obj;
    ParticleSystem wind_point;
    
    // Start is called before the first frame update
    void Start()
    {
        wind_obj = GameObject.Find("Particle System");
        wind_point = wind_obj.GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var wind_partical = wind_point.forceOverLifetime;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(0.75f, 1.0f);
        wind_partical.x = new ParticleSystem.MinMaxCurve(1.5f, curve);


        //  rigidBody = this.GetComponent<Rigidbody>();
        // rigidBody.AddForce(Vector3.forward * 100);



    }
}
