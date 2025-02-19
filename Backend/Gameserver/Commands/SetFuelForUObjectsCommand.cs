using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class SetFuelForUObjectsCommand : Interfaces.ICommand
    {
        private readonly IEnumerable<IUObject> _uobjects;
        private readonly double _fuelamount;

        public SetFuelForUObjectsCommand(IEnumerable<IUObject> uobjects, double fuelamount)
        {
            _uobjects = uobjects;
            _fuelamount = fuelamount;
        }

        public void Execute()
        {
            _uobjects.ToList().ForEach(
                x => IoC.Resolve<Interfaces.ICommand>(
                    "Gameserver.UObject.SetProperty", x,
                    "Fuel", _fuelamount).Execute()
            );
        }
    }
}
