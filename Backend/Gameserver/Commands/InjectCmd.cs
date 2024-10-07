using Gameserver.Interfaces;


namespace Gameserver.Commands
{
    public class InjectCmd : ICommand, IInjectable
    {
        private ICommand command;

        public InjectCmd(ICommand cmd)
        {
            command = cmd;
        }

        public void Inject(ICommand oth)
        {
            command = oth;
        }
        public void Execute()
        {
            command.Execute();
        }
    }
}
