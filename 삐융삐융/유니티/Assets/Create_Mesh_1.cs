using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Mesh_1 : MonoBehaviour
{
    public Material[] m_gm_Ground;

    // 단일 메쉬
    Vector3[] vVertices = new Vector3[3];
    Vector2[] vUvs = new Vector2[3];
    int[] nTriangles = new int[3];

    // 양면 메쉬
    Vector3[] vVertices_2 = new Vector3[6];
    Vector2[] vUvs_2 = new Vector2[6];
    int[] nTriangles_2 = new int[6];

    // num_ground == 0 : ■ ■ ■ │ num_ground == 1 : □ □ ■
    //                   ■ ■ □ │                   □ ■ ■
    //                   ■ □ □ │                   ■ ■ ■
    public void Create_Mesh_1_1(int num_ground, Vector3[] v_vertices)
    {
        Mesh mesh = new Mesh();

        if (num_ground == 0)
        {
            vUvs[0] = new Vector2(0f, 0f);
            vUvs[1] = new Vector2(0f, 1f);
            vUvs[2] = new Vector2(1f, 1f);
        }
        else
        {
            vUvs[0] = new Vector2(0f, 0f);
            vUvs[1] = new Vector2(1f, 1f);
            vUvs[2] = new Vector2(1f, 0f);
        }

        nTriangles[0] = 0;
        nTriangles[1] = 1;
        nTriangles[2] = 2;

        mesh.vertices = v_vertices;
        mesh.uv = vUvs;
        mesh.triangles = nTriangles;
        mesh.RecalculateNormals();

        this.gameObject.GetComponent<MeshRenderer>().material = m_gm_Ground[1];
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;

        this.gameObject.name = "Map Mesh(" + v_vertices[0].x + "," + v_vertices[0].y + ", " + v_vertices[0].z + ")";
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        this.gameObject.transform.SetParent(GameObject.Find("Map Mesh GameObject Set").transform);

        Destroy(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }
    public void Create_Mesh_Back_1_1(int num_ground, Vector3[] v_vertices)
    {
        Mesh mesh_back = new Mesh();

        if (num_ground == 0)
        {
            vUvs[0] = new Vector2(0f, 1f);
            vUvs[1] = new Vector2(1f, 1f);
            vUvs[2] = new Vector2(1f, 0f);

            //vUvs[0] = new Vector2(0f, 0f);
            //vUvs[1] = new Vector2(1f, 0f);
            //vUvs[2] = new Vector2(0f, -1f);
        }
        else
        {
            vUvs[0] = new Vector2(0f, 0f);
            vUvs[1] = new Vector2(0f, 1f);
            vUvs[2] = new Vector2(1f, 0f);

            //vUvs[0] = new Vector2(0f, 0f);
            //vUvs[1] = new Vector2(0f, 1f);
            //vUvs[2] = new Vector2(1f, 0f);
        }

        nTriangles[0] = 2;
        nTriangles[1] = 1;
        nTriangles[2] = 0;

        mesh_back.vertices = v_vertices;
        mesh_back.uv = vUvs;
        mesh_back.triangles = nTriangles;
        mesh_back.RecalculateNormals();

        this.gameObject.GetComponent<MeshRenderer>().material = m_gm_Ground[1];
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh_back;

        this.gameObject.name = "Map Mesh_Back(" + v_vertices[0].x + "," + v_vertices[0].y + ", " + v_vertices[0].z + ")";
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        this.gameObject.transform.SetParent(GameObject.Find("Map Mesh GameObject Set").transform);

        Destroy(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }

    public void Create_Mesh_Total_1_1(int num_ground, Vector3[] v_vertices)
    {
        Mesh mesh = new Mesh();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vVertices_2[(i * 3 )+ j] = v_vertices[j];
            }
        }

        if (num_ground == 0)
        {
            vUvs_2[0] = new Vector2(0f, 0f);
            vUvs_2[1] = new Vector2(0f, 1f);
            vUvs_2[2] = new Vector2(1f, 1f);

            vUvs_2[3] = new Vector2(0f, 1f);
            vUvs_2[4] = new Vector2(1f, 1f);
            vUvs_2[5] = new Vector2(1f, 0f);
        }
        else
        {
            vUvs_2[0] = new Vector2(0f, 0f);
            vUvs_2[1] = new Vector2(1f, 1f);
            vUvs_2[2] = new Vector2(1f, 0f);

            vUvs_2[3] = new Vector2(0f, 0f);
            vUvs_2[4] = new Vector2(0f, 1f);
            vUvs_2[5] = new Vector2(1f, 0f);
        }

        nTriangles_2[0] = 0;
        nTriangles_2[1] = 1;
        nTriangles_2[2] = 2;

        nTriangles_2[3] = 5;
        nTriangles_2[4] = 4;
        nTriangles_2[5] = 3;

        mesh.vertices = vVertices_2;
        mesh.uv = vUvs_2;
        mesh.triangles = nTriangles_2;
        mesh.RecalculateNormals();

        this.gameObject.GetComponent<MeshRenderer>().material = m_gm_Ground[1];
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;

        this.gameObject.name = "Map Mesh(" + v_vertices[0].x + "," + v_vertices[0].y + ", " + v_vertices[0].z + ")";
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        this.gameObject.transform.SetParent(GameObject.Find("Map Mesh GameObject Set").transform);

        Destroy(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }

    public void Change()
    {
        //this.gameObject.GetComponent<MeshFilter>().mesh.uv[0] = new Vector2(0f, 1f);
        //this.gameObject.GetComponent<MeshFilter>().mesh.uv[0] = new Vector2(1f, 1f);
        //this.gameObject.GetComponent<MeshFilter>().mesh.uv[2] = new Vector2(1f, 0f);
        //this.gameObject.GetComponent<MeshFilter>().mesh.triangles[0] = 2;
        //this.gameObject.GetComponent<MeshFilter>().mesh.triangles[1] = 1;
        //this.gameObject.GetComponent<MeshFilter>().mesh.triangles[2] = 0;
    }
}
