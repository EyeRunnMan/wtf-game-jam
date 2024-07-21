using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using WTF.common.AudioSystem;
using WTF.Common;
using WTF.Common.IdentitySystem;

public class UIPointerSFXComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private IdentitySO m_pointerEnterSFX_Id;
    [SerializeField] private IdentitySO m_pointerExitSFX_Id;
    [SerializeField] private IdentitySO m_pointerDownSFX_Id;

    private IAudioSystem audioSystem;
    private void Start()
    {
        DependencySolver.TryGetInstance(out audioSystem);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSystem.PlayAudio(m_pointerEnterSFX_Id.Identity);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        audioSystem.PlayAudio(m_pointerExitSFX_Id.Identity);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSystem.PlayAudio(m_pointerDownSFX_Id.Identity);
    }
}
