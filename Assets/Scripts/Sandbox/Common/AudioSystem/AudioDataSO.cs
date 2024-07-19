using System.Collections;
using UnityEditor;
using UnityEngine;
using WTF.Common.IdentitySystem;
using WTF.global;

namespace WTF.common.AudioSystem
{
    [CreateAssetMenu(menuName = MENU_PATH, fileName = nameof(AudioDataSO), order = 1)]
    public class AudioDataSO : ScriptableObject, IIdentityMeta
    {
        private const string MENU_PATH = CONSTANTS.PATH.SCRIPTABLE_OBJECTS + nameof(AudioSystem) + CONSTANTS.SLASH + nameof(AudioDataSO);
        public Identity Identity => audioMeta.Identity;
        public AudioMeta audioMeta;
    }
}