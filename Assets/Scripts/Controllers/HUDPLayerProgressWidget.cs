using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDPlayerProgressWidget : MonoBehaviour
{
    [SerializeField]
    List<Image> progressImages;
    public int currentProgress;
    public void SetProgress(int progress)
    {
        currentProgress = progress;
        for (int i = 0; i < progressImages.Count; i++)
        {
            if (i >= progress)
            {
                progressImages[i].enabled = false;
            }
            else
            {
                progressImages[i].enabled = true;
            }
        }
    }

    [ContextMenu("SetProgress")]
    public void SetProgress()
    {
        SetProgress(currentProgress);
    }
}
