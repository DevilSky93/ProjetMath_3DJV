﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blender tuto https://www.youtube.com/watch?v=-QfLdbzSZdw

[RequireComponent(typeof(MeshFilter))]
public class acp_mesh : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> mesh_vertices = new List<Vector3>();
    public acp_functions acp_Functions;
    public GameObject GO_point_min;
    public GameObject GO_point_max;
    public GameObject GO_point_black;
    private (Vector3, Vector3) _segmentMinMax;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this._segmentMinMax.Item1, this._segmentMinMax.Item2);
    }

    void Start()
    {
        //_____INIT______

        mesh = gameObject.GetComponent<MeshFilter>().mesh;

        //Position
        foreach (Vector3 v in mesh.vertices)
            mesh_vertices.Add(v + gameObject.transform.position);

        //Rotation
        Renderer rend = gameObject.GetComponent<Renderer>();
        Vector3 center = rend.bounds.center;
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(gameObject.transform.localRotation.eulerAngles.x, gameObject.transform.localRotation.eulerAngles.y, gameObject.transform.localRotation.eulerAngles.z);
        for (int i=0; i<mesh_vertices.Count; i++)
        {
            mesh_vertices[i] = newRotation * (mesh_vertices[i] - center) + center;
            if(GO_point_black != null)
                Instantiate(GO_point_black, mesh_vertices[i], Quaternion.identity);
        }


        //______ACP______

        mesh_vertices = acp_Functions.centrateDatas(mesh_vertices);

        Matrix3x3 matCov = acp_Functions.matrixCov(mesh_vertices);

        //matCov.display();

        Vector3 eigenvector = acp_Functions.Eigenvector(matCov);

        //print("eigenvector : " + eigenvector.x + " " + eigenvector.y + " " + eigenvector.z);

        List<Vector3> mesh_vertices_projections = acp_Functions.projectedDatas(mesh_vertices, eigenvector);

        _segmentMinMax = acp_Functions.projectedDatasExtremes(mesh_vertices, mesh_vertices_projections, eigenvector);

        print("min : " + _segmentMinMax.Item1.x + " " + _segmentMinMax.Item1.y + " " + _segmentMinMax.Item1.z);
        print("max : " + _segmentMinMax.Item2.x + " " + _segmentMinMax.Item2.y + " " + _segmentMinMax.Item2.z);

        Instantiate(GO_point_min, _segmentMinMax.Item1, Quaternion.identity);
        Instantiate(GO_point_max, _segmentMinMax.Item2, Quaternion.identity);
    }

    void Update()
    {
        
    }
}