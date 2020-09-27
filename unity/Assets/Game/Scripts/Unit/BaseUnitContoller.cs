using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class BaseUnitContoller : MonoBehaviour, ISelectable, IEntity
{
    
    [SerializeField]
    float UnitHealth;

    [SerializeField]
    public UnitType UnitType;

    public bool IsSelected { get => SelectionHandler.Instance.IsSelected(this); }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        SelectionHandler.Instance.Select(this);
    }
}
