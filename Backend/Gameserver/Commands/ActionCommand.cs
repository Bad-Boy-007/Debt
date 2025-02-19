namespace Gameserver.Commands
{
    public class ActionCommand : Interfaces.ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute() => _action();
    }
}
