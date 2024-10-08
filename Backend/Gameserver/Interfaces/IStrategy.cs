

namespace Gameserver.Interfaces
{
    public interface IStrategy
    {
        public object Execute(params object[] args);
    }
}
