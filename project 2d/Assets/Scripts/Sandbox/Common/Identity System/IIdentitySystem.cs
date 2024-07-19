using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;

namespace WTF.Common.IdentitySystem
{
    public interface IIdentitySystem
    {
        public void AddIdentityMetas(List<IdentityMeta> identities);
        public IIdentityMapper<IdentityMeta> mapper { get; }

    }

}

