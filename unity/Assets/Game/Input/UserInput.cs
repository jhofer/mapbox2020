using System.Collections;
using System;
using System.Collections.Generic;
using DigitalRubyShared;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    [SerializeField]
    public GameObject cameraContainer;
    private RotateGestureRecognizer rotateGesture;
    private SwipeGestureRecognizer upSwipeGesture;
    private ScaleGestureRecognizer scaleGesture;
    private TapGestureRecognizer tapGesture;
  

  
    private PanGestureRecognizer panGesture;
    private CamMovement camMovement;

    // Start is called before the first frame update
    void Start()
    {
         this.camMovement = cameraContainer.GetComponent<CamMovement>();

        CreateRotateGesture();
        CreateScaleGesture();
        CreateTapGesture();
        CreatePanGesture();
    }

    private void CreatePanGesture()
    {
       panGesture  = new PanGestureRecognizer();
       panGesture.StateUpdated += MoveMap;
       FingersScript.Instance.AddGesture(panGesture);

    }

    private void MoveMap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            Debug.Log("Move map");
            float deltaX = panGesture.DeltaX / 25.0f;
            float deltaY = panGesture.DeltaY / 25.0f;
            var movment = new Vector2(deltaX, deltaY);
            CamMovement.Instance.Move(movment);
          
          
        }
    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += RayCastTap;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void RayCastTap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector3 pos = new Vector3(gesture.FocusX, gesture.FocusY, 0.0f);
            // pos = Camera.main.ScreenToWorldPoint(pos);



            var ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                var terrainLayer = 10;
                var go = hit.transform.gameObject;
                Debug.Log("Tap on " + go.name);
                ISelectable selectable;
                if (TryGetSelectable(go, out selectable))
                {
                    Debug.Log("Selectable gefunden");
                    selectable.Select();
                }
                else
                {
                    var ctrl = SelectionHandler.Instance.GetSelection<BaseUnitContoller>();
                    if (ctrl != null)
                    {
                        var mc = ctrl.GetComponent<MovementController>();
                        Debug.Log("Set new target");
                        mc.SetTarget(hit.point);


                    }
                    else
                    {
                        UnitSpawner.Instance.SpawnEntity(hit.point);
                        Debug.Log("No selectable");

                    }

                }


            }
            else
            {
                Debug.Log("No hit");
            }
        }
    
      

    }

    private static bool TryGetSelectable(GameObject go, out ISelectable selectable)
    {

        selectable = go.GetComponentInParent<ISelectable>();
        return selectable != null;

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
           
            camMovement.Elevate(delta);

           

        }
    }



    private void RotateCam(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {

            cameraContainer.transform.Rotate(0.0f, rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg,0.0f );
        }
    }



  


 

    // Update is called once per frame
    void Update()
    {
        
    }
}
