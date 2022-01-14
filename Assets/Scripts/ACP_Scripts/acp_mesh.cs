using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blender tuto https://www.youtube.com/watch?v=-QfLdbzSZdw

[RequireComponent(typeof(MeshFilter))]
public class acp_mesh : MonoBehaviour
{
    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        int cpt = 0;
        foreach(Vector3 v in mesh.vertices)
        {
            //print(v);
            cpt++;
        }
        print(cpt);
    }

    void Update()
    {
        
    }
}
