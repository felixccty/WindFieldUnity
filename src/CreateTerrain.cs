using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{

    public int N = 200;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    // 用来存放顶点数据
    List<Vector3> verts;
    List<int> indices;
    public float height = 30f;
    public Texture2D heightMap;


    public int NMap = 512;
    private void Awake()
    {
    }

    private void Start()
    {
        verts = new List<Vector3>();
        indices = new List<int>();

        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider= GetComponent<MeshCollider>();
        Generate();
    }

    public void Generate()
    {
        ClearMeshData();

        // 把数据填写好
        AddMeshData();

        // 把数据传递给Mesh，生成真正的网格
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        //mesh.uv = uvs.ToArray();
        mesh.triangles = indices.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        
    }

    void ClearMeshData()
    {
        verts.Clear();
        indices.Clear();
    }

    void AddMeshData()
    {


        Color32[] colors = heightMap.GetPixels32();
        //01填充顶点数据 
        for (int z = 0; z < N; ++z)//按先x后z的顶点排列顺序，所以先循环的是z
        {
            for (int x = 0; x < N; ++x)
            {
                int xx = Mathf.RoundToInt(x * 1.0f / N * NMap);
                int zz = Mathf.RoundToInt(z * 1.0f / N * NMap);


                float y = colors[zz * NMap + xx].r / 255f * height;
                Vector3 temp = new Vector3(x, y, z);
                verts.Add(temp);
            }
        }
        //02填充索引数据
        for (int z = 0; z < N - 1; ++z)
        {
            for (int x = 0; x < N - 1; ++x)
            {
                int index_lb = z * N + x;//index of the left bottom vertex. lb = left bottom
                int index_lt = (z + 1) * N + x;
                int index_rt = (z + 1) * N + x + 1;
                int index_rb = z * N + x + 1;

                indices.Add(index_lb); indices.Add(index_lt); indices.Add(index_rt);
                indices.Add(index_rt); indices.Add(index_rb); indices.Add(index_lb);
            }
        }


    }

    float lastUpdateTime = 0;
    private void Update()
    {
        if (Time.time >= lastUpdateTime + 0.1f)
        {
            //Generate();
            lastUpdateTime = Time.time;
        }
    }
}
