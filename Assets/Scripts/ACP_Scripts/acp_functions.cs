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

    List<Vector3> centrateDatas(List<Vector3> points, Vector3 barycenter)
    {
        List<Vector3> v_centrateDatas = new List<Vector3>();
        float x , y, z;
        foreach (Vector3 v in points)
        {
            x = v.x - barycenter.x;
            y = v.y - barycenter.y;
            z = v.z - barycenter.z;
            v_centrateDatas.Add(new Vector3(x, y, z));
        }
        return v_centrateDatas;
    }

    double[,] matrixCov(List<Vector3> points)
    {
        const int MATRIX_ROWS = 3;
        const int MATRIX_COLUMNS = 3;
        double[,] matCov = new double[MATRIX_ROWS, MATRIX_COLUMNS];
        double covariance;
        Vector3 barycenter = calculateBarycenter(points);

        for (int i = 0; i < MATRIX_ROWS; i++)
        {
            for (int j = 0; j < MATRIX_COLUMNS; j++)
            {
                covariance = 0.0;
                for (int n = 0; n<points.Count; n++)
                {
                    covariance += points[n][i] * points[n][j] - barycenter[j] * points[n][i] - barycenter[i] * points[n][j] + barycenter[i] * barycenter[j];
                }

                matCov[i, j] = covariance/points.Count;
            }
        }
        return matCov;
    }

    void testList()
    {
        Vector3 ground1p1 = new Vector3(1f, 1f, 1f);
        Vector3 ground1p2 = new Vector3(2f, 3f, 4f);
        Vector3 ground1p3 = new Vector3(2f, 6f, 1f);
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

        TestPoints = centrateDatas(TestPoints, barycenter);

        double[,] matCov = matrixCov(TestPoints);
        /*foreach (var cov in matCov)
        {
            Debug.Log(cov);
        }*/
        /*print(matCov[0, 0] + " " + matCov[0, 1] + " " + matCov[0, 2]);
        print(matCov[1, 0] + " " + matCov[1, 1] + " " + matCov[1, 2]);
        print(matCov[2, 0] + " " + matCov[2, 1] + " " + matCov[2, 2]);*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
