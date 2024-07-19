using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Common.IdentitySystem
{

    public class IdentitySystem : MonoBehaviour, IIdentitySystem
    {

        private IdentityMapper _mapper;
        public IIdentityMapper<IdentityMeta> mapper => _mapper;

        public void AddIdentityMetas(List<IdentityMeta> identities)
        {
            _mapper.AddIdentityMetas(identities);
            gameObject.name = nameof(IdentitySystem);
        }

        private void Awake()
        {
            DependencySolver.RegisterInstance(this as IIdentitySystem);
            DontDestroyOnLoad(gameObject);
            _mapper = new IdentityMapper(new List<IdentityMeta>());
        }

        private void OnDestroy()
        {
            DependencySolver.RemoveInstance(this as IIdentitySystem);
        }
    }
}

