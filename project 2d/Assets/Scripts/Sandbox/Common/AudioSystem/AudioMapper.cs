using System.Collections.Generic;
using UnityEngine;
using WTF.Common;
using WTF.Common.DebugSystem;
using WTF.Common.IdentitySystem;

namespace WTF.common.AudioSystem
{
    public class AudioMapper : IIdentityMapper<AudioMeta>
    {
        private IDebugSystem debugSystem;
        private Dictionary<Identity, AudioMeta> _audioMetaDictionary;
        public AudioMapper(List<AudioDataSO> audioDatas)
        {

            DependencySolver.TryGetInstance(out debugSystem);
            _audioMetaDictionary = new Dictionary<Identity, AudioMeta>();
            foreach (var audioData in audioDatas)
            {
                if (!_audioMetaDictionary.TryAdd(audioData.Identity, audioData.audioMeta))
                {
                    debugSystem.LogError(message: "Duplicate identity found in AudioDataSO: " + audioData.Identity);
                }
                else
                {
                    debugSystem.Log(message: "Added identity to AudioMapper: " + audioData.Identity);
                }
            }
        }

        public bool TryGetIdentityData(Identity identity, out AudioMeta identityData)
        {
            return _audioMetaDictionary.TryGetValue(identity, out identityData);
        }
    }
}