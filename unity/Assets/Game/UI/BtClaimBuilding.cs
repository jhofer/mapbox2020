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
        Building.Selected.Claim();
        DialogHandler.Instance.building = null;
    }

    public void BtnClose()
    {
        Building.ResetSelection();
        DialogHandler.Instance.building = null;
    }
}
