using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://www.youtube.com/watch?v=u__4SxMTbDk

public class JarvisAlgoV2 : MonoBehaviour, IAlgorithm
{
    public void MainAlgorithm(List<GameObject> points)
    {
        JarvisAlgorithm(points);
    }

    public void ExecuteAlgorithm()
    {
        PointsManager.DeleteLines();
        List<GameObject> copy = new List<GameObject>(PointsManager.Points);
        MainAlgorithm(copy);
    }

    private int lowestPointY(List<GameObject> points)
    {
        int lowest = 0;
        for (int i = 1; i < points.Count; i++)
        {
            if (points[lowest].transform.position.y > points[i].transform.position.y)
                lowest = i;
        }
        return lowest;
    }

    private void drawJarvis(List<GameObject> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            PointsManager.DrawLines(points[i], points[i + 1]);
        }
        PointsManager.DrawLines(points[points.Count - 1], points[0]);
    }

    private void JarvisAlgorithm(List<GameObject> points)
    {
        List<GameObject> hull = new List<GameObject>();
        hull.Add(points[lowestPointY(points)]);
        //print(PointsManager.Orientation(points[0].transform.position, points[1].transform.position, points[2].transform.position));

    }
}
