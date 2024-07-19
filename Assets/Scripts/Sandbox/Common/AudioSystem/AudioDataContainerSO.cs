using System.Collections.Generic;
using UnityEngine;
using WTF.global;

namespace WTF.common.AudioSystem
{
    [CreateAssetMenu(menuName = MENU_PATH, fileName = nameof(AudioDataContainerSO), order = 1)]
    public class AudioDataContainerSO : ScriptableObject
    {
        private const string MENU_PATH = CONSTANTS.PATH.SCRIPTABLE_OBJECTS + nameof(AudioSystem) + CONSTANTS.SLASH + nameof(AudioDataContainerSO);
        public List<AudioDataSO> AudioDataList;

    }
}