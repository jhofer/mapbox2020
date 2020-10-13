using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private BaseUnitContoller unitController;

    [SerializeField]
    public GameObject Turret;
    private Quaternion turretTransform;

    // Start is called before the first frame update
    void Start()
    {
        this.unitController = GetComponent<BaseUnitContoller>();
    }
    // update() -> animation () --> lateupdate()
    private void Update()
    {
        //save initial rotation before animation
        turretTransform = Turret.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        var target = unitController.Target;
        if (target != null)
        {
            // reset rotation to state before animation
            Turret.transform.rotation = turretTransform;
            Debug.Log("animate turret");
           
            Vector3 relativePos = target.transform.position - Turret.transform.position;
           
            Quaternion toRotation = Quaternion.LookRotation(relativePos);

            Turret.transform.rotation = Quaternion.Lerp(Turret.transform.rotation, toRotation, Time.deltaTime);
        }
        
    }
}
