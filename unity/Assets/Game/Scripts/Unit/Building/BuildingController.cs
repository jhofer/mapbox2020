using System;
using Endgame.DTOs;
using Mapbox.Unity.MeshGeneration.Components;
using UnityEngine;

public class BuildingController : MonoBehaviour, ISelectable, IEntity
{
   
    private bool IsOwned  { get => this.dto.userId != null && this.dto.userId == Hub.Instance.UserId; }
    private bool IsEnemy  { get => this.dto.userId != null && this.dto.userId != Hub.Instance.UserId; }
    private Outline outline;
    private FeatureBehaviour featureBehvaviour;
   
    private BuildingDto dto;

    public bool IsSelected { get => SelectionHandler.Instance.IsSelected(this); }

    public void UpdateBuilding(BuildingDto obj)
    {
        dto = obj;

    }

   
    
    public string MapBoxId { get => this.dto.id; }
    public double BuildingValue { get => this.dto.value; }

    public void Claim()
    {
      
        EventBus.Instance.Publish<ClaimBuildingRequest>(new ClaimBuildingRequest(this.MapBoxId));
        
      
    }



    void Start()
    {
        this.outline = this.GetComponent<Outline>();
        this.featureBehvaviour = this.GetComponent<FeatureBehaviour>();
        this.outline.enabled = false;

        var mesh = featureBehvaviour.VectorEntity.Mesh;
        var volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
        var id = featureBehvaviour.Data.Data.Id;
        var buildingType = BuildingTypes.GetType(featureBehvaviour.Data.Properties["type"].ToString());
        this.dto = new BuildingDto()
        {
            id = id.ToString(),
            buildingType = buildingType.ToString(),
            volume = volume,
            value = Math.Round(buildingType.multiplier * volume)
        };
     
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
        }
        else if (this.IsOwned)
        {
            this.outline.OutlineColor = Color.green;
            this.outline.enabled = true;
        }
        else if(this.IsEnemy)
        {
            this.outline.OutlineColor = Color.red;
            this.outline.enabled = true;
        }
        else
        {
            this.outline.enabled = false;
        }
    }

    public void Select()
    {
        SelectionHandler.Instance.Select(this);
    }
}
