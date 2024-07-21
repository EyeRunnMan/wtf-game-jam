using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Common.InputSystem.Components
{
    [RequireComponent(typeof(LineRenderer))]
    public class InputLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        private InputSystem inputSystem;
        private List<Vector3> positions = new List<Vector3>();

        private void Start()
        {
            inputSystem = InputSystem.GetInstance();
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

        private void OnInterActionStarted(Vector2 _)
        {
            positions.Clear();

            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }
}
