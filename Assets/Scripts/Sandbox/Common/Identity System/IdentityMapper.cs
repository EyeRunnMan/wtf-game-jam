using System.Collections.Generic;
using UnityEngine;
using WTF.Common.DebugSystem;

namespace WTF.Common.IdentitySystem
{
    public class IdentityMapper : IIdentityMapper<IdentityMeta>
    {
        private IDebugSystem debugSystem;
        Dictionary<Identity, IdentityMeta> _identityMetaDictionary;
        public IdentityMapper(List<IdentityMeta> identityDatas)
        {
            DependencySolver.TryGetInstance(out debugSystem);
            _identityMetaDictionary = new Dictionary<Identity, IdentityMeta>();

            AddIdentityMetas(identityDatas);
        }

        public void AddIdentityMetas(List<IdentityMeta> identities)
        {
            foreach (var identityData in identities)
            {
                if (!_identityMetaDictionary.TryAdd(identityData.Identity, identityData))
                {
                    debugSystem.LogError("Duplicate identity found in IdentityMeta: " + identityData.Identity);
                }
                else
                {
                    debugSystem.Log("Added identity to IdentityMapper: " + identityData.Identity);
                }
            }
        }

        public bool TryGetIdentityData(Identity identity, out IdentityMeta identityData)
        {
            throw new System.NotImplementedException();
        }
    }
}

