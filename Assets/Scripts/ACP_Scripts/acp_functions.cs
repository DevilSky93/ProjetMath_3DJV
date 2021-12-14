using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acp_functions : MonoBehaviour
{
    List<Vector3> TestPoints = new List<Vector3>();
    // Start is called before the first frame update

    Vector3 calculateBarycenter(List<Vector3> points)
    {
        float bary_x=0f, bary_y = 0f, bary_z = 0f;
        float somme_x = 0f, somme_y = 0f, somme_z = 0f;
        int nb_points = points.Count;

        foreach(Vector3 v in points)
        {
            somme_x += v.x;
            somme_y += v.y;
            somme_z += v.z;
        }

        bary_x = somme_x / nb_points;
        bary_y = somme_y / nb_points;
        bary_z = somme_z / nb_points;

        return new Vector3(bary_x, bary_y, bary_z);
    }

    void testList()
    {
        Vector3 ground1p1 = new Vector3(1f, 1f, 1f);
        Vector3 ground1p2 = new Vector3(2f, 3f, 4f);
        Vector3 ground1p3 = new Vector3(2f, 1f, 1f);
        Vector3 ground1p4 = new Vector3(1f, 3f, 4f);
        TestPoints.Add(ground1p1);
        TestPoints.Add(ground1p2);
        TestPoints.Add(ground1p3);
        TestPoints.Add(ground1p4);
    }

    void Start()
    {
        testList();
        
        Vector3 barycenter = calculateBarycenter(TestPoints);
        print(barycenter);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
