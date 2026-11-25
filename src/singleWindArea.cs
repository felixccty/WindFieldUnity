using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class singleWindArea : MonoBehaviour
{
    public float global_wind_dir_x=0;
    public float global_wind_dir_y = 0;
    public string csvFilePath = "D:/windData/1.txt"; // CSV文件路径
    // Start is called before the first frame update
    void Start()
    {
        ImportCSV();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
       // Rigidbody rigidBody;
        ParticleSystem ps;
        ps=other.gameObject.GetComponent<ParticleSystem>();
        //rigidBody = other.gameObject.GetComponent<Rigidbody>();

        //rigidBody.AddForce(this.transform.forward * 1000);
        //ps.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
        //Debug.Log(rigidBody);

    }
    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Debug.Log(ps);

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(255, 0, 0, 255);
            enter[i] = p;
        }
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
        }

        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        Debug.Log(ps);
    }

 
    void ImportCSV()
    {
        StreamReader reader = new StreamReader(csvFilePath);

        //string[,] arr = new string[80, 80];
        string[] arr = new string[80];

        int total_count = 0;
        float x_total = 0;
        float y_total = 0;
        while (!reader.EndOfStream)
        {
            
            string line = reader.ReadLine();
            line = line.Replace("-999.0:-999.0", "");
            int line_len = line.Length;
            if (line_len <= 80)
            {
                continue;
            }
            if (line != "")
            {
                string[] data = line.Split(','); // 按逗号分割每一行数据
                for(int i = 0; i < data.Length; i++)
                {
                    if (data[i] == "")
                    {
                        continue;
                    }
                    string[] data_xy= data[i].Split(':');
                    x_total = x_total + float.Parse(data_xy[0]);
                    y_total = y_total + float.Parse(data_xy[1]);
                    total_count++;
                    Debug.Log(x_total);
                    Debug.Log(y_total);
                    Debug.Log(total_count);
                }




                // 这里可以根据CSV文件的结构，将data[0]、data[1]等值保存到相应的变量中
                //Debug.Log("Column 1: " + data[0]);
                // Debug.Log("Column 2: " + data[1]);


            }
        }
        reader.Close();

        float avg_x_total = x_total / total_count;
        float avg_y_total = y_total / total_count;
        Debug.Log("算出的风向");
        Debug.Log(avg_x_total);
        Debug.Log(avg_y_total);

        global_wind_dir_x = avg_x_total;
        global_wind_dir_y = avg_y_total;




    }
}
