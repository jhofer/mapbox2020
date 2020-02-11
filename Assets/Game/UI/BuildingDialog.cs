using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDialog : MonoBehaviour
{
    [SerializeField]
    public Building building;

    public Text txtValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            txtValue.text = building.BuildingValue+"$";
        }
       
    }
}
