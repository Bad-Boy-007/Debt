using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class EndMoveCmd
    {
        private readonly IEndable endable;

        public EndMoveCmd(IEndable endable)
        {
            this.endable = endable;
        }

        public void Execute()
        {
            IoC.Resolve<string>("Gameserver.UObject.DeleteProperty", endable.Object, endable.Property);
            var command = endable.Command;
            var emptyCommand = new EmptyCmd();
            IoC.Resolve<IInjectable>("Gameserver.Command.Inject", command, emptyCommand);
        }
    }
}
