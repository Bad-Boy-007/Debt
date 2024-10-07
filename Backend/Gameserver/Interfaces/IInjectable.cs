

namespace Gameserver.Interfaces
{
    public interface IInjectable
    {
        void Inject(ICommand obj);
    }
}
