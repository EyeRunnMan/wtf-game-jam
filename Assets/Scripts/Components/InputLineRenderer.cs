using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WTF.Common.InputSystem.Components
{
    [RequireComponent(typeof(LineRenderer))]
    public class InputLineRenderer : MonoBehaviour
    {
        IInputSystem inputSystem;
        [SerializeField]
        LineRenderer lineRenderer;
        private void Start()
        {
            WTF.Common.DependencySolver.TryGetInstance(out inputSystem);
            inputSystem.OnSwipeStartEvent += OnInterActionStarted;
            inputSystem.OnDuringSwipeEvent += OnDuringInteraction;
            inputSystem.OnSwipeEventEnded += OnInteractionEnded;

        }
        private void OnDestroy()
        {
            inputSystem.OnSwipeStartEvent -= OnInterActionStarted;
            inputSystem.OnDuringSwipeEvent -= OnDuringInteraction;
            inputSystem.OnSwipeEventEnded -= OnInteractionEnded;
        }
        List<Vector3> positions = new List<Vector3>();
        private void OnInteractionEnded()
        {
            positions.Clear();

            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(positions.ToArray());
        }

        private void OnDuringInteraction(Vector2 vector)
        {
            positions.Add(vector);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

        private void OnInterActionStarted(Vector2 vector2)
        {
            positions.Clear();

            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(positions.ToArray());
        }

    }

}
