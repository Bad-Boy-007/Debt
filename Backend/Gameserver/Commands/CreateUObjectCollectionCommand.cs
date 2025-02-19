using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class CreateUObjectCollectionCommand : Interfaces.ICommand
    {
        private readonly int _uobjectcount;

        public CreateUObjectCollectionCommand(int uobjectcount)
        {
            _uobjectcount = uobjectcount;
        }

        public void Execute()
        {
            var uobjectmap = IoC.Resolve<IDictionary<int, IUObject>>("Gameserver.UObject.Map");

            Enumerable.Range(0, _uobjectcount).ToList().ForEach(
                i => uobjectmap.Add(i, IoC.Resolve<IUObject>("Gameserver.UObject.Create")));
        }
    }
}
