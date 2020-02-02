using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : BaseSingleton<CamMovement>
{
    [SerializeField]
    public MapUtils mapUtils;
    [SerializeField]
    Vector3 targetPos;
    [SerializeField]
    int camMovementSpeedMultiplier = 2;
    [SerializeField]
    double camRange = 0.5;

    [SerializeField]
    public float maxCamHeight = 350;
    [SerializeField]
    public float topDownStart = 100;
    [SerializeField]
    public float minCamHeight = 15;



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

    public void Move(Vector2 movment)
    {
        var currentHeight = Camera.main.transform.localPosition.y;

        var forwardAmount = movment.y * Time.deltaTime * camMovementSpeedMultiplier* (currentHeight / 100) ;
        var sideAmount = movment.x * Time.deltaTime * camMovementSpeedMultiplier* (currentHeight / 100) ;
        var forwardMovent =  transform.forward * forwardAmount;
        var sideMovment =  transform.right * sideAmount;
        transform.position = transform.position+forwardMovent + sideMovment;
      
    }

    internal void Elevate(float delta)
    {
        var currentHeight = Camera.main.transform.localPosition.y;
        var speed = camMovementSpeedMultiplier * Time.deltaTime * 3;
        float range = delta * (currentHeight / 100)* speed ;
        var sum = currentHeight + range;
        Debug.Log(currentHeight);
        Debug.Log(range);
        Debug.Log(sum);
        var isInRange = (sum > minCamHeight || range > 0) && (sum < maxCamHeight || range < 0);
        if (isInRange)
        {
            Camera.main.transform.Translate(Vector3.up * range, Space.World);

            Camera.main.transform.LookAt(transform);



        }
    }

    internal void Rotate(float rotationRadiansDelta)
    {
        transform.Rotate(0.0f, rotationRadiansDelta * Mathf.Rad2Deg, 0.0f);
    }
}
