using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static List<GameObject> Points = new List<GameObject>();
    public static List<LineRenderer> lineRenderers = new List<LineRenderer>();

    private static List<IAlgorithm> _algorithmes;

    public enum AlgoChoice
    {
        GrahamScan = 0,
        Jarvis = 1
    }

    public static AlgoChoice Choice;

    private void Start()
    {
        _algorithmes = FindObjectsOfType<MonoBehaviour>().OfType<IAlgorithm>().ToList();
    }

    public static int Orientation(Vector3 p, Vector3 q, Vector3 r)
    {
        int val = (int)((q.y - p.y) * (r.x - q.x) -
                        (q.x - p.x) * (r.y - q.y));
        if (val == 0) return 0;
        return (val > 0) ? 1 : 2;
    }
    
    //Initialize lines to draw
    private static void InitLines(LineRenderer lr)
    {
        lr.name = "Segment";
        lr.startWidth = .1f;
        lr.endWidth = .1f;
        lr.sortingOrder = -1;
        lr.material = new Material(Shader.Find("Sprites/Default")) { color = Color.black };
    }

    public static void DrawLines(GameObject p1, GameObject p2)
    {
        var go = new GameObject();
        var lr = go.AddComponent<LineRenderer>();
        InitLines(lr);
        lr.SetPosition(0, p1.transform.position);
        lr.SetPosition(1, p2.transform.position);
        
        PointsManager.lineRenderers.Add(lr);
    }

    public static void ExecuteAlgorithm()
    {
        _algorithmes[(int)Choice].ExecuteAlgorithm();
    }

    public static void DeleteLines()
    {
        if (lineRenderers.Count <= 0) return;
        for (int i = lineRenderers.Count-1; i >= 0; i--)
        {
            Destroy(lineRenderers[i].gameObject);
            lineRenderers.RemoveAt(i);
        }
    }
    
    public static void DeletePoints()
    {
        if (Points.Count <= 0) return;
        for (int i = Points.Count-1; i >= 0; i--)
        {
            Destroy(Points[i].gameObject);
            Points.RemoveAt(i);
        }
    }
}