using System;
using System.Collections.Generic;


public static class BuildingTypes
{

    public static Dictionary<string, BuildingType> dict = new Dictionary<string, BuildingType>();
    public static BuildingType GetType(string mapboxType)
    {
        if (dict.TryGetValue(mapboxType,  out var btype))
        {
            return btype;
        }
        return BUILDING;
    }

    public static BuildingType BUILDING = new BuildingType("building",1);
    public static BuildingType COMMERCIAL = new BuildingType("commercial",2);
    public static BuildingType CONSTRUCTION = new BuildingType("construction", 2);
    public static BuildingType TRAIN_STATION = new BuildingType("train_station",20);
    public static BuildingType PUBLIC = new BuildingType("public",10);



}

public class BuildingType
{

    private string mapBoxType;
    public readonly int multiplier;

    public BuildingType(string mapboxType, int multiplier)
    {
        this.mapBoxType = mapboxType;
        this.multiplier = multiplier;
        BuildingTypes.dict.Add(mapboxType, this);
    }

  



}


