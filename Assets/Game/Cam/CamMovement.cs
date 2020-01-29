using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
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
            var range = Vector3.Distance(transform.position, targetPos);
                 
            if (range >= camRange)
            {
                var realtiveSpeed = range * camMovementSpeedMultiplier;
                deltaPos = transform.position - targetPos;
                deltaPos = deltaPos.normalized;
                transform.Translate(deltaPos * realtiveSpeed * Time.deltaTime, Space.World);
                previousPos = targetPos;
            }
        }
     
    }
}
