using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    [SerializeField]
    public MapUtils mapUtils;
    [SerializeField]
    Vector3 targetPos;
    [SerializeField]
    int camMovementSpeedMultiplier = 2;
    [SerializeField]
    double camRange = 0.5;

    

    Vector3 previousPos = Vector3.zero;
    Vector3 deltaPos = Vector3.zero;
    private bool focus;
    private float focusTime;
    private Transform targetTransform;


    // Start is called before the first frame update
    void Start()
    {
        targetTransform = transform;
        Camera.main.transform.LookAt(this.transform);
    }

    public void SetTarget(Vector3 target)
    {
        targetTransform = null;
        this.targetPos = target;
    }

    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            targetPos = targetTransform.position;
        }

        if (targetPos != default(Vector3))
        {
                    
            
            var range = Vector3.Distance(transform.position, targetPos);

            if (range >= camRange)
            {
               
                var realtiveSpeed = range * camMovementSpeedMultiplier;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, realtiveSpeed * Time.deltaTime);

             
            }
        }
     
    }
}
