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
    //More types: https://docs.mapbox.com/vector-tiles/reference/mapbox-streets-v8/
    public static BuildingType BUILDING = new BuildingType("building",1);
    public static BuildingType PUBLIC = new BuildingType("public", 4);
    public static BuildingType TRAIN_STATION = new BuildingType("train_station",5);
    public static BuildingType SCHOOL = new BuildingType("school", 6);
    public static BuildingType HOSPITAL = new BuildingType("hospital", 6);
    public static BuildingType UNIVERSITY = new BuildingType("university", 6);




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


