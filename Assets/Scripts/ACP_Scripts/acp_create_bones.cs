using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acp_create_bones : MonoBehaviour
{
    public acp_functions acp_Functions;
    List<Transform> _trans = new List<Transform>();
    List<MeshFilter> _meshFilter = new List<MeshFilter>();
    List<(Vector3, Vector3)> _segmentMinMax = new List<(Vector3, Vector3)>();
    int _numberLimbs;

    public GameObject GO_point_min;
    public GameObject GO_point_max;
    public GameObject GO_point_black;
    public GameObject GO_point_barycenter;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (_segmentMinMax.Count != 0)
        {
            foreach ((Vector3, Vector3) seg in _segmentMinMax)
                Gizmos.DrawLine(seg.Item1, seg.Item2);
        }
    }

    List<Vector3> fillMeshVertices(Transform trans, MeshFilter meshfilter)
    {
        List<Vector3> m_v = new List<Vector3>();
        //Position
        foreach (Vector3 v in meshfilter.mesh.vertices)
            m_v.Add(v + trans.position);
        //Rotation
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = trans.localRotation.eulerAngles;
        for (int i = 0; i < m_v.Count; i++)
        {
            m_v[i] = newRotation * (m_v[i] - trans.position) + trans.position;
            //if (GO_point_black != null)
                //Instantiate(GO_point_black, m_v[i], Quaternion.identity);
        }
        return m_v;
    }

    void ACP_CreateSegment(List<Vector3> mesh_vertices, int i)
    {
        Vector3 barycenter = acp_Functions.calculateBarycenter(mesh_vertices);

        Matrix3x3 matCov = acp_Functions.matrixCov(mesh_vertices, barycenter);

        //matCov.display();

        mesh_vertices = acp_Functions.centrateDatas(mesh_vertices, barycenter);

        Vector3 eigenvector = acp_Functions.Eigenvector(matCov);

        //print("eigenvector : " + eigenvector.x + " " + eigenvector.y + " " + eigenvector.z);

        List<Vector3> mesh_vertices_projections = acp_Functions.projectedDatas(mesh_vertices, eigenvector);

        _segmentMinMax.Add(acp_Functions.projectedDatasExtremes(mesh_vertices, mesh_vertices_projections, eigenvector, barycenter));

        //print("min : " + _segmentMinMax[i].Item1.x + " " + _segmentMinMax[i].Item1.y + " " + _segmentMinMax[i].Item1.z);
        //print("max : " + _segmentMinMax[i].Item2.x + " " + _segmentMinMax[i].Item2.y + " " + _segmentMinMax[i].Item2.z);

        Instantiate(GO_point_min, _segmentMinMax[i].Item1, Quaternion.identity);
        Instantiate(GO_point_max, _segmentMinMax[i].Item2, Quaternion.identity);
    }


    // Start is called before the first frame update
    void Start()
    {
        _numberLimbs = gameObject.transform.childCount;
        _trans = new List<Transform>();
        _meshFilter = new List<MeshFilter>();
        _segmentMinMax = new List<(Vector3, Vector3)>();

        //All childs
        for (int i = 0; i < _numberLimbs; i++)
        {
            //Child's transform
            Transform trans = gameObject.transform.GetChild(i);
            _trans.Add(trans);

            //Childs'mesh
            MeshFilter meshFilter;
            if (trans.TryGetComponent(out meshFilter))
            {
                _meshFilter.Add(meshFilter);
            }
            else
            {
                _meshFilter.Add(null);
            }

            //Create Segments
            List<Vector3> mesh_vertices = new List<Vector3>();
            if (_meshFilter[i] != null)
            {
                mesh_vertices = fillMeshVertices(_trans[i], _meshFilter[i]);
                ACP_CreateSegment(mesh_vertices, i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
