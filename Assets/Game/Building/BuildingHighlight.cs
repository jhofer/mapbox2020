using cakeslice;
using Mapbox.Unity.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHighlight : MonoBehaviour
{
    private bool focus;
    private float focusTime;
    private Outline outline;
    private static List<BuildingHighlight> buildingHightlights = new List<BuildingHighlight>();



    // Start is called before the first frame update
    void Start()
    {
        this.outline = this.GetComponent<Outline>();
        buildingHightlights.Add(this);


    }

    private static void ResetEveryFocus()
    {
        foreach (var building in buildingHightlights)
        {
            building.ResetFocus();
        }
    }

    private void ResetFocus()
    {
        this.focus = false;
    }

    void OnMouseDown()
    {
        Debug.Log("Click building");
        BuildingHighlight.ResetEveryFocus();
        this.focus = true;
        this.focusTime = Time.time;

    }


    // Update is called once per frame

}
