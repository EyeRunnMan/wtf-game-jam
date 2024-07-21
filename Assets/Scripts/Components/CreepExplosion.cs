using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTF.Configs;
using WTF.Players;

namespace WTF.PlayerControls
{
    public class CreepExplosion : MonoBehaviour
    {
        [SerializeField] private float m_explodeDurationInSeconds = 0.5f;
        [SerializeField] private float[] m_explosionRanges;

        private Creep m_explosionParentCreep;
        private CreepTypes m_explosionType;
        private int m_explosionPower;
        private float m_explosionTimer;
        private float m_explosionScale;
        private readonly float m_searchRadiusMultiplier = 2.05f;

        private void Start()
        {
            m_explosionParentCreep = transform.parent.GetComponent<Creep>();
        }

        public void TriggerExplosion(int intensity, CreepTypes type)
        {
            m_explosionType = type;
            m_explosionPower = intensity;
            m_explosionScale = m_explosionRanges[Mathf.Clamp(intensity, 0, m_explosionRanges.Length - 1)];
            ClearExplosionArea();

            if (intensity == 1)
            {
                return;
            }

            StartCoroutine(ExpandExplosionArea());
        }

        private IEnumerator ExpandExplosionArea()
        {
            while(m_explosionTimer <= m_explodeDurationInSeconds)
            {
                m_explosionTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(m_explosionScale, m_explosionScale, 0), m_explosionTimer / m_explodeDurationInSeconds);
                yield return null;
            }

            transform.localScale = new Vector3(m_explosionScale, m_explosionScale, 0);
            StartExplosion();
        }

        private void StartExplosion()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_explosionScale * m_searchRadiusMultiplier, LayerMask.GetMask("creep"));
            foreach (Collider2D collider in colliders)
            {
                if (collider.transform.parent.gameObject == m_explosionParentCreep.gameObject)
                {
                    continue;
                }

                Creep creep = collider.transform.parent.GetComponent<Creep>();
                if (creep == null)
                {
                    continue;
                }

                if (m_explosionPower <= creep.creepCount)
                {
                    continue;
                }

                creep.InitiateExplosion(m_explosionType);
            }

            m_explosionParentCreep.SelfExplode();
            ClearExplosionArea();
        }

        private void ClearExplosionArea()
        {
            m_explosionTimer = 0;
            transform.localScale = Vector3.zero;
        }
    }
}
