using Gameserver.Commands;


namespace Gameserver.Interfaces
{
    public interface IEndable
    {
        public InjectCmd Command { get; }
        public IUObject Object { get; }
        public IEnumerable<string> Property { get; }

    }
}
