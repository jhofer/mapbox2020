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
       // DialDialogHandler.
    }
}
