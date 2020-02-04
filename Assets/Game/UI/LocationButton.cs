using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public PositionWithLocationProvider locationProvider;


    public void MoveToLocation()
    {
        locationProvider.triggerFollow = true;
    }
}
