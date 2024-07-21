using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Common.InputSystem
{
    public class InputSystem : MonoBehaviour, IInputSystem
    {
        [SerializeField]
        private int doubleTapDelayInMilliseconds = 500;
        bool isInterrupted = false;
        bool canProcessMoveInput = false;
        bool isCheckingForDoubleTap = false;
        public event Action<Vector2> OnSwipeStartEvent;
        public event Action<Vector2> OnDuringSwipeEvent;
        public event Action OnSwipeEventEnded;
        public event Action OnDoubleTapEvent;

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
            if (isCheckingForDoubleTap == false)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        StartCoroutine(DoubleTapCheck());
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(DoubleTapCheck());
                    }
                }
            }
        }

        private IEnumerator DoubleTapCheck()
        {
            isCheckingForDoubleTap = true;
            var startTime = Time.time;
            var endTime = Time.time + doubleTapDelayInMilliseconds / 1000;
            var didUserDoubleTapped = false;
            yield return null;
            while (Time.time < endTime)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        didUserDoubleTapped = true;
                        break;
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        didUserDoubleTapped = true;
                        break;
                    }
                }

                yield return null;
            }
            if (didUserDoubleTapped)
            {
                OnDoubleTapEvent?.Invoke();
            }
            isCheckingForDoubleTap = false;
        }

        public void OnInteractionEnded()
        {
            canProcessMoveInput = false;
            OnSwipeEventEnded?.Invoke();
        }

        public void OnDuringInteraction(Vector2 screenPosition)
        {
            OnDuringSwipeEvent?.Invoke(Camera.main.ScreenToWorldPoint(screenPosition));
        }

        public void OnInteractionStart(Vector2 screenPosition)
        {
            isInterrupted = false;
            canProcessMoveInput = true;
            OnSwipeStartEvent?.Invoke(Camera.main.ScreenToWorldPoint(screenPosition));
        }

        public void InterruptInteraction()
        {
            isInterrupted = true;
        }

        public void InterruptSwipe()
        {
            throw new NotImplementedException();
        }
    }
}
