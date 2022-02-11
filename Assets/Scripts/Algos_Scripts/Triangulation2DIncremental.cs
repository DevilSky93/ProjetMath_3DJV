﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangulation2DIncremental : MonoBehaviour, IAlgorithm
{
    public void MainAlgorithm(List<GameObject> points)
    {
        // Step 1 : sort by absciss ascending order 
        var copytmp = points.OrderBy(c => c.transform.position.y).ToList();
        var copy = copytmp.OrderBy(c => c.transform.position.x).ToList();

        // Step 2 
        for (int i = 0; i < 4; i++)
        {
            if (i+1 > 3)
            {
                PointsManager.DrawLines(copy[i], copy[0]);
            }
            else
            {
                PointsManager.DrawLines(copy[i], copy[i+1]);
            }
        }
    }

    public void ExecuteAlgorithm()
    {
        PointsManager.DeleteLines();
        List<GameObject> copy = new List<GameObject>(PointsManager.Points);
        MainAlgorithm(copy);
    }

    public void printPoints(List<GameObject> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            print(points[i].transform.position);
        }
    }
}