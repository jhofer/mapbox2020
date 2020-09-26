﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : BaseSingleton<DialogHandler>
{
    [SerializeField]
    public BuildingController building;

    [SerializeField]
    public GameObject buildingDialog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        buildingDialog.SetActive(building != null);
        buildingDialog.GetComponent<BuildingDialog>().building = building;

    }
}
