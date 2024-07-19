using System;
using UnityEngine;

namespace WTF.Common.IdentitySystem
{
    public interface IIdentityMeta
    {
        public Identity Identity { get; }
    }
    [Serializable]
    public struct IdentityMeta : IIdentityMeta
    {
        [SerializeField]
        public IdentitySO identitySO;

        public Identity Identity => identitySO.Identity;
        public string name;
        [TextArea]
        public string description;
    }

}

