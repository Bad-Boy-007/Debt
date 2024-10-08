using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class LongOperation : IStrategy
    {
        private readonly string _cmdName;
        private readonly IUObject _target;

        public LongOperation(string cmdName, IUObject target)
        {
            _cmdName = cmdName;
            _target = target;
        }

        public object Execute(params object[] args)
        {
            var macroCommand = IoC.Resolve<Interfaces.ICommand>("Gameserver.MacroCommand.Create", _cmdName, _target);

            var repeatCommand = IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.Repeat", macroCommand);

            var injectCommand = IoC.Resolve<Interfaces.ICommand>("Gameserver.Command.Inject", repeatCommand);

            return injectCommand;
        }
    }
}
