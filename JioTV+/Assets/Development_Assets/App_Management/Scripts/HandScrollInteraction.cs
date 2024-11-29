using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace JioCinema
{
    public class HandScrollInteraction : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] private InputActionProperty rightHandInputAction;
        [SerializeField] private InputActionProperty rightHandInputActionPosition;

        [SerializeField] private ScrollController scrollController;

        //[SerializeField] private SliderController sliderController;
        
        private Vector3 rightHandPosition;
        
        private bool isPinchHold;
        public bool IsPinchHold => isPinchHold;
        
        private ScrollRect horizontalScroll;
        
        private Vector3 lastPalmPostion = Vector3.zero;
        private RaycastHit hitinfo;
        private bool slideHit;

        public static Action pinchClicked;
        public static Action<RaycastHit, Vector3> OnSliderMoved;
        public static Action<Vector3> sliderCurrentPosition;

        private float pinchTime = 0;
        
        private void OnEnable()
        {
            rightHandInputAction.EnableDirectAction();
            rightHandInputActionPosition.EnableDirectAction();
            rightHandInputAction.action.performed += PinchActionPerformedByRightHand;
        }
        
        private void PinchActionPerformedByRightHand(InputAction.CallbackContext obj)
        {
            pinchTime = 0;
            slideHit = false;
            ScrollRaycast();
            
            isPinchHold = true;
        }
        
        private void OnDisable()
        {
            rightHandInputAction.DisableDirectAction();
            rightHandInputActionPosition.DisableDirectAction();
            rightHandInputAction.action.performed -= PinchActionPerformedByRightHand;
        }

        private void ScrollRaycast()
        {
            Ray _gazeRay = new Ray(mainCamera.position, mainCamera.forward);
            
            if (Physics.Raycast(_gazeRay, out hitinfo))
            {
               

                if (hitinfo.transform.TryGetComponent(out HorizontalScroll scrollRect))
                {
                    scrollController.InitializeScroll(rightHandInputActionPosition.action.ReadValue<Vector3>(), scrollRect);
                }
                else scrollController.InitializeScroll(rightHandInputActionPosition.action.ReadValue<Vector3>());


                //Debug.Log($"Hitinfo slider: {hitinfo.point.x}, {hitinfo.point.y}, {hitinfo.point.z}");

                if (hitinfo.transform.TryGetComponent(out Slider _slider))
                {
                    Debug.Log($"point pos : { _slider.transform.InverseTransformPoint(hitinfo.point)}");
                    OnSliderMoved?.Invoke(hitinfo, rightHandInputActionPosition.action.ReadValue<Vector3>());

                    slideHit = true;
                    //sliderController.InitializeSlider(rightHandInputActionPosition.action.ReadValue<Vector3>(), _slider, hitinfo);
                }
            }
            else scrollController.InitializeScroll(rightHandInputActionPosition.action.ReadValue<Vector3>());
        }

        private void Update()
        {
            rightHandPosition = rightHandInputActionPosition.action.ReadValue<Vector3>();
            if (isPinchHold)
            {
                var isScroll = scrollController.CheckAndScroll(rightHandPosition);
                
                if (slideHit)
                {
                    sliderCurrentPosition?.Invoke(rightHandPosition);
                }

                if (!rightHandInputAction.action.IsInProgress())
                {
                    isPinchHold = false;
                    horizontalScroll = null;
                    
                    if ((pinchTime is > 0 and <= 0.500f) && !isScroll)
                    {
                        pinchClicked?.Invoke();
                        pinchTime = 0;
                    }
                }
                pinchTime += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ScrollRaycast();
            }
        }
    }
}

