using Gameserver.Adapters;
using Gameserver.Interfaces;

namespace Gameserver.Strategies
{
    public class GetAdapterCreatorStrategy : IStrategy
    {
        public object Strategy(params object[] args)
        {
            //  ol, nw
            return new AdapterCreator((Type)args[0], (Type)args[1]);
        }
    }
}
