using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acp_create_bones : MonoBehaviour
{
    public acp_functions acp_Functions;
    Transform[] _trans;
    Mesh[] _mesh;
    List<Vector3> mesh_vertices;
    private (Vector3, Vector3) _segmentMinMax;

    public GameObject GO_point_min;
    public GameObject GO_point_max;
    public GameObject GO_point_black;
    public GameObject GO_point_barycenter;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this._segmentMinMax.Item1, this._segmentMinMax.Item2);
    }

    List<Vector3> fillMeshVertices()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        Transform trans;
        for (int go =0; go < gameObject.transform.childCount; go++)
        {
            trans = gameObject.transform.GetChild(go);
            MeshFilter meshfilter;
            mesh_vertices = new List<Vector3>();
            if (trans.TryGetComponent(out meshfilter))
            {
                //Position
                foreach (Vector3 v in meshfilter.mesh.vertices)
                    mesh_vertices.Add(v + trans.position);
                //Rotation
                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = trans.localRotation.eulerAngles;
                for (int i = 0; i < mesh_vertices.Count; i++)
                {
                    mesh_vertices[i] = newRotation * (mesh_vertices[i] - trans.position) + trans.position;
                    if (GO_point_black != null)
                        Instantiate(GO_point_black, mesh_vertices[i], Quaternion.identity);
                }
            }
            Vector3 barycenter = acp_Functions.calculateBarycenter(mesh_vertices);

            Matrix3x3 matCov = acp_Functions.matrixCov(mesh_vertices, barycenter);

            matCov.display();

            mesh_vertices = acp_Functions.centrateDatas(mesh_vertices, barycenter);

            Vector3 eigenvector = acp_Functions.Eigenvector(matCov);

            print("eigenvector : " + eigenvector.x + " " + eigenvector.y + " " + eigenvector.z);

            List<Vector3> mesh_vertices_projections = acp_Functions.projectedDatas(mesh_vertices, eigenvector);

            _segmentMinMax = acp_Functions.projectedDatasExtremesSAMI(mesh_vertices, mesh_vertices_projections, eigenvector, barycenter);

            print("min : " + _segmentMinMax.Item1.x + " " + _segmentMinMax.Item1.y + " " + _segmentMinMax.Item1.z);
            print("max : " + _segmentMinMax.Item2.x + " " + _segmentMinMax.Item2.y + " " + _segmentMinMax.Item2.z);

            Instantiate(GO_point_min, _segmentMinMax.Item1, Quaternion.identity);
            Instantiate(GO_point_max, _segmentMinMax.Item2, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
