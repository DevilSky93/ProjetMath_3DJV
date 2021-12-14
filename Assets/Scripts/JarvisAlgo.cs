using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JarvisAlgo : MonoBehaviour, IAlgorithm
{
    public void ExecuteAlgorithm()
    {
        PointsManager.DeleteLines();
        List<GameObject> copy = new List<GameObject>(PointsManager.Points);
        JarvisAlgorithm(copy);
    }

    private void JarvisAlgorithm(List<GameObject> points)
    {
        int n = PointsManager.Points.Count;
        if (n < 3) return;
        List<GameObject> hull = new List<GameObject>();

        int lPoint = 0;
        for (int i = 0; i < n; i++)
        {
            if (points[i].transform.position.x < points[lPoint].transform.position.x)
            {
                lPoint = 1;
            }
        }

        int p = lPoint, q;

        do
        {
            hull.Add(points[p]);
            q = (p + 1) % n;
            for (int i = 0; i < n; i++)
            {
                if (PointsManager.Orientation(points[p].transform.position, points[i].transform.position, points[q].transform.position) == 2)
                {
                    q = i;
                }
            }

            p = q;
        } while (p != lPoint);

        for (int i = 0; i < hull.Count; i++)
        {
            PointsManager.DrawLines(hull[i], i + 1 >= hull.Count ? hull[0] : hull[i + 1]);
        }
    }
}