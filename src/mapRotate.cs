using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class mapRotate : MonoBehaviour
{
    // Start is called before the first frame update
    //public float wind_dir = 0;
    public float wind_speed = 20;
    //public float global_wind_dir_x = 0;
    //public float global_wind_dir_y = 0;
    Text rootFilePath;
    GameObject text_obj;
    string rootFilePath_text = "";
    string csvFilePath_a = ""; // CSV文件路径
    
    void Start()
    {
        //get_path();
        //string final_file_name=GetFilePath(rootFilePath);
        wind_speed = 10;
        text_obj = GameObject.Find("Text_path");

        rootFilePath = text_obj.GetComponent<Text>();
        rootFilePath_text = rootFilePath.text;
        csvFilePath_a = GetAllFiles();

        //获取设置当前屏幕分辩率 
        Resolution[] resolutions = Screen.resolutions;
        //设置当前分辨率 
        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

        Screen.fullScreen = true;  //设置成全屏
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //wind_dir = wind_dir + 30;

        text_obj = GameObject.Find("Text_path");

        rootFilePath = text_obj.GetComponent<Text>();
        rootFilePath_text = rootFilePath.text;
        csvFilePath_a = GetAllFiles();
        ImportCSV();
        Debug.Log(csvFilePath_a);
    }

    void get_path()
    {
        string folderPath = rootFilePath_text; // 将此处替换为要查询的文件夹路径

        DirectoryInfo directory = new DirectoryInfo(folderPath);
        FileSystemInfo[] filesAndFolders = directory.GetFileSystemInfos();

        if (filesAndFolders.Length > 0)
        {
            FileSystemInfo lastItem = filesAndFolders[filesAndFolders.Length - 1];

           // Debug.Log("最后一项名称：" + lastItem.Name);
           // Debug.Log("最后一项类型：" + lastItem.Extension);
           // Debug.Log("最后一项完整路径：" + lastItem.FullName);
        }
        else
        {
            Debug.LogError("该文件夹内没有任何项。");
        }

    }

    public static string GetFilePath(string path)
    {
        if (Directory.Exists(path))
        {
            //文件路径
            string[] dir = Directory.GetDirectories(path);
            //文件名
            string[] names = new string[dir.Length];
            for (int i = 0; i < dir.Length; i++)
            {
                //赋值文件命名
                names[i] = Path.GetFileName(dir[i]);
            }
            return names[dir.Length-1];
        }
        else
        {
            Debug.LogError("未找到路径");
            return null;
        }
    }
    public string GetAllFiles()
    {

        string[] files = Directory.GetFiles(rootFilePath_text, "*.*", SearchOption.AllDirectories);
        string final_path = files[(files.Length)-2];

        return final_path;
    }

    void ImportCSV()
    {
        StreamReader reader = new StreamReader(csvFilePath_a);

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
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == "")
                    {
                        continue;
                    }
                    string[] data_xy = data[i].Split(':');
                    x_total = x_total + float.Parse(data_xy[0]);
                    y_total = y_total + float.Parse(data_xy[1]);
                    total_count++;
                    //Debug.Log(x_total);
                    //Debug.Log(y_total);
                    //Debug.Log(total_count);
                }

                // 这里可以根据CSV文件的结构，将data[0]、data[1]等值保存到相应的变量中
                //Debug.Log("Column 1: " + data[0]);
                // Debug.Log("Column 2: " + data[1]);


            }
        }
        reader.Close();

        float avg_x_total = x_total / total_count;
        float avg_y_total = y_total / total_count;
        wind_speed= Mathf.Abs(Mathf.Tan(avg_x_total / avg_y_total));
        //Debug.Log("算出的风向");
        //Debug.Log(avg_x_total);
        //Debug.Log(avg_y_total);
        //Debug.Log(total_count);
        float rotate_angle=Mathf.Atan(avg_x_total/ avg_y_total);
        //Debug.Log(rotate_angle);
        this.gameObject.transform.localEulerAngles = new Vector3(0, rotate_angle, 0);

        //global_wind_dir_x = avg_x_total;
        //global_wind_dir_y = avg_y_total;
    }

}
