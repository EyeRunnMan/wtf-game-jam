using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTF.common.AudioSystem;
using WTF.Common;
using WTF.Common.IdentitySystem;

public class AudioComp : MonoBehaviour
{
    public bool playOnEnable;

    public IdentitySO audioID;
    private IAudioSystem audioSystem;
    private void OnEnable()
    {
        DependencySolver.TryGetInstance(out audioSystem);

        if (playOnEnable)
        {
            PlayAudio();
        }
    }
    public void PlayAudio()
    {
        audioSystem.PlayAudio(audioID.Identity);
    }
}
