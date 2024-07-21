using UnityEngine;
using WTF.Common;
using WTF.Configs;
using WTF.Helpers;
using WTF.Players;

namespace WTF.PlayerControls
{
    public class CreepSpriteHandler : MonoBehaviour
    {
        [SerializeField] private Sprite[] m_goodCharSprites;
        [SerializeField] private Sprite[] m_badCharSprites;
        [SerializeField] private SpriteRenderer m_charSpriteRenderer;

        private ISpawnerFactory m_factory;

        public void Start()
        {
            DependencySolver.TryGetInstance(out m_factory);
        }

        public void UpdateSprite(CreepTypes creepType, int spriteType)
        {
            Sprite[] spriteList = {};
            if (creepType == CreepTypes.Player) spriteList = m_goodCharSprites;
            else if (creepType == CreepTypes.Enemy) spriteList = m_badCharSprites;

            if (spriteList == null || spriteList.Length <= 0)
            {
                Debug.LogWarning("No sprites found for type: "+creepType);
                return;
            }

            spriteType = Mathf.Clamp(spriteType, 0, spriteList.Length - 1);
            m_charSpriteRenderer.sprite = spriteList[spriteType];
        }

        public Creep UpdateCharacterAndSprite(CreepTypes creepType, int spriteType)
        {
            gameObject.SetActive(false);

            Creep newCreep = m_factory.CreateCreep(creepType);

            if (newCreep == null)
            {
                return null;
            }

            newCreep.transform.position = transform.position;
            newCreep.transform.rotation = transform.rotation;
            newCreep.spriteHandler.UpdateSprite(creepType, spriteType);

            foreach (Transform child in transform)
            {
                child.parent = newCreep.transform;
            }
            newCreep.transform.parent = transform.parent;

            Destroy(gameObject);
            return newCreep;
        }
    }
}
