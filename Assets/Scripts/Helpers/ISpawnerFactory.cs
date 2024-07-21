using WTF.Configs;
using WTF.Players;

namespace WTF.Helpers
{
    public interface ISpawnerFactory
    {
        Creep CreateCreep(CreepTypes type);
    }
}
