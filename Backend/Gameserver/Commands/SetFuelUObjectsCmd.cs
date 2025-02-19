using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class SetFuelUObjectsCmd : Interfaces.ICommand
    {
        private readonly IEnumerable<IUObject> _uobjects;
        private readonly double _fuel;

        public SetFuelUObjectsCmd(IEnumerable<IUObject> uobjects, double fuel)
        {
            _uobjects = uobjects;
            _fuel = fuel;
        }

        public void Execute()
        {
            _uobjects.ToList().ForEach(
                uobject => IoC.Resolve<Interfaces.ICommand>(
                    "Game.UObject.SetProperty", uobject,
                    "Fuel", _fuel).Execute()
            );
        }
    }
}
