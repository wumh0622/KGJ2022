using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDrawer : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    PolygonCollider2D collider2D;
    LineRenderer line;

    public float scanShowTime;
    private void Awake()
    {
        collider2D = GetComponent<PolygonCollider2D>();
        line = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = new Vector3[collider2D.points.Length];

        for (int idx = 0; idx < collider2D.points.Length; idx++)
        {
            points[idx] = (collider2D.points[idx] * transform.lossyScale) + (Vector2)transform.position;
        }
        line.positionCount = collider2D.points.Length;
        line.SetPositions(points);
        line.loop = true;
        line.enabled = false;

    }

    public void ShowScanEffect()
    {
        line.enabled = true;
        SimpleTimerManager.Instance.RunTimer(ShowScanEffectEnd, scanShowTime);
    }
    void ShowScanEffectEnd()
    {
        line.enabled = false;
    }
}
