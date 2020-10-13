using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class BaseUnitContoller : MonoBehaviour, ISelectable, IEntity
{
    
    [SerializeField]
    public float UnitHealth;

    [SerializeField]
    public float WeaponRange;

    [SerializeField]
    public UnitType UnitType;
    public GameObject Target;

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

    public void SetTarget(GameObject gameObject)
    {
        if (Target != gameObject)
        {
            if (gameObject == null)
            {
                this.Target = gameObject;
                Debug.Log("Reset target");
            }
            else
            {
                this.Target = gameObject;
                Debug.Log("Set target");
            }
        }
      
    }
}
