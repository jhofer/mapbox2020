using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
   
    private BaseUnitContoller unitController;

    // Start is called before the first frame update
    void Start()
    {
        this.unitController = GetComponent<BaseUnitContoller>();
   
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, unitController.WeaponRange);
       
        foreach (var hitCollider in hitColliders)
        {
            var gameObj = hitCollider.gameObject;

            if (gameObj.GetComponent<BaseUnitContoller>() !=null && this.gameObject != gameObj)
            {
               
                unitController.SetTarget(gameObj);
                return;
            }
          
        }
        unitController.SetTarget(null);
    }
}
