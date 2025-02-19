namespace Gameserver.Interfaces
{
    public interface IQueue
    {
        void Add(ICommand cmd);
        ICommand Take();
    }
}
