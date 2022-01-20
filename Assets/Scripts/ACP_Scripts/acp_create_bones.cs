using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acp_create_bones : MonoBehaviour
{
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        var transform = this.GetComponentsInChildren<Transform>();
        foreach(var t in transform){
            print(t);
            mesh = t.GetComponent<MeshFilter>().mesh;
            foreach (Vector3 v in mesh.vertices)
                //mesh_vertices.Add(v + gameObject.transform.position);
                print(v);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
