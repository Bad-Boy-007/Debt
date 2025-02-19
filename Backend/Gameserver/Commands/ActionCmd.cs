using Gameserver.Interfaces;


namespace Gameserver.Commands
{
    public class ActionCmd : ICommand
    {
        private readonly Action _action;

        public ActionCmd(Action action)
        {
            _action = action;
        }

        public void Execute() => _action();
    }
}
