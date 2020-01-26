using UnityEngine;
using Mapbox.Directions;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity;
using System;
using Mapbox.Map;

namespace Mapbox.Examples
{
    // TODO: move to own file
    public enum UnitType
    {
        Vehicle,
        Soldier,
    }

	public class AstronautDirections : MonoBehaviour
	{
		AbstractMap _map;
		Directions.Directions _directions;
		Action<List<Vector3>> callback;
        RoutingProfile routingProfile;

        [SerializeField]
        UnitType unitType;

        void Awake()
		{
			_directions = MapboxAccess.Instance.Directions;
            switch (unitType)
            {
                case UnitType.Soldier:
                    routingProfile = RoutingProfile.Walking;
                    break;
                case UnitType.Vehicle:
                    routingProfile = RoutingProfile.Driving;
                    break;
                default:
                    routingProfile = RoutingProfile.Driving;
                    break;
            }
        }

		public void Query(Action<List<Vector3>> vecs, Transform start, Transform end, AbstractMap map)
		{
			if (callback == null)
				callback = vecs;

			_map = map;

			var wp = new Vector2d[2];
			wp[0] = start.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			wp[1] = end.GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			var _directionResource = new DirectionResource(wp, routingProfile);
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);
		}

		void HandleDirectionsResponse(DirectionsResponse response)
		{
			if (null == response.Routes || response.Routes.Count < 1)
			{
				return;
			}

			var dat = new List<Vector3>();
			foreach (var point in response.Routes[0].Geometry)
            {
                var lat = point.x;
                var lon = point.y;
                Vector3 withHeight = GetVectorOnMap(lat, lon);

                Debug.Log("add withHeight x:" + withHeight.x + " y:" + withHeight.y + " z:" + withHeight.z);
                dat.Add(withHeight);
            }

            callback(dat);
		}

        private Vector3 GetVectorOnMap(double lat, double lon)
        {
            //get tile ID
            var tileIDUnwrapped = TileCover.CoordinateToTileId(new Mapbox.Utils.Vector2d(lat, lon), (int)_map.Zoom);

            //get tile
            UnityTile tile = _map.MapVisualizer.GetUnityTileFromUnwrappedTileId(tileIDUnwrapped);


            //lat lon to meters because the tiles rect is also in meters
            Vector2d v2d = Conversions.LatLonToMeters(new Mapbox.Utils.Vector2d(lat, lon));
            //get the origin of the tile in meters
            Vector2d v2dcenter = tile.Rect.Center - new Mapbox.Utils.Vector2d(tile.Rect.Size.x / 2, tile.Rect.Size.y / 2);
            //offset between the tile origin and the lat lon point
            Vector2d diff = v2d - v2dcenter;

            //maping the diffetences to (0-1)
            float Dx = (float)(diff.x / tile.Rect.Size.x);
            float Dy = (float)(diff.y / tile.Rect.Size.y);


            var location = Conversions.GeoToWorldPosition(lat, lon, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
            var height = tile.QueryHeightData(Dx, Dy);
            var withHeight = new Vector3(location.x, height, location.z);
            return withHeight;
        }
    }
}