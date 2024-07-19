using System;
using UnityEditor;
using UnityEngine;
using WTF.Common.IdentitySystem;
using WTF.global;

namespace WTF.common.AudioSystem
{

    [Serializable]
    public partial struct AudioMeta : IIdentityMeta
    {
        private const string IDENTITY_FILE_NAME_SUFFIX = CONSTANTS.DOT + CONSTANTS.SCRIPTABLE_OBJECT.IDENTITY + CONSTANTS.DOT + CONSTANTS.SYSTEMS.AUDIO;
        public Identity Identity => identityMeta.Identity;
        public IdentityMeta identityMeta;
        public AudioData audioData;

    }

}