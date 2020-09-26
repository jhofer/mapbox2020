using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IEntity, ISelectable
{
    [SerializeField]
    AbstractMap map;

    [SerializeField]
    float UnitHealth;

    public void Select()
    {
        CamMovement.Instance.SetTarget(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
