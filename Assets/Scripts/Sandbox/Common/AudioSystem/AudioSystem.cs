using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WTF.Common;
using WTF.Common.DebugSystem;
using WTF.Common.IdentitySystem;

namespace WTF.common.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        private IDebugSystem debugSystem;
        public AudioDataContainerSO AudioDataContainer;
        private IIdentityMapper<AudioMeta> _mapper;
        public IIdentitySystem identitySystem;

        public IIdentityMapper<AudioMeta> mapper => _mapper;
        [SerializeField]
        private AudioSource audioSource;
        private void Awake()
        {
            DependencySolver.RegisterInstance(this as IAudioSystem);
            DontDestroyOnLoad(gameObject);
            gameObject.name = nameof(AudioSystem);
            _mapper = new AudioMapper(AudioDataContainer.AudioDataList);
            gameObject.name = nameof(AudioSystem);

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
                switch (audioMeta.audioData.audioType)
                {
                    case AudioType.SFX:
                        audioSource.PlayOneShot(audioMeta.audioData.audioClip);
                        break;
                    case AudioType.Music:
                        audioSource.clip = audioMeta.audioData.audioClip;
                        audioSource.loop = true;
                        audioSource.Play();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                debugSystem.LogError("AudioMeta not found for identity: " + identity);
            }
        }

        public void PlayAudio(Identity identity)
        {
            PlayAudioOnSource(identity, audioSource);
        }
    }

}
