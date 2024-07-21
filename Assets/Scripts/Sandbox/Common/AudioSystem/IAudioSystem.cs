using JetBrains.Annotations;
using UnityEngine;
using WTF.Common.IdentitySystem;

namespace WTF.common.AudioSystem
{
    public interface IAudioSystem
    {
        public IIdentityMapper<AudioMeta> mapper { get; }

        public void PlayAudioOnSource(Identity identity, AudioSource audioSource);
        public void PlayAudio(Identity identity);
    }

}
