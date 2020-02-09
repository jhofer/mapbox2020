using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mapbox.Unity.MeshGeneration.Components;
using UnityEngine;

public class Building : MonoBehaviour, IEntity, ISelectable
{
    private static Building selectedBuilding = null;
    private bool isOwned = false;
    private bool isEnemy = false;
    private Outline outline;

    public void Claim()
    {
        ResetSelection();
        this.isOwned = true;
    }

    private FeatureBehaviour featureBehvaviour;
    private float volume;
    private BuildingType buildingType;
    private float buildingValue;

    public bool IsSelected { get => this == selectedBuilding; }
    public static Building Selected { get => selectedBuilding; }

    void Start()
    {
        this.outline = this.GetComponent<Outline>();
        this.featureBehvaviour = this.GetComponent<FeatureBehaviour>();
        this.outline.enabled = false;

        var mesh = featureBehvaviour.VectorEntity.Mesh;
        this.volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
        this.buildingType = BuildingTypes.GetType(featureBehvaviour.Data.Properties["type"].ToString());
        this.buildingValue = buildingType.multiplier * volume;
    }

    // Update is called once per frame
    void Update()
    {

        var range = CamMovement.Instance.maxCamHeight;
        var outlineRange = 5;
        var outlineWidth = (outlineRange / range) * CamMovement.Instance.currentHeight;
        this.outline.OutlineWidth = Math.Max(2, outlineWidth);
        if (IsSelected)
        {
            this.outline.OutlineColor = Color.yellow;
            this.outline.enabled = true;

            var buffer = new StringBuilder();
            buffer.AppendLine("====VecctorEntity-Feature.Props===");
            foreach (var prop in featureBehvaviour.VectorEntity.Feature.Properties)
            { 
            buffer.AppendLine(prop.Key + ": " + prop.Value);
            }
            buffer.AppendLine("===Data.Properties====");
            foreach (var prop in featureBehvaviour.Data.Properties)
            {
                buffer.AppendLine(prop.Key + ": " + prop.Value);
            }
            Debug.Log(buffer.ToString());

        }
        else if (this.isOwned)
        {
            this.outline.OutlineColor = Color.green;
            this.outline.enabled = true;
        }
        else if(this.isEnemy)
        {
            this.outline.OutlineColor = Color.red;
            this.outline.enabled = true;
        }
        else
        {
            this.outline.enabled = false;
        }
    }

    public static void ResetSelection()
    {
        selectedBuilding = null;
    }

    public void Select()
    {
        if (IsSelected)
        {
            DialogHandler.Instance.building = null;
            selectedBuilding = null;
        }
        else
        {
            DialogHandler.Instance.building = this;
            selectedBuilding = this;
        }
      
       
    }
}
