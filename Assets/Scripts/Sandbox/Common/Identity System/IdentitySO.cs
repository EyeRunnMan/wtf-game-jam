using UnityEditor;
using UnityEngine;
using WTF.global;

namespace WTF.Common.IdentitySystem
{

    [CreateAssetMenu(menuName = MENU_PATH, fileName = nameof(IdentitySO), order = 1)]
    public class IdentitySO : ScriptableObject
    {
        private const string MENU_PATH = CONSTANTS.PATH.SCRIPTABLE_OBJECTS + nameof(IdentitySystem) + CONSTANTS.SLASH + nameof(IdentitySO);
        private const string FILE_NAME_SUFFIX = CONSTANTS.DOT + CONSTANTS.SCRIPTABLE_OBJECT.IDENTITY;

        [SerializeField]
        private Identity _identity;
        public Identity Identity
        {
            get => string.IsNullOrEmpty(_identity.Id) ? Identity.Invalid : _identity;
            set
            {
                _identity = value;
            }
        }

    }
}

