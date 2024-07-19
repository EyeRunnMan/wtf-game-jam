namespace WTF.Common.IdentitySystem
{
    public interface IIdentityMapper<T> where T : IIdentityMeta
    {
        public bool TryGetIdentityData(Identity identity, out T identityData);
    }
}

