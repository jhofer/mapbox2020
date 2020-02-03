using Mapbox.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUtils : BaseSingleton<MapUtils>
{
    [SerializeField]
    public AbstractMap map;
    // Start is called before the first frame update
    public Vector3 GetVectorOnMap(double lat, double lon)
    {
        //get tile ID
        var tileIDUnwrapped = TileCover.CoordinateToTileId(new Mapbox.Utils.Vector2d(lat, lon), (int)map.Zoom);
        var isTileLoaded = map.MapVisualizer.ActiveTiles.ContainsKey(tileIDUnwrapped);
        var location = Conversions.GeoToWorldPosition(lat, lon, map.CenterMercator, map.WorldRelativeScale).ToVector3xz();
        float height = 0;

        if (isTileLoaded)
        {
            Debug.Log("Tile Is active load height");
            height = GetHeight(lat, lon, tileIDUnwrapped);
        }

        var withHeight = new Vector3(location.x, height, location.z);
            return withHeight;


    }

    private float GetHeight(double lat, double lon, UnwrappedTileId tileIDUnwrapped)
    {
        float height;
        //get tile
        UnityTile tile = map.MapVisualizer.GetUnityTileFromUnwrappedTileId(tileIDUnwrapped);


        //lat lon to meters because the tiles rect is also in meters
        Vector2d v2d = Conversions.LatLonToMeters(new Mapbox.Utils.Vector2d(lat, lon));
        //get the origin of the tile in meters
        Vector2d v2dcenter = tile.Rect.Center - new Mapbox.Utils.Vector2d(tile.Rect.Size.x / 2, tile.Rect.Size.y / 2);
        //offset between the tile origin and the lat lon point
        Vector2d diff = v2d - v2dcenter;

        //maping the diffetences to (0-1)
        float Dx = (float)(diff.x / tile.Rect.Size.x);
        float Dy = (float)(diff.y / tile.Rect.Size.y);

        height = tile.QueryHeightData(Dx, Dy);
        return height;
    }

    public Vector3 AdjustHeight(Vector3 pos)
    {
        var latlong = pos.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
        return GetVectorOnMap(latlong);
    }

    public Vector3 GetVectorOnMap(Vector2d latlong)
    {
        return GetVectorOnMap(latlong.x, latlong.y);
    }

    public Vector3 GetVectorOnMap(Location location)
    {
        return GetVectorOnMap(location.LatitudeLongitude.x, location.LatitudeLongitude.y);
    }
}
