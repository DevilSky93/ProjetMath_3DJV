using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blender tuto https://www.youtube.com/watch?v=-QfLdbzSZdw

public class acp_functions : MonoBehaviour
{
    List<Vector3> TestPoints = new List<Vector3>();
    // Start is called before the first frame update

    void testList()
    {
        /*Vector3 ground1p1 = new Vector3(1f, 1f, 1f);
        Vector3 ground1p2 = new Vector3(2f, 3f, 4f);
        Vector3 ground1p3 = new Vector3(2f, 5f, 1f);
        Vector3 ground1p4 = new Vector3(1f, 3f, 4f);*/
        // D'après DCode : 
        //λ1 = 2.25 -> {0.,0.,1.}
        //λ2 = 2.13278 -> {0.256668,0.9665,0.}
        //λ3 = 0.117218 -> {−0.9665,0.256668,0.}

        Vector3 ground1p1 = new Vector3(2f, 1f, 2f);
        Vector3 ground1p2 = new Vector3(3f, 3f, 4f);
        Vector3 ground1p3 = new Vector3(2f, 5f, 1f);
        Vector3 ground1p4 = new Vector3(1f, 3f, 4f);
        // D'après DCode : 
        //λ1 = 2.3676 -> {0.,−0.805691,0.592336}
        //λ2 = 1.3199 -> {0.,0.592336,0.805691}
        //λ3 = 0.5 -> {1.,0.,0.}

        //TODO Erreur avec ce dataset
        /*Vector3 ground1p1 = new Vector3(6f, 2f, 4f);
        Vector3 ground1p2 = new Vector3(4f, 6f, 2f);
        Vector3 ground1p3 = new Vector3(6f, 4f, 2f);
        Vector3 ground1p4 = new Vector3(4f, 4f, 4f);*/
        // D'après DCode : 
        //λ1=3→{1,−2,1}
        //λ2=1→{−1,0,1}
        //λ3=0→{1,1,1}

        TestPoints.Add(ground1p1);
        TestPoints.Add(ground1p2);
        TestPoints.Add(ground1p3);
        TestPoints.Add(ground1p4);
    }

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

    List<Vector3> centrateDatas(List<Vector3> points)
    {
        Vector3 barycenter = calculateBarycenter(TestPoints);

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

    Matrix3x3 matrixCov(List<Vector3> points)
    {
        Matrix3x3 matCov = new Matrix3x3();
        double covariance;
        Vector3 barycenter = calculateBarycenter(points);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                covariance = 0.0;
                for (int n = 0; n<points.Count; n++)
                {
                    covariance += points[n][i] * points[n][j] - barycenter[j] * points[n][i] - barycenter[i] * points[n][j] + barycenter[i] * barycenter[j];
                }

                matCov.mat[i, j] = (float)covariance /points.Count;
            }
        }
        return matCov;
    }

    float maxAbsValue(Vector3 v3)
    {
        return Mathf.Max(Mathf.Max(Mathf.Abs(v3.x), Mathf.Abs(v3.y)), Mathf.Abs(v3.z));
    }

    Vector3 Eigenvector(Matrix3x3 matCov)
    {
        Vector3 v0 = new Vector3(0f, 0f, 1f);
        int ITERATIONS = 100;

        Vector3 vk = matCov.multiplyVector(v0);
        float lambdaK = maxAbsValue(vk);
        vk = (1 / lambdaK) * vk;

        for (int k=1; k<ITERATIONS; k++)
        {
            vk = matCov.multiplyVector(vk);
            lambdaK = maxAbsValue(vk);
            vk = (1 / lambdaK) * vk;
        }
        //print("lambda K : " + lambdaK);

        return vk.normalized;
    }

    List<Vector3> projectedDatas(List<Vector3> points, Vector3 eigenvector)
    {
        List<Vector3> projectedDatas = new List<Vector3>();
        float x, y, z;
        Vector3 prod_v = new Vector3();
        float prod;

        foreach (Vector3 A in points)
        {
            prod_v = Vector3.Scale(A, eigenvector);
            prod = prod_v.x + prod_v.y + prod_v.z;
            x = prod * eigenvector.x;
            y = prod * eigenvector.y;
            z = prod * eigenvector.z;
            //print("projectedData : " + x + " " + y + " " + z);
            projectedDatas.Add(new Vector3(x, y, z));
        }

        return projectedDatas;
    }


    void Start()
    {
        testList();

        TestPoints = centrateDatas(TestPoints);

        Matrix3x3 matCov = matrixCov(TestPoints);

        //matCov.display();

        Vector3 eigenvector = Eigenvector(matCov);

        print("eigenvector : " + eigenvector.x + " " + eigenvector.y + " " + eigenvector.z);

        List<Vector3> TestProjections = projectedDatas(TestPoints, eigenvector);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
