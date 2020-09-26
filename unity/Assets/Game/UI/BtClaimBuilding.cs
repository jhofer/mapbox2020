using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtClaimBuilding : MonoBehaviour
{



    // Start is called before the first frame update
     void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
    {
        
    }

    public void BtnClaim()
    {
        BuildingController.Selected.Claim();
        DialogHandler.Instance.building = null;

    }

    public void BtnClose()
    {
        BuildingController.ResetSelection();
        DialogHandler.Instance.building = null;
    }
}
