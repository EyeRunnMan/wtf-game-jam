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
            inputSystem.OnInteractionStartEvent += OnInterActionStarted;
            inputSystem.OnDuringInteractionEvent += OnDuringInteraction;
            inputSystem.OnInteractionEndedEvent += OnInteractionEnded;

        }
        private void OnDestroy()
        {
            inputSystem.OnInteractionStartEvent -= OnInterActionStarted;
            inputSystem.OnDuringInteractionEvent -= OnDuringInteraction;
            inputSystem.OnInteractionEndedEvent -= OnInteractionEnded;
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
            positions.Add(Camera.main.ScreenToWorldPoint(vector));
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

        private void OnInterActionStarted()
        {
            positions.Clear();

            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(positions.ToArray());
        }

    }

}
