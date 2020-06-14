using System;
using System.Collections;
using System.Collections.Generic;
using Endgame.Domain;
using UnityEngine;

public class ConquerManager : MonoBehaviour
{
    MonoBehaviour map; 
    
    // Start is called before the first frame update
    void Start()
    {
        Hub.Instance.On<BuildingDto>("ConquerBuilding", ConquerBuilding);
    }

    private void ConquerBuilding(BuildingDto obj)
    {
        var buildings = this.map.GetComponentsInChildren<Building>();
        foreach (var b in buildings)
        {
            if(b.MapBoxId == obj.id)
            {
                b.UpdateBuilding(obj);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
