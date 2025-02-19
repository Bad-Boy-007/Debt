using Gameserver.Interfaces;
using Hwdtech;

namespace Gameserver.Strategies
{
    public class GetCodeFromAdapterCreatorStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            var creator = IoC.Resolve<ICreator>("Game.AdapterCreator", (Type)args[0], (Type)args[1]);
            return creator.Create();
        }
    }
}
