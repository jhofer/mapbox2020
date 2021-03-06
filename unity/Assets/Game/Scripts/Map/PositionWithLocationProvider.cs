
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Map;
    using UnityEngine;
    using Mapbox.Unity.MeshGeneration.Data;
    using System;

    public class PositionWithLocationProvider : MonoBehaviour
	{
		[SerializeField]
		private AbstractMap _map;

        [SerializeField]
        public bool alwayseFollow = true;

        /// <summary>
        /// The rate at which the transform's position tries catch up to the provided location.
        /// </summary>
        [SerializeField]
		float _positionFollowFactor;

		/// <summary>
		/// Use a mock <see cref="T:Mapbox.Unity.Location.TransformLocationProvider"/>,
		/// rather than a <see cref="T:Mapbox.Unity.Location.EditorLocationProvider"/>. 
		/// </summary>
		[SerializeField]
		bool _useTransformLocationProvider;

		bool _isInitialized;

		/// <summary>
		/// The location provider.
		/// This is public so you change which concrete <see cref="T:Mapbox.Unity.Location.ILocationProvider"/> to use at runtime.
		/// </summary>
		ILocationProvider _locationProvider;
		public ILocationProvider LocationProvider
		{
			private get
			{
				if (_locationProvider == null)
				{
					_locationProvider = _useTransformLocationProvider ?
						LocationProviderFactory.Instance.TransformLocationProvider : LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
			set
			{
				if (_locationProvider != null)
				{
					_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;

				}
				_locationProvider = value;
				_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			}
		}

        public MapUtils MapUtils { get; private set; }

        Vector3 _targetPosition;
        private Location location;
        public bool triggerFollow;

        void Start()
		{
			LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			_map.OnInitialized += () => _isInitialized = true;
            _map.MapVisualizer.OnTileHeightProcessingFinished += HightProcessed;

            MapUtils = MapUtils.Instance;
		}

        private void HightProcessed(UnityTile obj)
        {
          
            _targetPosition = MapUtils.GetVectorOnMap(location);
        }

        void OnDestroy()
		{
			if (LocationProvider != null)
			{
				LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
			}
		}

		void LocationProvider_OnLocationUpdated(Location loc)
		{
           //Debug.Log("latitude: " + loc.LatitudeLongitude.x + "longitude: " + location.LatitudeLongitude.y);
            this.location = loc;


            if (_isInitialized && location.IsLocationUpdated)
			{
				_targetPosition = MapUtils.GetVectorOnMap(location);
			}
		}

		void Update()
		{
            if (alwayseFollow)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _positionFollowFactor);
            }else if (triggerFollow)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _positionFollowFactor);
                if(Vector3.Distance(transform.position, _targetPosition) < 3)
                {
                    this.triggerFollow = false;
                }
              
            }
        }
	}

