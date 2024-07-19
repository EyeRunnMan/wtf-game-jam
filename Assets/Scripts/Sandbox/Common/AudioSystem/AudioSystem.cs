using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WTF.Common;
using WTF.Common.DebugSystem;
using WTF.Common.IdentitySystem;

namespace WTF.common.AudioSystem
{
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        private IDebugSystem debugSystem;
        public AudioDataContainerSO AudioDataContainer;
        private IIdentityMapper<AudioMeta> _mapper;
        public IIdentitySystem identitySystem;

        public IIdentityMapper<AudioMeta> mapper => _mapper;

        private void Awake()
        {
            DependencySolver.RegisterInstance(this as IAudioSystem);
            DontDestroyOnLoad(gameObject);
            gameObject.name = nameof(AudioSystem);
            _mapper = new AudioMapper(AudioDataContainer.AudioDataList);
        }
        private void Start()
        {
            DependencySolver.TryGetInstance(out debugSystem);
            DependencySolver.TryGetInstance(out identitySystem);
            identitySystem.AddIdentityMetas(AudioDataContainer.AudioDataList.ConvertAll(x => x.audioMeta.identityMeta));
        }
        private void OnDestroy()
        {
            DependencySolver.RemoveInstance(this as IAudioSystem);
        }

        public void PlayAudioOnSource(Identity identity, AudioSource audioSource)
        {
            if (mapper.TryGetIdentityData(identity, out var audioMeta))
            {
                audioSource.clip = audioMeta.audioData.audioClip;
                audioSource.Play();
            }
            else
            {
                debugSystem.LogError("AudioMeta not found for identity: " + identity);
            }
        }
    }

}
