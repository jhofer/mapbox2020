using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtClaimBuilding : MonoBehaviour
{



    public void BtnClaim()
    {
        SelectionHandler.Instance.GetSelection<BuildingController>().Claim();
        SelectionHandler.Instance.ClearSelection();

    }

    public void BtnClose()
    {
        SelectionHandler.Instance.ClearSelection();
       
    }
}
