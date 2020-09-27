using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : BaseSingleton<DialogHandler>
{
   
    [SerializeField]
    public GameObject buildingDialog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var buildingCtrl = SelectionHandler.Instance.GetSelection<BuildingController>(); 
        buildingDialog.SetActive(buildingCtrl != null);
        buildingDialog.GetComponent<BuildingDialog>().building = buildingCtrl;
       
    }
}
