using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class SetShipsCmd : Interfaces.ICommand
    {
        private readonly IEnumerable<IUObject> _uobjects;

        public SetShipsCmd(IEnumerable<IUObject> uobjects)
        {
            _uobjects = uobjects;
        }

        public void Execute()
        {
            var positerator = IoC.Resolve<IEnumerator<object>>("Game.Get.Iterator");

            _uobjects.ToList()
                .ForEach(ship => IoC.Resolve<Interfaces.ICommand>(
                    "Game.Set.Ship", ship, positerator).Execute());

            positerator.Reset();
        }
    }
}
