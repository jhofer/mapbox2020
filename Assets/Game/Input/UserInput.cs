using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRubyShared;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField]
    public Transform cameraContainer;
    private RotateGestureRecognizer rotateGesture;
    private SwipeGestureRecognizer upSwipeGesture;
    private ScaleGestureRecognizer scaleGesture;
  

    [SerializeField]
    public float maxCamHeight = 350;

    public float topDownStart = 100;

    [SerializeField]
    public float minCamHeight = 15;

    // Start is called before the first frame update
    void Start()
    {
      
        CreateRotateGesture();
        CreateScaleGesture();
    }

  


    private void CreateScaleGesture()
    {
        scaleGesture = new ScaleGestureRecognizer();
        scaleGesture.StateUpdated += AdjustCamHeight;
        FingersScript.Instance.AddGesture(scaleGesture);
    }

    private void CreateRotateGesture()
    {
        rotateGesture = new RotateGestureRecognizer();
        rotateGesture.StateUpdated += RotateCam;
        FingersScript.Instance.AddGesture(rotateGesture);
    }

    private void AdjustCamHeight(GestureRecognizer gesture)
    {
                 
        if (gesture.State == GestureRecognizerState.Executing)
        {
            var delta = scaleGesture.ScaleMultiplierRange;
            var currentHeight = Camera.main.transform.localPosition.y;
            float speed = (delta* currentHeight)/ 100;
            var sum = currentHeight + speed;
            Debug.Log(currentHeight);
            Debug.Log(speed);
            Debug.Log(sum);
            var isInRange = (sum > minCamHeight || speed > 0) && (sum < maxCamHeight || speed < 0);
            if (isInRange)
            {
                Camera.main.transform.Translate(Vector3.up * speed, Space.World);
             
                Camera.main.transform.LookAt(cameraContainer);

               
                            

            }

        }
    }



    private void RotateCam(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            cameraContainer.Rotate(0.0f, rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg,0.0f );
        }
    }



  


 

    // Update is called once per frame
    void Update()
    {
        
    }
}
