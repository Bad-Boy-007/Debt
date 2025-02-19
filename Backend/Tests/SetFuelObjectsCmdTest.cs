using Gameserver.Interfaces;
using Hwdtech.Ioc;
using Hwdtech;
using Moq;
using Gameserver.Commands;

namespace Tests
{
    public class SetFuelObjectsCmdTest
    {
        public SetFuelObjectsCmdTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void SetFuelSuccess()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.UObject.SetProperty",
                (object[] args) => new ActionCmd(
                () => ((IUObject)args[0]).SetProperty((string)args[1], args[2]))).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.UObjects.Set.Fuel",
                (object[] args) => new SetFuelUObjectsCmd((IEnumerable<IUObject>)args[0], (double)args[1])).Execute();

            var uobjects = Enumerable.Range(0, 3).Select(u =>
            {
                var mock = new Mock<IUObject>();
                mock.Setup(u => u.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
                return mock;
            }).ToList();

            uobjects.ForEach(
                mock => mock.Verify(
                    u => u.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Never()));

            IoC.Resolve<Gameserver.Interfaces.ICommand>(
                "Game.UObjects.Set.Fuel",
                uobjects.Select(x => x.Object), 10.5).Execute();

            uobjects.ForEach(
                mock => mock.Verify(
                    x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Once()));
        }
    }
}
