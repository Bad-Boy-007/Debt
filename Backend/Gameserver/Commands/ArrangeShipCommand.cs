using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class ArrangeShipCommand : Interfaces.ICommand
    {
        private readonly IUObject _uobject;
        private readonly IEnumerator<object> _posenumerator;

        public ArrangeShipCommand(IUObject uobject, IEnumerator<object> posenumerator)
        {
            _uobject = uobject;
            _posenumerator = posenumerator;
        }

        public void Execute()
        {
            IoC.Resolve<Interfaces.ICommand>(
                "Gameserver.UObject.SetProperty", _uobject,
                "Position", _posenumerator.Current).Execute();
            _posenumerator.MoveNext();
        }
    }
}
