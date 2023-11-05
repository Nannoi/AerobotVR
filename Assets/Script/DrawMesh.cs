using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;


public class DrawMesh : MonoBehaviour
{
    public GameObject[] loc ;
    private MeshFilter meshFilter;

    private string robotId;
    //private List<GameObject> locs = new List<GameObject>();
    private int numRobots;
    private GameObject neighbour;


    // Start is called before the first frame update
    public void Initialize(string robotId, int numRobots,GameObject neighbour, GameObject[] loc)
    {
        this.robotId = robotId;
        // this.neighbourId = neighbourId;
        // this.locs = locs;
        this.numRobots = numRobots;
        this.neighbour = neighbour;
        this.loc = loc;

    }
    
    public void Start()
    {

        meshFilter = GetComponent<MeshFilter>();
        
       
    }

    // Update is called once per frame
    void Update()
    {

        UpdateFabricMesh();
    }

    public void CreateFabricMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] newVertices = new Vector3[loc.Length];
        Vector2[] uvs = new Vector2[loc.Length];
        Vector3[] normals = new Vector3[loc.Length];
        int[] newTris = GenerateTriangleStripIndices(loc.Length);

        for (int k = 0; k < loc.Length; k++)
        {
            newVertices[k] = loc[k].transform.position;
        }

        for (int k = 0; k < loc.Length; k++)
        {
            uvs[k] = new Vector2(newVertices[k].x, newVertices[k].y);
        }

        for (int m = 0; m < normals.Length; m++)
        {
            normals[m] = new Vector3(0f, 1f, 0f); // Set the default normal to (0, 1, 0) if needed
        }

        mesh.name = "Fabric" + robotId;
        mesh.vertices = newVertices;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = newTris;

        meshFilter.mesh = mesh;
    }

    public void ResetFabricMesh()
    {
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        meshFilter.mesh = mesh;

    }

    public void UpdateFabricMesh()
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = new Vector3[loc.Length];
        Vector2[] uvs = new Vector2[loc.Length];
        Vector3[] normals = new Vector3[loc.Length];

        if (meshFilter != null && meshFilter.mesh != null && loc != null && mesh.vertices != null)
        {
            for (int i = 0; i < loc.Length; i++)
            {
                vertices[i] = loc[i].transform.position;
            }

            for (int j = 0; j < loc.Length; j++)
            {
                uvs[j] = new Vector2(vertices[j].x, vertices[j].y);
            }

            // Calculate normals
            for (int k = 0; k < mesh.triangles.Length; k += 3)
            {
                int i1 = mesh.triangles[k];
                int i2 = mesh.triangles[k + 1];
                int i3 = mesh.triangles[k + 2];

                Vector3 side1 = vertices[i2] - vertices[i1];
                Vector3 side2 = vertices[i3] - vertices[i1];
                Vector3 normal = Vector3.Cross(side1, side2).normalized;

                normals[i1] += normal;
                normals[i2] += normal;
                normals[i3] += normal;
            }

            for (int m = 0; m < normals.Length; m++)
            {
                normals[m] = normals[m].normalized;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.normals = normals;

            meshFilter.mesh = mesh;
        }
        else
        {
            meshFilter.mesh = null;
        }
    }


    private int[] GenerateTriangleStripIndices(int vertexCount)
    {
        int[] tris = new int[(vertexCount - 2) * 3];
       
        int index = 0;

        for (int i = 0; i < vertexCount - 2; i++)
        {
            if (i % 2 == 0)
            {
                tris[index++] = i;
                tris[index++] = i + 1;
                tris[index++] = i + 2;
            }
            else
            {
                tris[index++] = i + 1;
                tris[index++] = i;
                tris[index++] = i + 2;
            }
          
        }

        return tris;
    }

    }