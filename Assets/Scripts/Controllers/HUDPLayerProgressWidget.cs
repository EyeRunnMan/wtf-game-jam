using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerProgressWidget : MonoBehaviour
{
    [SerializeField] List<Image> m_progressImages;

    private int m_currentProgress;

    private void Start()
    {
        m_currentProgress = m_progressImages.Count;
    }

    public void UpdateProgress(int delta)
    {
        m_currentProgress += delta;
        m_currentProgress = Mathf.Clamp(m_currentProgress, -1, m_progressImages.Count);
        for (int i = 0; i < m_progressImages.Count; ++i)
        {
            if (i >= m_currentProgress)
            {
                m_progressImages[i].enabled = false;
            }
            else
            {
                m_progressImages[i].enabled = true;
            }
        }
    }
}
