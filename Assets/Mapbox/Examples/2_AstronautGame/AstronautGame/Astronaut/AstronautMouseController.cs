using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using System;
using UnityEngine.SceneManagement;

namespace Mapbox.Examples
{
	public class AstronautMouseController : MonoBehaviour
	{
		[Header("Character")]
		[SerializeField]
		GameObject character;
		[SerializeField]
		float characterSpeed;
		[SerializeField]
		Animator characterAnimator;

		[Header("References")]
		[SerializeField]
		AstronautDirections directions;
		[SerializeField]
		Transform startPoint;
		[SerializeField]
		Transform endPoint;
		[SerializeField]
		AbstractMap map;
		//[SerializeField]
		//GameObject rayPlane;
		[SerializeField]
		Transform _movementEndPoint;

		[SerializeField]
		LayerMask layerMask;

		Ray ray;
		RaycastHit hit;
		LayerMask raycastPlane;
		float clicktime;
		bool moving;
		bool characterDisabled;

		void Start()
		{
			characterAnimator = GetComponentInChildren<Animator>();
			if (!Application.isEditor)
			{
				this.enabled = false;
				return;
			}
		}

		void Update()
		{
			if (characterDisabled)
				return;

			

			bool click = false;

			if (Input.GetMouseButtonDown(0))
			{
				clicktime = Time.time;
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (Time.time - clicktime < 0.15f)
				{
					click = true;
				}
			}

            var isNotFocusClick = Time.time - focusTime > 1;

			if (click&focus& isNotFocusClick)
			{
				ray = camContainer.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				{
					startPoint.position = transform.localPosition;
					endPoint.position = hit.point;
					MovementEndpointControl(hit.point, true);

					directions.Query(GetPositions, startPoint, endPoint, map);
                }
                else
                {
                    Debug.Log("No hit");
                }
			}
		}

		#region Character : Movement
		List<Vector3> futurePositions;
		bool interruption;
		void GetPositions(List<Vector3> vecs)
		{
			futurePositions = vecs;

			if (futurePositions != null && moving)
			{
				interruption = true;
			}
			if (!moving)
			{
				interruption = false;
				MoveToNextPlace();
			}
		}

		Vector3 nextPos;
		void MoveToNextPlace()
		{
			if (futurePositions.Count > 0)
			{
				nextPos = futurePositions[0];
				futurePositions.Remove(nextPos);

				moving = true;
				characterAnimator.SetBool("IsWalking", true);
				StartCoroutine(MoveTo());
			}
			else if (futurePositions.Count <= 0)
			{
				moving = false;
				characterAnimator.SetBool("IsWalking", false);
			}
		}

		Vector3 prevPos;
		IEnumerator MoveTo()
		{
			prevPos = transform.localPosition;

			float time = CalculateTime();
			float t = 0;

			StartCoroutine(LookAtNextPos());

			while (t < 1 && !interruption)
			{
				t += Time.deltaTime / time;

				transform.localPosition = Vector3.Lerp(prevPos, nextPos, t);

				yield return null;
			}

			interruption = false;
			MoveToNextPlace();
		}

		float CalculateTime()
		{
			float timeToMove = 0;

			timeToMove = Vector3.Distance(prevPos, nextPos) / characterSpeed;

			return timeToMove;
		}
		#endregion

		#region Character : Rotation
		IEnumerator LookAtNextPos()
		{
			Quaternion neededRotation = Quaternion.LookRotation(nextPos - character.transform.position);
			Quaternion thisRotation = character.transform.localRotation;

			float t = 0;
			while (t < 1.0f)
			{
				t += Time.deltaTime / 0.25f;
				var rotationValue = Quaternion.Slerp(thisRotation, neededRotation, t);
				character.transform.rotation = Quaternion.Euler(0, rotationValue.eulerAngles.y, 0);
				yield return null;
			}
		}
        #endregion

        #region CameraControl

        void OnMouseDown()
        {
            AstronautMouseController.ResetEveryFocus();
            
            this.focus = true;
            this.focusTime = Time.time; 

        }

        private static void ResetEveryFocus()
        {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                var sf = (AstronautMouseController)root.GetComponent<AstronautMouseController>();
                if (sf != null)
                {
                    sf.ResetFocus();
                }
            }
        }

        private void ResetFocus()
        {
            this.focus = false;
        }

        [Header("CameraSettings")]
		[SerializeField]
		GameObject camContainer;
        [SerializeField]
        int camMovementSpeedMultiplier=2;
        [SerializeField]
        double camRange = 0.5;
     
		Vector3 deltaPos = Vector3.zero;
        private bool focus;
        private float focusTime;

     
		#endregion

		#region Utility
		public void DisableCharacter()
		{
			characterDisabled = true;
			moving = false;
			StopAllCoroutines();
			character.SetActive(false);
		}

		public void EnableCharacter()
		{
			characterDisabled = false;
			character.SetActive(true);
		}

		public void LayerChangeOn()
		{
			Debug.Log("OPEN");
		}

		public void LayerChangeOff()
		{
			Debug.Log("CLOSE");
		}

		void MovementEndpointControl(Vector3 pos, bool active)
		{
            var endpoint = new Vector3(pos.x, pos.y + 0.2f, pos.z); ;
            Debug.Log("x:"+pos.x+ "y:" + pos.y + "z:" + pos.z );
            _movementEndPoint.position = endpoint;

            _movementEndPoint.gameObject.SetActive(active);
		}
		#endregion
	}
}