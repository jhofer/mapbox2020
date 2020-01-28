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
    private LongPressGestureRecognizer longPressGesture;
    private float lastY;

    // Start is called before the first frame update
    void Start()
    {
        CreateDoubleTapGesture();
        CreateTapGesture();
        CreateSwipeGesture();
        CreatePanGesture();
        CreateScaleGesture();
        CreateRotateGesture();
        CreateLongPressGesture();
    }

    private void CreateLongPressGesture()
    {
        longPressGesture = new LongPressGestureRecognizer();
        longPressGesture.MaximumNumberOfTouchesToTrack = 1;
        longPressGesture.StateUpdated += LongPressGestureCallback;
        FingersScript.Instance.AddGesture(longPressGesture);
    }

    private void LongPressGestureCallback(GestureRecognizer gesture)
    {


        if (gesture.State == GestureRecognizerState.Began)
        {
            this.lastY = gesture.FocusY;

        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            var delta = lastY - gesture.FocusY;

            float speed = delta / 10;
            var currentHeight = Camera.main.transform.localPosition.y;
            var sum = currentHeight + speed;
            Debug.Log(currentHeight);
            Debug.Log(speed);
            Debug.Log(sum);
            if((sum > 15 || speed > 0) && (sum < 30 || speed <0))
            {
                Camera.main.transform.Translate(Vector3.up * speed, Space.World);

            }
            lastY = gesture.FocusY;
        }


       
    }

    private void CreateRotateGesture()
    {
        rotateGesture = new RotateGestureRecognizer();
        rotateGesture.StateUpdated += RotateGestureCallback;
        FingersScript.Instance.AddGesture(rotateGesture);
    }

    private void RotateGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            cameraContainer.Rotate(0.0f, rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg,0.0f );
        }
    }

    private void CreateScaleGesture()
    {

    }

    private void CreatePanGesture()
    {

    }

    private void CreateSwipeGesture()
    {

            upSwipeGesture = new SwipeGestureRecognizer();
            upSwipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
            upSwipeGesture.MinimumNumberOfTouchesToTrack = 2;
            upSwipeGesture.StateUpdated += SwipeGestureCallback;
            upSwipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
            FingersScript.Instance.AddGesture(upSwipeGesture);

    }

    private void SwipeGestureCallback(GestureRecognizer gesture)
    {

    }

    private void CreateTapGesture()
    {

    }

    private void CreateDoubleTapGesture()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
