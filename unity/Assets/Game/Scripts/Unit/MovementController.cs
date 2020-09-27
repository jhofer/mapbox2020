using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    public float UnitSpeed;



    [SerializeField]
    public UnitType unitType;

    bool isMoving;
    [SerializeField]
    Vector3 nextPos;
    [SerializeField]
    private Vector3 prevPos;

    [SerializeField]
    public List<Vector3> futurePositions;

   

    bool interruption;
  

    public void Start()
    {
        
    }

    public void SetTarget(Vector3 point)
    {
        MapUtils.Instance.GetWayPoints(unitType, transform.position, point, GetPositions);

    }


    void GetPositions(List<Vector3> newRoute)
    {   


        bool doReplacePath = newRoute != null;
        if (doReplacePath)
        {
            futurePositions = new List<Vector3>();
            prevPos = transform.position;
            nextPos = transform.position;
            interruption = true;
            futurePositions.AddRange(newRoute);

        }



    }

    void Update()
    {

        //MoveToNextPos();
        StartCoroutine(Moving());

    }

    IEnumerator Moving()
    {
        
       

        float t = 0;
       
        var time = TimeToTravleDistance();
        var hasRoute = futurePositions != null;
        while (hasRoute&&!interruption && t < 1 && Vector3.Distance(transform.position, nextPos)>0.1)
        {
            isMoving = true;

         
            t += Time.deltaTime / time; // 1/20 = 0.05... 20/20 = 10
            transform.position = Vector3.MoveTowards(transform.position,nextPos, UnitSpeed);

            transform.LookAt(nextPos);
            yield return null;
        }
        futurePositions.Remove(nextPos);
        interruption = false;
        isMoving = false;
        MoveToNextPos();

    }

    private void MoveToNextPos()
    {
        
        if (futurePositions?.Count > 0)
        {
            prevPos = transform.position;
            nextPos = futurePositions[0];
            
        
            StartCoroutine(Moving());
        }
        
    }



    float TimeToTravleDistance()
    {
        float timeToMove = 0;
        var distance = Vector3.Distance(prevPos, nextPos);
        timeToMove = distance / UnitSpeed;
        
        return timeToMove;
    }

}