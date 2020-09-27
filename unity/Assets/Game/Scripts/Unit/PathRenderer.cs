using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    private MovementController movmentCtrl;
    private LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        this.movmentCtrl = gameObject.GetComponent<MovementController>();
        this.lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
       

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

    }

    // Update is called once per frame
    void Update()
    {

        if(movmentCtrl.futurePositions != null)
        {
            var path = new List<Vector3>();
            path.Add(transform.position);
            path.AddRange(movmentCtrl.futurePositions);
            path = path.Select(v => v+Vector3.up*1f).ToList();
            var points = path.ToArray();
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
     
    }
}
