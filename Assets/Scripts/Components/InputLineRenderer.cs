using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTF.Events;

namespace WTF.Common.InputSystem.Components
{
    [RequireComponent(typeof(LineRenderer))]
    public class InputLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_lineRenderer;

        private bool m_canRender;
        private InputSystem m_inputSystem;
        private List<Vector3> m_positions = new List<Vector3>();

        private void OnEnable()
        {
            m_canRender = true;
            m_inputSystem = InputSystem.GetInstance();
            m_inputSystem.OnSwipeStartEvent += OnInterActionStarted;
            m_inputSystem.OnDuringSwipeEvent += OnDuringInteraction;
            m_inputSystem.OnSwipeEventEnded += OnInteractionEnded;

            EventDispatcher<bool>.Register(CustomEvents.OutOfMoves, OnOutOfMoves);
        }

        private void OnDisable()
        {
            m_inputSystem.OnSwipeStartEvent -= OnInterActionStarted;
            m_inputSystem.OnDuringSwipeEvent -= OnDuringInteraction;
            m_inputSystem.OnSwipeEventEnded -= OnInteractionEnded;

            EventDispatcher<bool>.Unregister(CustomEvents.OutOfMoves, OnOutOfMoves);
        }

        private void OnInteractionEnded()
        {
            m_positions.Clear();

            m_lineRenderer.positionCount = 0;
            m_lineRenderer.SetPositions(m_positions.ToArray());
        }

        private void OnDuringInteraction(Vector2 vector)
        {
            if (!m_canRender)
            {
                return;
            }

            m_positions.Add(vector);
            m_lineRenderer.positionCount = m_positions.Count;
            m_lineRenderer.SetPositions(m_positions.ToArray());
        }

        private void OnInterActionStarted(Vector2 _)
        {
            m_positions.Clear();

            m_lineRenderer.positionCount = 0;
            m_lineRenderer.SetPositions(m_positions.ToArray());
        }

        private void OnOutOfMoves(bool _)
        {
            m_canRender = false;
        }
    }
}
