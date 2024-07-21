using System;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Common.InputSystem
{
    public class InputSystem : MonoBehaviour, IInputSystem
    {
        bool isInterrupted = false;
        bool canProcessMoveInput = false;

        public event Action<Vector2> OnInteractionStartEvent;
        public event Action<Vector2> OnDuringInteractionEvent;
        public event Action OnInteractionEndedEvent;

        private void Awake()
        {
            DependencySolver.RegisterInstance(this as IInputSystem);
            DontDestroyOnLoad(gameObject);
            isInterrupted = false;
            canProcessMoveInput = false;
            gameObject.name = "[InputSystem]";
        }
        private void OnDestroy()
        {
            DependencySolver.RemoveInstance(this as IInputSystem);
        }
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
                {
                    OnInteractionStart(Input.touches[0].position);
                }
                if (Input.touches[0].phase == TouchPhase.Ended || Input.GetMouseButtonUp(0) || isInterrupted)
                {
                    OnInteractionEnded();
                }
                else if ((Input.touches[0].phase == TouchPhase.Moved || Input.GetMouseButton(0)) && canProcessMoveInput)
                {
                    OnDuringInteraction(Input.touches[0].position);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnInteractionStart(Input.mousePosition);
                }
                if (Input.GetMouseButtonUp(0) || isInterrupted)
                {
                    OnInteractionEnded();
                }
                else if (Input.GetMouseButton(0) && canProcessMoveInput)
                {
                    OnDuringInteraction(Input.mousePosition);
                }
            }
        }

        public void OnInteractionEnded()
        {
            canProcessMoveInput = false;
            OnInteractionEndedEvent?.Invoke();
        }

        public void OnDuringInteraction(Vector2 screenPosition)
        {
            OnDuringInteractionEvent?.Invoke(Camera.main.ScreenToWorldPoint(screenPosition));
        }

        public void OnInteractionStart(Vector2 screenPosition)
        {
            isInterrupted = false;
            canProcessMoveInput = true;
            OnInteractionStartEvent?.Invoke(Camera.main.ScreenToWorldPoint(screenPosition));
        }

        public void InterruptInteraction()
        {
            isInterrupted = true;
        }
    }
}
