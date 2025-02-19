using Gameserver.Interfaces;
using Hwdtech;


namespace Gameserver.Commands
{
    public class CreateUObjectsCmd : Interfaces.ICommand
    {
        private readonly int _count;

        public CreateUObjectsCmd(int count)
        {
            _count = count;
        }

        public void Execute()
        {
            var uobjectset = IoC.Resolve<IDictionary<int, IUObject>>("Game.Create.UObjectSet");

            Enumerable.Range(0, _count).ToList().ForEach(
                i => uobjectset.Add(i, IoC.Resolve<IUObject>("Game.Create.UObject")));
        }
    }
}
