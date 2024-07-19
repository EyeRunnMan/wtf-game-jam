using System;

namespace WTF.Common.IdentitySystem
{
    [Serializable]
    public struct Identity : IEquatable<Identity>
    {
        public string Id;
        public int Type;
        public int SubType;
        public bool Equals(Identity other)
        {
            return Id == other.Id;
        }
        public static Identity Invalid => new Identity { Id = "INVALID", Type = -1, SubType = -1 };
    }
}

