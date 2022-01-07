using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3
{
    public float[,] mat;

    public Matrix3x3()
    {
        this.mat = new float[3, 3];
    }

    public Vector3 multiplyVector(Vector3 v)
    {
        Vector3 result = new Vector3(0f, 0f, 0f);

        result.x = mat[0, 0] * v.x + mat[0, 1] * v.y + mat[0, 2] * v.z;
        result.y = mat[1, 0] * v.x + mat[1, 1] * v.y + mat[1, 2] * v.z;
        result.z = mat[2, 0] * v.x + mat[2, 1] * v.y + mat[2, 2] * v.z;

        return result;
    }

    public void display()
    {
        Debug.Log("___Matrice de covariance : ___");
        Debug.Log(mat[0, 0] + " " + mat[0, 1] + " " + mat[0, 2]);
        Debug.Log(mat[1, 0] + " " + mat[1, 1] + " " + mat[1, 2]);
        Debug.Log(mat[2, 0] + " " + mat[2, 1] + " " + mat[2, 2]);
        Debug.Log("______________________________");
    }
}
