using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acp_create_bones : MonoBehaviour
{
    public acp_functions acp_Functions;
    public float _epsilon;
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
    }

    float SqrDistance(Vector3 p1, Vector3 p2)
    {
        float x = p2.x - p1.x;
        float y = p2.y - p1.y;
        float z = p2.z - p1.z;
        return x * x + y * y + z * z;
    }

    bool adjustJoint(int indexSeg1, int indexSeg2, float epsilon)
    {
        List<Vector3> seg1List = new List<Vector3>();
        seg1List.Add(_segmentMinMax[indexSeg1].Item1);
        seg1List.Add(_segmentMinMax[indexSeg1].Item2);
        List<Vector3> seg2List = new List<Vector3>();
        seg2List.Add(_segmentMinMax[indexSeg2].Item1);
        seg2List.Add(_segmentMinMax[indexSeg2].Item2);
        int indexClosestSeg1 = 0;
        int indexClosestSeg2 = 0;

        float minDistance = float.MaxValue;
        float distance;

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                distance = SqrDistance(seg1List[i], seg2List[j]);
                if (minDistance > distance)
                {
                    indexClosestSeg1 = i;
                    indexClosestSeg2 = j;
                    minDistance = distance;
                }
            }
        }



        if (minDistance < epsilon)
        {
            Vector3 center = seg1List[indexClosestSeg1] + (seg2List[indexClosestSeg2] - seg1List[indexClosestSeg1]) / 2;
            if (indexClosestSeg1 == 0)
                _segmentMinMax[indexSeg1] = (center, _segmentMinMax[indexSeg1].Item2);
            else
                _segmentMinMax[indexSeg1] = (_segmentMinMax[indexSeg1].Item1, center);

            if (indexClosestSeg2 == 0)
                _segmentMinMax[indexSeg2] = (center, _segmentMinMax[indexSeg2].Item2);
            else
                _segmentMinMax[indexSeg2] = (_segmentMinMax[indexSeg2].Item1, center);
            return true;
        }
        else
            return false;
    }

    void makeRig()
    {
        adjustJoint(0, 1, _epsilon);
        adjustJoint(2, 3, _epsilon);
        adjustJoint(4, 5, _epsilon);
        adjustJoint(6, 7, _epsilon);
        adjustJoint(6, 7, _epsilon);
        adjustJoint(7, 8, _epsilon);
        adjustJoint(9, 10, _epsilon);
        adjustJoint(10, 11, _epsilon);
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
            print(i + " " + trans);
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

        makeRig();

        for(int i=0; i<_segmentMinMax.Count; i++)
        {
            Instantiate(GO_point_min, _segmentMinMax[i].Item1, Quaternion.identity);
            Instantiate(GO_point_max, _segmentMinMax[i].Item2, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
