using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class ArrangeShipsCommand : Interfaces.ICommand
    {
        private readonly IEnumerable<IUObject> _uobjects;

        public ArrangeShipsCommand(IEnumerable<IUObject> uobjects)
        {
            _uobjects = uobjects;
        }

        public void Execute()
        {
            var positerator = IoC.Resolve<IEnumerator<object>>("Gameserver.Position.Iterator");

            _uobjects.ToList()
                .ForEach(ship => IoC.Resolve<Interfaces.ICommand>(
                    "Gameserver.Arrange.Ship", ship, positerator).Execute());

            positerator.Reset();
        }
    }
}
