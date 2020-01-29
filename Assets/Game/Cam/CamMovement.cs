using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [SerializeField]
    public MapUtils mapUtils;
    [SerializeField]
    GameObject target;
    [SerializeField]
    int camMovementSpeedMultiplier = 2;
    [SerializeField]
    double camRange = 0.5;
    Vector3 previousPos = Vector3.zero;
    Vector3 deltaPos = Vector3.zero;
    private bool focus;
    private float focusTime;


    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.LookAt(this.transform);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && target.activeSelf)
        {

            var targetPos = target.transform.position;
            if (target.TryGetComponent<Building>(out var building))
            {
                targetPos = target.transform.TransformPoint(target.GetComponent<BoxCollider>().center);// mapUtils.AdjustHeight(target.GetComponent<BoxCollider>().center..position);


            }
            var range = Vector3.Distance(transform.position, targetPos);

            if (range >= camRange)
            {
               
                var realtiveSpeed = range * camMovementSpeedMultiplier;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, realtiveSpeed * Time.deltaTime);

             
            }
        }
     
    }
}
