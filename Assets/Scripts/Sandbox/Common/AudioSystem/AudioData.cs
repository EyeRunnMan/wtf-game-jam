using System;
using UnityEngine;

namespace WTF.common.AudioSystem
{
    [Serializable]
    public struct AudioData
    {
        public AudioClip audioClip;
        public AudioType audioType;
    }
    public enum AudioType
    {
        SFX,
        Music
    }
}